using System;
using System.Threading.Tasks;

namespace Kata
{
    public abstract class KataStepBase
    {
        private IClientContext _clientContext;

        protected KataStepBase(IClientContext clientContext)
        {
            _clientContext = clientContext;
        }

        public async Task ExecuteAndVerifyAsync(string nameOfLoggedInUser,
            Func<IClientContext, bool> verificationFunc)
        {
            await Execute(nameOfLoggedInUser);
            verificationFunc(_clientContext);
        }

        protected abstract Task Execute(string nameOfLoggedInUser);
    }
}