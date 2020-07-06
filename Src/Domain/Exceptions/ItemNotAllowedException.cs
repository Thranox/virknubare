using System;

namespace Domain.Exceptions
{
    public class ItemNotAllowedException : Exception
    {
        public ItemNotAllowedException(string id, string item):base(id+";"+item)
        {
            Item = item;
            Id = id;
        }

        public string Id { get; }

        public string Item { get; }
    }
}