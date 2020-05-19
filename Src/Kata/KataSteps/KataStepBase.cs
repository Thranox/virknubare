using System;
using System.Threading.Tasks;

namespace Kata.KataSteps
{
    public abstract class KataStepBase
    {
        protected IClientContext ClientContext;

        protected KataStepBase(IClientContext clientContext)
        {
            ClientContext = clientContext;
        }

        public async Task ExecuteAndVerifyAsync(string nameOfLoggedInUser,
            Func<IClientContext, bool> verificationFunc)
        {
            Console.WriteLine("Executing as " +nameOfLoggedInUser);
            
            await Execute(nameOfLoggedInUser);
            var valid = verificationFunc(ClientContext);

            if(!valid)
                throw new InvalidOperationException(
                    $"Running as {nameOfLoggedInUser},alidation failed in {GetType().FullName}"
                    );
        }

        protected abstract Task Execute(string nameOfLoggedInUser);
    }
}