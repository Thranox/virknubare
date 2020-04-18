using Domain.SharedKernel;

namespace Application.Dtos
{
    public class FlowStepDto : ValueObject
    {
        public string Key { get; set; }
        public string From { get; set; }
    }
}