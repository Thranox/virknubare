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

        public string PathToFile { get; set; }
        public DateTime CreationTime { get; set; }
        public DateTime? SubmissionTime { get; set; }
    }
}