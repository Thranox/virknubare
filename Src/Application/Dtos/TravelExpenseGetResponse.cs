using System.Collections.Generic;

namespace Application.Dtos
{
    public class TravelExpenseGetResponse
    {
        public IEnumerable<TravelExpenseInListDto> Result { get; set; }
    }
}