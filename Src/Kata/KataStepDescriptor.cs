using System;

namespace Kata
{
    public class KataStepDescriptor
    {
        public KataStepDescriptor(string identifier)
        {
            Identifier = identifier;
            VerificationFunc = context => true; // Default to "Yes, this is ok"
        }

        public string Identifier { get; }
        public string NameOfLoggedInUser { get; set; }

        public Func<IClientContext, bool> VerificationFunc { get; set; }

        public KataStepDescriptor AsUser(string userName)
        {
            var clone = MemberwiseClone() as KataStepDescriptor;
            clone.NameOfLoggedInUser = userName;

            return clone;
        }

        public KataStepDescriptor WithVerification(Func<IClientContext, bool> verificationFunc)
        {
            var clone = MemberwiseClone() as KataStepDescriptor;
            clone.VerificationFunc = verificationFunc;

            return clone;
        }
    }
}