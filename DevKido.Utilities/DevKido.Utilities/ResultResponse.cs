//for any query or any solution in IT, please contact to anrorathod@gmail.com - +919725437729
using DevKido.Utilities.DataTable;
using System.Collections.Generic;

namespace DevKido.Utilities
{
    public class ResultResponse<T>
    {
        public bool Success { get; set; } = true;
        public string Message { get; set; }
        public DTResult<T> Data { get; set; }
        public T Datas { get; set; }
        public Dictionary<string, string> Exceptions { get; set; }
    }

    public class ResultResponses<T>
    {
        public bool Success { get; set; } = true;
        public string Message { get; set; }
        public T Data { get; set; }
        public Dictionary<string, string> Exceptions { get; set; }
    }
    public class ResultResponseSingle<T>
    {
        public bool Success { get; set; } = true;
        public string Message { get; set; }
        public T Data { get; set; }
        public Dictionary<string, string> Exceptions { get; set; }
    }
}
