using System.Collections.Generic;
using Domain.SharedKernel;

namespace Application.Dtos
{
    public class PlaceDto:ValueObject
    {
        public override IEnumerable<object> GetEqualityComponents()
        {
            yield return Street;
            yield return StreetNumber;
            yield return ZipCode;
        }

        public string Street { get; set; }
        public string StreetNumber { get; set; }
        public string ZipCode { get; set; }
    }
}