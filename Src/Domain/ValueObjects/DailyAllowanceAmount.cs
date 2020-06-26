namespace Domain.ValueObjects
{
    public class DailyAllowanceAmount
    {
        public int DaysLessThan4hours { get; set; }
        public int DaysMoreThan4hours { get; set; }
    }
}