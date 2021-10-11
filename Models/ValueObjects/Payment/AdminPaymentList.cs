using System.Collections.Generic;

namespace OnlineConsulting.Models.ValueObjects.Payment
{
    public class AdminPaymentList
    {
        public List<AdminPaymentListItem> List { get; set; }
        public int PageIndex { get; set; }
        public bool HasPreviousPage { get; set; }
        public bool HasNextPage { get; set; }
        public IEnumerable<int> SurroundingIndexes { get; set; }
    }
}
