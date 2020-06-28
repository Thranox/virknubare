using System;

namespace Application.Dtos
{
    public class LossOfEarningSpecDto
    {
        public Guid Id { get; set; }
        public int Rate { get; set; }
        public string Description { get; set; }
    }
}