using System;
using System.Collections.Generic;
using System.Text;

namespace ExpressionToTSQL.Model
{
    public class JoinExpressionResult
    {
        public string FromColumnName { get; set; }
        public string TableName { get; set; }
        public string ColumnName { get; set; }
    }
}
