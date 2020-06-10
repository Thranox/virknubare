using IdentityServer4.Models;
using IdentityServer4.Services;
using IdentityServer4.Stores;
using IdentityServerAspNetIdentit.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using IdentityModel;
using Serilog;
using SharedWouldBeNugets;

namespace IdentityServer4.Quickstart.UI
{
    [SecurityHeaders]
    [AllowAnonymous]
    public class RegisterController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IIdentityServerInteractionService _interaction;
        private readonly IClientStore _clientStore;
        private readonly IAuthenticationSchemeProvider _schemeProvider;
        private readonly IEventService _events;

        public RegisterController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IIdentityServerInteractionService interaction,
            IClientStore clientStore,
            IAuthenticationSchemeProvider schemeProvider,
            IEventService events)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _interaction = interaction;
            _clientStore = clientStore;
            _schemeProvider = schemeProvider;
            _events = events;
        }

        /// <summary>
        /// Entry point into the login workflow
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> NewUser(string returnUrl)
        {
            // build a model so we know what to show on the login page
            var vm = await BuildRegisterViewModelAsync(returnUrl);

            return View(vm);
        }

        private async Task<RegisterInputModel> BuildRegisterViewModelAsync(string returnUrl)
        {
            await Task.CompletedTask;

            return new RegisterInputModel
            {
                ReturnUrl=returnUrl
            };
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
                var user = _userManager.FindByNameAsync(model.UserName).Result;
                if (user == null)
                {
                    user = new ApplicationUser
                    {
                        UserName = model.UserName
                    };
                    var result = _userManager
                        .CreateAsync(user, model.Password)
                        .Result;

                    if (!result.Succeeded) throw new Exception(result.Errors.First().Description);

                    var enumerable = new List<Claim>()
                    {
                        new Claim(JwtClaimTypes.Email, model.Email),
                        new Claim(JwtClaimTypes.EmailVerified, "false", ClaimValueTypes.Boolean),
                    };
                    result = _userManager
                        .AddClaimsAsync(user, enumerable)
                        .Result;

                    if (!result.Succeeded) throw new Exception(result.Errors.First().Description);

                    Log.Debug(model.UserName + " created");

                    _emailFactory.CreateConfirmationEmail()

                    Log.Debug("Email send to "+model.UserName );

                    return View("Success");
                }

                Log.Debug(model.UserName + " already exists");
                return View("UserAlreadyExisted");
            }

            // something went wrong, show form with error
            var vm = await BuildRegisterViewModelAsync(model.ReturnUrl);
            return View(vm);
        }
    }

    public class RegisterInputModel
    {
        public string UserName { get; set; }
        public string ReturnUrl { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
    }
}