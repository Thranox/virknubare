using System.Collections.Generic;

namespace Web.ApiModels
{
    public class TravelExpenseGetResponse
    {
        public IEnumerable<TravelExpenseDto> Result { get; set; }
    }
}