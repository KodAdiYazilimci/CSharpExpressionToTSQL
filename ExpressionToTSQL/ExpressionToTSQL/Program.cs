using System;
using System.Linq.Expressions;

namespace ExpressionToTSQL
{
    class Program
    {
        static void Main(string[] args)
        {
            Expression<Func<SampleClass, bool>> expression = (x => x.Name == "Foo");

            var expressionResult = GetExpression(expression.Body as BinaryExpression);
        }

        private static ExpressionResult GetExpression(BinaryExpression binaryExpression)
        {
            return new ExpressionResult()
            {
                MemberName = (binaryExpression.Left as MemberExpression).Member.Name,
                Value = (binaryExpression.Right as ConstantExpression).Value.ToString(),
                Condition = binaryExpression.NodeType.ToString()
            };
        }
    }
}
