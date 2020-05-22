using System;
using System.Collections.Generic;
using CSharpFunctionalExtensions;
using Newtonsoft.Json;

namespace Application.Dtos
{
    public class FlowStepDto : ValueObject
    {
        public Guid FlowStepId { get; set; }
        public string CustomerName { get; set; }
        public Guid CustomerId { get; set; }
        public string Key { get; set; }
        public string FromStageText { get; set; }
        public Guid FromStageId { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }

        public override IEnumerable<object> GetEqualityComponents()
        {
            yield return CustomerName;
            yield return CustomerId;
            yield return Key;
            yield return FromStageText;
            yield return FromStageId;
        }
    }
}