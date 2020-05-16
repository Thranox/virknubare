namespace SharedWouldBeNugets
{
    public class PolUserCapabilities
    {
        public bool CanCreateTravelExpense { get; internal set; }
        public PolUserCapabilities AddCanCreateTravelExpense()
        {
            var clone = MemberwiseClone() as PolUserCapabilities;
            clone.CanCreateTravelExpense = true;
            return clone;
        }
    }
}