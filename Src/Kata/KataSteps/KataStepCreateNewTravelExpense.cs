using System;
using System.Linq;
using System.Threading.Tasks;
using Application.Dtos;
using Domain.ValueObjects;
using Newtonsoft.Json;
using RestSharp;
using Serilog;

namespace Kata.KataSteps
{
    public class KataStepCreateNewTravelExpense : KataStepBase, IKataStep
    {
        private readonly ILogger _logger;
        private readonly IRestClientProvider _restClientProvider;

        public KataStepCreateNewTravelExpense(ILogger logger, IRestClientProvider restClientProvider,
            IClientContext clientContext) : base(clientContext)
        {
            _logger = logger;
            _restClientProvider = restClientProvider;
        }

        public bool CanHandle(string kataStepIdentifier)
        {
            return kataStepIdentifier == "CreateNewTravelExpense";
        }

        protected override async Task Execute(string nameOfLoggedInUser)
        {
            // Still alice, create new Travel Expense
            _logger.Debug("Creating TravelExpense...");
            var restClient = _restClientProvider.GetRestClient(nameOfLoggedInUser);
            var customerId = ClientContext.UserInfoGetResponse.UserCustomerInfo
                .First(x => x.UserCustomerStatus != (int)UserStatus.Initial).CustomerId;
            var lossOfEarningSpecDtos = ClientContext.UserInfoGetResponse.UserCustomerInfo.Single(x=>x.CustomerId==customerId).LossOfEarningSpecs;
            var restRequest = new RestRequest(
                    new Uri("/travelexpenses", UriKind.Relative)
                )
                .AddJsonBody(new TravelExpenseCreateDto
                {
                    Description = "From kata",
                    CustomerId = customerId,
                    DailyAllowanceAmount = new DailyAllowanceAmountDto
                    {
                        DaysLessThan4hours = 2,
                        DaysMoreThan4hours = 3
                    },
                    DestinationPlace = new PlaceDto
                    {
                        Street = "Jegstrupvænget",
                        StreetNumber = "269",
                        ZipCode = "8310"
                    },
                    FoodAllowances = new FoodAllowancesDto
                    {
                        Morning = 1,
                        Lunch = 1,
                        Dinner = 1
                    },
                    TransportSpecification = new TransportSpecificationDto
                    {
                        KilometersCustom = 42,
                        KilometersCalculated = 43,
                        KilometersOverTaxLimit = 5,
                        KilometersTax = 7,
                        Method = "Method",
                        NumberPlate = "AX68276"
                    },
                    Purpose = "Purpose",
                    ArrivalDateTime = DateTime.Today,
                    DepartureDateTime = DateTime.Today.AddDays(3),
                    IsAbsenceAllowance = true,
                    IsEducationalPurpose = true,
                    Expenses = 42.42,
                    CommitteeId = 42,
                    LossOfEarnings =new []
                    {
                        new LossOfEarningDto{NumberOfHours = 3, Date = DateTime.Today, LossOfEarningSpecId = lossOfEarningSpecDtos[0].Id},
                        new LossOfEarningDto{NumberOfHours = 1, Date = DateTime.Today, LossOfEarningSpecId = lossOfEarningSpecDtos[1].Id},
                        new LossOfEarningDto{NumberOfHours = 0, Date = DateTime.Today, LossOfEarningSpecId = lossOfEarningSpecDtos[2].Id}
                    }
                });
            var travelExpenseCreateResponse = await restClient.PostAsync<TravelExpenseCreateResponse>(restRequest);
            ClientContext.TravelExpenseCreateResponse = travelExpenseCreateResponse;
            _logger.Debug("Created TravelExpense {travelExpenseCreateResponse}",
                JsonConvert.SerializeObject(travelExpenseCreateResponse));
        }
    }
}