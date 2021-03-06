using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineConsulting.Tools
{
    public class PaginatedList<T> : List<T>
    {
        public int PageIndex { get; private set; }
        public int TotalPages { get; private set; }
        public bool HasPreviousPage => PageIndex > 1;
        public bool HasNextPage => PageIndex < TotalPages;

        private PaginatedList(IEnumerable<T> items, int count, int pageIndex, int pageSize)
        {
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
            PageIndex = pageIndex > TotalPages ? TotalPages : pageIndex < 1 ? 1 : pageIndex;
            AddRange(items);
        }

        public PaginatedList(IEnumerable<T> items, int totalPages, int pageIndex)
        {
            TotalPages = totalPages;
            PageIndex = pageIndex;
            AddRange(items);
        }

        public IEnumerable<int> GetSurroundingIndexes(int maxSideRange)
        {

            var nextRangeEndIndex = TotalPages >= PageIndex + maxSideRange ? PageIndex + maxSideRange : TotalPages;
            var prevRangeEndIndex = PageIndex - maxSideRange > 1 ? PageIndex - maxSideRange : 1;

            return Enumerable.Range(prevRangeEndIndex, nextRangeEndIndex - prevRangeEndIndex + 1);
        }

        public static async Task<PaginatedList<T>> CreateAsync(IQueryable<T> source, int pageIndex, int pageSize)
        {
            if (pageIndex < 1) pageIndex = 1;

            var count = await source.CountAsync();
            var items = await source.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();
            return new PaginatedList<T>(items, count, pageIndex, pageSize);
        }

    }
}
