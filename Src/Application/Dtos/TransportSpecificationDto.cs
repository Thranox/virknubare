using System.Collections.Generic;
using Domain.SharedKernel;

namespace Application.Dtos
{
    public class TransportSpecificationDto:ValueObject
    {
        public override IEnumerable<object> GetEqualityComponents()
        {
            yield return KilometersCalculated;
            yield return KilometersCustom;
            yield return KilometersOverTaxLimit;
            yield return KilometersTax;
            yield return Method;
            yield return NumberPlate;
        }

        public int KilometersCalculated { get; set; }
        public int KilometersCustom { get; set; }
        public int KilometersOverTaxLimit { get; set; }
        public int KilometersTax { get; set; }
        public string Method { get; set; }
        public string NumberPlate { get; set; }
    }
}