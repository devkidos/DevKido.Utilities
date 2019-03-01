using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevKido.Utilities.Error
{
    public class ErrorModel
    {
        public int ErrorId { get; set; }
        public string SqlQuery { get; set; }
        public string SqlParam { get; set; }
        public string CommandType { get; set; }
        public decimal TotalSeconds { get; set; } = 0;
        public string Exception { get; set; }
        public string InnerException { get; set; }
        public int RequestId { get; set; }
        public string FileName { get; set; } = "";
        public string MethodName { get; set; } = "";
        public DateTime CreateDate { get; set; } = DateTime.Now;
        public bool Active { get; set; }
        public string UserIPAddress { get; set; }
        public string UserOS { get; set; }
        public string UserLocation { get; set; }
        public string PageName { get; set; }
        public string ErrorType { get; set; }
        public string RequestURL { get; set; }
        public string UserAgent { get; set; }
        public string UserName { get; set; }
    }
}
