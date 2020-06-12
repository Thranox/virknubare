using System;
using Domain.SharedKernel;

namespace Domain.Entities
{
    public class SubmissionEntity : BaseEntity
    {
        public SubmissionEntity()
        {
            CreationTime = DateTime.Now;
        }

        public Guid CustomerId { get; set; }
        public CustomerEntity Customer { get; set; }
        public string PathToFile { get; set; }
        public DateTime CreationTime { get; set; }
        public DateTime? SubmissionTime { get; set; }
    }
}