using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using IdentityModel;
using IdentityServer4.Models;
using IdentityServer4.Quickstart.UI;
using IdentityServer4.Services;
using IdentityServer4.Stores;
using IdentityServerAspNetIdentit.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Serilog;
using SharedWouldBeNugets;

namespace IDP.Quickstart.Account
{
    [SecurityHeaders]
    [AllowAnonymous]
    public class RegisterController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IIdentityServerInteractionService _interaction;
        private readonly IClientStore _clientStore;
        private readonly IEmailFactory _emailFactory;
        private readonly IMailService _mailService;
        private readonly ILogger _logger;
        private readonly string _mailFromAddress;

        public RegisterController(
            UserManager<ApplicationUser> userManager,
            IIdentityServerInteractionService interaction,
            IClientStore clientStore, 
            IEmailFactory emailFactory, 
            IMailService mailService,
            ILogger logger,
            IConfiguration configuration)
        {
            _userManager = userManager;
            _interaction = interaction;
            _clientStore = clientStore;
            _emailFactory = emailFactory;
            _mailService = mailService;
            _logger = logger;
            _mailFromAddress = configuration.GetValue<string>("MailFromAddress");
        }

        /// <summary>
        /// Entry point into the login workflow
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> NewUser(string returnUrl)
        {
            _logger.Debug("New user requested");
            // build a model so we know what to show on the login page
            var vm = await BuildRegisterViewModelAsync(returnUrl);

            return View(vm);
        }

        private async Task<RegisterInputModel> BuildRegisterViewModelAsync(string returnUrl)
        {
            await Task.CompletedTask;

            return new RegisterInputModel
            {
                ReturnUrl = returnUrl
            };
        }

        [HttpGet]
        public async Task<IActionResult> ConfirmEmail(string email, string securityStamp)
        {
            var user = _userManager.Users.SingleOrDefault(x => x.Email == email && x.SecurityStamp == securityStamp);

            if (user == null)
            {
                ModelState.AddModelError("NotConfirmed","User email could not be confirmed. Please contact administrator.");
                return View("Error");
            }

            user.EmailConfirmed = true;
            var claims = await _userManager.GetClaimsAsync(user);
            var emailConfirmedClaim = claims.Single(x => x.Type == JwtClaimTypes.EmailVerified);

            var emailConfirmedClaimNewValue = new Claim(JwtClaimTypes.EmailVerified, "true", ClaimValueTypes.Boolean);
            await _userManager
                .ReplaceClaimAsync(user, emailConfirmedClaim,emailConfirmedClaimNewValue);
            await _userManager.UpdateAsync(user);

            return View("EmailConfirmed");
        }

        /// <summary>
        /// Handle postback from register
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> NewUser(RegisterInputModel model, string button)
        {
            // check if we are in the context of an authorization request
            var context = await _interaction.GetAuthorizationContextAsync(model.ReturnUrl);

            // the user clicked the "cancel" button
            if (button != "register")
            {
                _logger.Debug("User cancelled creation of new user.");

                if (context != null)
                {
                    // if the user cancels, send a result back into IdentityServer as if they 
                    // denied the consent (even if this client does not require consent).
                    // this will send back an access denied OIDC error response to the client.
                    await _interaction.GrantConsentAsync(context, ConsentResponse.Denied);

                    // we can trust model.ReturnUrl since GetAuthorizationContextAsync returned non-null
                    if (await _clientStore.IsPkceClientAsync(context.ClientId))
                    {
                        // if the client is PKCE then we assume it's native, so this change in how to
                        // return the response is for better UX for the end user.
                        return this.LoadingPage("Redirect", model.ReturnUrl);
                    }

                    return Redirect(model.ReturnUrl);
                }
                else
                {
                    // since we don't have a valid context, then we just go back to the home page
                    return Redirect("~/");
                }
            }

            if (ModelState.IsValid)
            {
                _logger.Debug("New user posted for creation -- model state valid.");

                var user = _userManager.FindByNameAsync(model.UserName).Result;
                if (user == null)
                {
                    user = new ApplicationUser
                    {
                        UserName = model.UserName,
                        // Set verification code to a random number
                        SecurityStamp = new Random().Next(1000000).ToString(),
                        Email = model.Email,
                        EmailConfirmed = false
                    };
                    var result = _userManager
                        .CreateAsync(user, model.Password)
                        .Result;

                    if (!result.Succeeded)
                    {
                        _logger.Error("Problem creating user in db: "+ result.Errors.First().Description);
                        ModelState.AddModelError("errorCreatingUser",result.Errors.First().Description);
                        return View(model);
                    }

                    var claims = new List<Claim>()
                    {
                        new Claim(JwtClaimTypes.Email, model.Email),
                        new Claim(JwtClaimTypes.EmailVerified, "false", ClaimValueTypes.Boolean),
                        new Claim(ImproventoGlobals.ImproventoSubClaimName, Guid.NewGuid().ToString()),
                        //new Claim(JwtClaimTypes.Name, "Alice Smith"),
                        new Claim(JwtClaimTypes.GivenName, model.FirstName),
                        new Claim(JwtClaimTypes.FamilyName, model.LastName),
                        
                        //new Claim(JwtClaimTypes.Address,
                        //    @"{ 'street_address': 'One Hacker Way', 'locality': 'Heidelberg', 'postal_code': 69118, 'country': 'Germany' }",
                        //    IdentityServerConstants.ClaimValueTypes.Json)
                    };
                    result = _userManager
                        .AddClaimsAsync(user, claims)
                        .Result;

                    if (!result.Succeeded)
                    {
                        _logger.Error("Problem creating claims in db: " + result.Errors.First().Description);
                        ModelState.AddModelError("errorCreatingClaims", result.Errors.First().Description);
                        return View(model);
                    }

                    Log.Debug(model.UserName + " created");

                    var path = GetConfirmPath(Request.Path);

                    var uriBuilder = new UriBuilder
                    {
                        Scheme = Request.Scheme,
                        Host = Request.Host.Host,
                        Path = path,
                        Query = $"email={model.Email}&securityStamp={user.SecurityStamp}"
                    };
                    if (Request.Host.Port.HasValue)
                    {
                        uriBuilder.Port = Request.Host.Port.Value;
                    }

                    _logger.Debug("Sending confirmation mail to new user: " +user.Email);
                    var confirmationEmail = _emailFactory.CreateConfirmationEmail(user, claims, uriBuilder.Uri);

                    try
                    {
                        await _mailService
                            .SendAsync(_mailFromAddress,new []{ model.Email}, confirmationEmail.Subject, confirmationEmail.Body);
                    }
                    catch (Exception e)
                    {
                        _logger.Error(e,"Problem sending email: \n" + e.Message+"\n"+e.StackTrace);
                        ModelState.AddModelError("errorCreatingClaims", "Could not create user.");
                        return View(model);
                    }

                    Log.Debug("Email send to "+model.UserName );

                    return View("Success");
                }

                Log.Debug(model.UserName + " already exists");
                return View("UserAlreadyExisted");
            }

            // something went wrong, show form with error
            _logger.Debug("New user posted for creation -- model state invalid.");
            return View(model);
        }

        private string GetConfirmPath(in PathString requestPath)
        {
            var indexOfRegister = requestPath.Value.ToLower().IndexOf("/register/");
            return requestPath.Value.Substring(0, indexOfRegister) + "/Register/ConfirmEmail";
        }
    }

    public class RegisterInputModel
    {
        [Required]
        public string UserName { get; set; }
        public string ReturnUrl { get; set; }
        [Required]
        public string Password { get; set; }
        [Required][EmailAddress]
        public string Email { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
    }
}