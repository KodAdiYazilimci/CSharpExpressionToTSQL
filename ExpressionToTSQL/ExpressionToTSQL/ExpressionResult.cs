using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace ExpressionToTSQL
{
    public class ExpressionResult
    {
        public string MemberName { get; set; }
        public string Value { get; set; }
        public ExpressionType Condition { get; set; }
        public string SubProperty { get; set; }
        public List<object> SubPropertyArguments { get; set; } = new List<object>();
        public Type SubPropertyArgumentType { get; set; }
        public string Parentheses { get; set; }
    }
}
