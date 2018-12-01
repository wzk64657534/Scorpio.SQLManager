using System;
using System.Collections.Generic;
using System.Text;

namespace Scorpio.Core.Model
{
    public class TableStructure
    {
        public string TableName { get; set; }
        public int Num { get; set; }
        public string FieldName { get; set; }
        public int IsKey { get; set; }
        public string FieldType { get; set; }
        public int FieldLength { get; set; }
        public int DecimalDigit { get; set; }
        public int IsNull { get; set; }
        public string DefaultValue { get; set; }
        public string Explain { get; set; }
    }
}
