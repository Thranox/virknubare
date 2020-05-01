using System;
using Domain.SharedKernel;
using Newtonsoft.Json;

namespace Application.Dtos
{
    public class FlowStepDto : ValueObject
    {
        public string CustomerName { get; set; }
        public Guid  CustomerId { get; set; }
        public string Key { get; set; }
        public string FromStageText { get; set; }
        public Guid FromStageId { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}