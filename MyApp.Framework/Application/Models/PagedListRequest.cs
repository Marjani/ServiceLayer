using MyApp.Framework.Aspects.Validation;
using MyApp.Framework.Extensions;

namespace MyApp.Framework.Application.Models
{
    public class PagedListRequest : IShouldNormalize
    {
        public long TotalCount { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }

        /// <summary>
        ///     Sorting information.
        ///     Should include sorting field and optionally a direction (ASC or DESC)
        ///     Can contain more than one field separated by comma (,).
        /// </summary>
        /// <example>
        ///     Examples:
        ///     "Name"
        ///     "Name DESC"
        ///     "Name ASC, Age DESC"
        /// </example>
        public string SortBy { get; set; }

        public void Normalize()
        {
            if (PageNumber < 1)
                PageNumber = 1;

            if (PageSize < 0)
                PageNumber = 10;

            if (SortBy.IsEmpty())
                SortBy = "Id DESC";
        }
    }
}