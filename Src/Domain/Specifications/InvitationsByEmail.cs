using System;
using System.Linq.Expressions;
using Domain.Entities;
using Domain.Interfaces;
using Domain.ValueObjects;

namespace Domain.Specifications
{
    public class InvitationsByEmail : ISpecification<InvitationEntity>
    {
        public InvitationsByEmail(string email)
        {
            var lowerCaseEmail = email.ToLower();
            Criteria = e =>e.InvitationState == InvitationState.Invited && e.Email == lowerCaseEmail;
        }

        public Expression<Func<InvitationEntity, bool>> Criteria { get; }
    }
}