using System;
using System.Collections.Generic;
using System.Text;

namespace ExpressionToTSQL.Model
{
    public class OrderByExpressionResult
    {
        public string MemberName { get; set; }
        public bool IsAscending { get; set; }
    }
}
