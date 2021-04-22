
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dashboard.Models
{
    [Table("Log")]
    public class LogModel : BaseModel
    {
        public DateTime LogTime { get; set; }
        public string AppName { get; set; }
        public string CategoryName { get; set; }
        public string EventId { get; set; }
        public string LogLevel { get; set; }
        public string Message { get; set; }
        public string Exception { get; set; }
    }
}
