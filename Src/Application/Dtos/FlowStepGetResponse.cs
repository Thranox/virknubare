using System.Collections.Generic;

namespace Application.Dtos
{
    public class FlowStepGetResponse
    {
        public IEnumerable<FlowStepDto> Result { get; set; }
    }
}