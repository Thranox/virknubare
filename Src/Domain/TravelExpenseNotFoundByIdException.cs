using System;

namespace Domain
{
    public class TravelExpenseNotFoundByIdException : Exception
    {
        public Guid Id { get; }

        public TravelExpenseNotFoundByIdException(Guid id)
        {
            Id = id;
        }
    }
}