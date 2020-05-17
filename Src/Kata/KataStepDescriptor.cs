namespace Kata
{
    public class KataStepDescriptor
    {
        public string Identifier { get; }
        public string NameOfLoggedInUser { get; set; }

        public KataStepDescriptor(string identifier)
        {
            Identifier = identifier;
        }

        public KataStepDescriptor AsUser(string userName)
        {
            var clone = MemberwiseClone() as KataStepDescriptor;
            clone.NameOfLoggedInUser = userName;

            return clone;
        }
    }
}