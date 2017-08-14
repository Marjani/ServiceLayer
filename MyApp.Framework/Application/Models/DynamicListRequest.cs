using System.Collections.Generic;

namespace MyApp.Framework.Application.Models
{
    public class DynamicListRequest
    {
        public int Take { get; set; }
        public int Skip { get; set; }
        public IEnumerable<Sort> Sorts { get; set; }
        public Filter Filter { get; set; }
    }
}