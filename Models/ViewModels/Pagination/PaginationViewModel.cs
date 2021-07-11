using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineConsulting.Models.ViewModels.Pagination
{
    public class PaginationViewModel
    {
        public int PageIndex { get; set; }
        public int TotalPages { get; set; }

        public bool HasPreviousPage { get; set; }

        public bool HasNextPage { get; set; }

        public List<int> SurroundingIndexes { get; set; }
    }
}
