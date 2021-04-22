
using System;

namespace Dashboard.Models
{
    public class SearchLogModel
    {
        public string AppName { get; set; } 
        public DateTime? ValidFrom { get; set; }
        public DateTime? ValidTo { get; set; }
        public string Level { get; set; }
        public int PageIndex { get; set; }
    }
}
