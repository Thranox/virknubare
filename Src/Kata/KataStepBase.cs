using System;
using System.Threading.Tasks;

namespace Kata
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
            await Execute(nameOfLoggedInUser);
            var valid = verificationFunc(ClientContext);
            if(!valid)
                throw new InvalidOperationException("Validation failed in "+this.GetType().FullName);
        }

        protected abstract Task Execute(string nameOfLoggedInUser);
    }
}