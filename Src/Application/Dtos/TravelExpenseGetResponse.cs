using System.Collections.Generic;

namespace Application.Dtos
{
    public class TravelExpenseGetResponse
    {
        public IEnumerable<TravelExpenseDto> Result { get; set; }
    }
}