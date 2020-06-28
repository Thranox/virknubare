using System;
using API.Shared.Controllers;

namespace Application.Dtos
{
    public class UserCustomerInfo
    {
        public Guid CustomerId;
        public string CustomerName;
        public string UserCustomerStatusText;
        public int UserCustomerStatus { get; set; }
        public LossOfEarningSpecDto[] LossOfEarningSpecs { get; set; }
    }
}