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

        public async Task ExecuteAndVerifyAsync(Properties properties, string nameOfLoggedInUser,
            Func<IClientContext, bool> verificationFunc)
        {
            await Execute(properties, nameOfLoggedInUser);
            verificationFunc(_clientContext);
        }

        protected abstract Task Execute(Properties properties, string nameOfLoggedInUser);
    }
}