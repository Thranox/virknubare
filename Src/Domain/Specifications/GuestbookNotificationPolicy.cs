using System;
using System.Linq.Expressions;
using CleanArchitecture.Core.Interfaces;

namespace Domain.Specifications
{
    //public class GuestbookNotificationPolicy : ISpecification<GuestbookEntry>
    //{
    //    public GuestbookNotificationPolicy(int entryAddedId = 0)
    //    {
    //        Criteria = e =>
    //            e.DateTimeCreated > DateTimeOffset.UtcNow.AddDays(-1) // created after 1 day ago
    //            && e.Id != entryAddedId; // don't notify the added entry
    //    }

    //    public Expression<Func<GuestbookEntry, bool>> Criteria { get; }
    //}
}