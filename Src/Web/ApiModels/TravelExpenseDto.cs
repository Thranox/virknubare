﻿using System;
using Domain.SharedKernel;

namespace Web.ApiModels
{
    public class TravelExpenseDto : ValueObject
    {
        public string Description { get; set; }
        public Guid Id { get; set; }
        public bool IsCertified { get; set; }
        public bool IsReportedDone { get; set; }
    }
}