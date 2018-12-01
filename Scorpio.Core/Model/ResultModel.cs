using System;
using System.Collections.Generic;
using System.Text;

namespace Scorpio.Core.Model
{
    public class ResultModel<T>
    {
        public int code { get; set; }
        public string msg { get; set; }
        public int count { get; set; }
        public T data { get; set; }
    }
}
