using System.Collections;
using System.Collections.Generic;

namespace MyApp.Framework.Application.Models
{
    public class DynamicListResponse
    {
        public IEnumerable Data { get; set; }
        public long Total { get; set; }
        public object Aggregates { get; set; }
        public IList<string> Errors { get; set; }
    }
}