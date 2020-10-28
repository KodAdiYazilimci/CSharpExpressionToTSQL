using System.Linq.Expressions;

namespace ExpressionToTSQL
{
    public class ExpressionResult
    {
        public string MemberName { get; set; }
        public string Value { get; set; }
        public ExpressionType Condition { get; set; }
        public string SubProperty { get; set; }
    }
}
