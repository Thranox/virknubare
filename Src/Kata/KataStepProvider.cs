using System;
using System.Collections.Generic;
using System.Linq;

namespace Kata
{
    public class KataStepProvider:IKataStepProvider
    {
        private readonly IEnumerable<IKataStep> _kataSteps;

        public KataStepProvider(IEnumerable<IKataStep> kataSteps)
        {
            _kataSteps = kataSteps;
        }
        public IKataStep GetStep(string kataStepIdentifier)
        {
            return _kataSteps.SingleOrDefault(x => x.CanHandle(kataStepIdentifier));
        }
    }
}