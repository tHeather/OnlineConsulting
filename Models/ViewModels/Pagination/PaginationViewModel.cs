using System.Collections.Generic;

namespace OnlineConsulting.Models.ViewModels.Pagination
{
    public class PaginationViewModel
    {
        public int PageIndex { get; set; }
        public int TotalPages { get; set; }
        public bool HasPreviousPage { get; set; }
        public bool HasNextPage { get; set; }
        public IEnumerable<int> SurroundingIndexes { get; set; }
        public string Action { get; set; }
        public Dictionary<string,string> RouteData { get; set; }
    }
}
