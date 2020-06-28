using System;
using System.Collections.Generic;
using Domain.SharedKernel;

namespace Domain.Entities
{
    public class PayoutTable : ValueObject
    {
        private readonly List<PayoutTableRow> _rows;

        private PayoutTable()
        {
            _rows = new List<PayoutTableRow>();
        }

        public PayoutTable(string[] headers) : this()
        {
            Headers = headers;
        }

        public string[] Headers { get; }

        public object Rows => _rows.ToArray();

        public override IEnumerable<object> GetEqualityComponents()
        {
            throw new NotImplementedException();
        }

        public void AddRow(PayoutTableRow payoutTableRow)
        {
            if (payoutTableRow.Items.Length != Headers.Length)
                throw new ArgumentException("Row should contain item for each header. " + payoutTableRow.Items.Length +
                                            " vs " + Headers.Length);
            _rows.Add(payoutTableRow);
        }
    }
}