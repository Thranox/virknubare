﻿using Domain.SharedKernel;

namespace Domain.Entities
{
    public class LossOfEarningSpecEntity:BaseEntity
    {
        public int Rate { get; set; }
        public string Description { get; set; }
    }
}