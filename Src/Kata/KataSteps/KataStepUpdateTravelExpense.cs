using System;
using System.Threading.Tasks;
using Application.Dtos;
using Newtonsoft.Json;
using RestSharp;
using Serilog;

namespace Kata.KataSteps
{
    public class KataStepUpdateTravelExpense : KataStepBase, IKataStep
    {
        private readonly ILogger _logger;
        private readonly IRestClientProvider _restClientProvider;

        public KataStepUpdateTravelExpense(ILogger logger, IRestClientProvider restClientProvider,
            IClientContext clientContext) : base(clientContext)
        {
            _logger = logger;
            _restClientProvider = restClientProvider;
        }

        public bool CanHandle(string kataStepIdentifier)
        {
            return kataStepIdentifier == "UpdateTravelExpense";
        }

        protected override async Task Execute(string nameOfLoggedInUser)
        {
            // Still alice, update Travel Expense
            _logger.Debug("Updating TravelExpense...");
            var restClient = _restClientProvider.GetRestClient(nameOfLoggedInUser);
            var restRequest = new RestRequest(
                    new Uri("/travelexpenses/"+ClientContext.TravelExpenseCreateResponse.Id, UriKind.Relative)
                )
                .AddJsonBody(new TravelExpenseUpdateDto()
                {
                    Description = "From kata",
                    DailyAllowanceAmount = new DailyAllowanceAmountDto
                    {
                        DaysLessThan4hours = 1,
                        DaysMoreThan4hours = 4
                    },
                    DestinationPlace = new PlaceDto
                    {
                        Street = "Jegstrupvænget",
                        StreetNumber = "270",
                        ZipCode = "8310"
                    },
                    FoodAllowances = new FoodAllowancesDto
                    {
                        Morning = 2,
                        Lunch = 2,
                        Dinner = 2
                    },
                    TransportSpecification = new TransportSpecificationDto
                    {
                        KilometersCustom = 420,
                        KilometersCalculated = 430,
                        KilometersOverTaxLimit = 50,
                        KilometersTax = 70,
                        Method = "Methods",
                        NumberPlate = "ZULU68276"
                    },
                    Purpose = "Purpose"
                });
            var travelExpenseUpdateResponse = await restClient.PutAsync<TravelExpenseUpdateResponse>(restRequest);
            ClientContext.TravelExpenseUpdateResponse = travelExpenseUpdateResponse;
            _logger.Debug("Updated TravelExpense {travelExpenseUpdateResponse}",
                JsonConvert.SerializeObject(travelExpenseUpdateResponse));
        }
    }
}