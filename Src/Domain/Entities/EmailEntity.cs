using System;
using System.Collections.Generic;
using Domain.SharedKernel;

namespace Domain.Entities
{
    public class EmailEntity : BaseEntity
    {
        public EmailEntity()
        {
            CreationTime = DateTime.Now;
        }

        public DateTime CreationTime { get; set; }
        public DateTime? SendTime { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public string[] Recievers { get; set; }
    }
}