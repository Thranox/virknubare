﻿using System;
using Domain.SharedKernel;

namespace Web.ApiModels
{
    public class TravelExpenseApproveDto:ValueObject
    {
        public Guid Id { get; set; }
    }
}