﻿using System;

namespace Domain.Exceptions
{
    public class ItemNotFoundException : Exception
    {
        public ItemNotFoundException(string id, string item)
        {
            Item = item;
            Id = id;
        }

        public string Id { get; }

        public string Item { get; set; }
    }
}