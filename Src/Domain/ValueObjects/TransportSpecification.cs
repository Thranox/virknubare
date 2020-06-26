namespace Domain.ValueObjects
{
    public class TransportSpecification
    {
        public string Method { get; set; }
        public int KilometersCalculated { get; set; }
        public int KilometersCustom { get; set; }
        public int KilometersTax { get; set; }
        public int KilometersOverTaxLimit { get; set; }
        public string NumberPlate { get; set; }
    }
}