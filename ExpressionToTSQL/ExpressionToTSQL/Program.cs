using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace ExpressionToTSQL
{
    class Program
    {
        static void Main(string[] args)
        {
            List<ExpressionResult> expressionResults = new List<ExpressionResult>();

            Expression<Func<SampleClass, bool>> expression = (x => x.Name == "Foo");
            expressionResults = GetExpressions(expression.Body as BinaryExpression, expressionResults);

            expressionResults.Clear();

            Expression<Func<SampleClass, bool>> expressionWithOr = (x => x.Name == "Foo" || x.Name == "Goo");
            expressionResults = GetExpressions(expressionWithOr.Body as BinaryExpression, expressionResults);
        }

        private static List<ExpressionResult> GetExpressions(BinaryExpression binaryExpression, List<ExpressionResult> toExpressionList)
        {
            if (binaryExpression.NodeType == ExpressionType.Equal)
            {
                toExpressionList.Add(new ExpressionResult()
                {
                    MemberName = (binaryExpression.Left as MemberExpression).Member.Name,       // Name
                    Condition = binaryExpression.NodeType,                           // ==
                    Value = (binaryExpression.Right as ConstantExpression).Value.ToString(),    // Foo
                });
            }
            else if (binaryExpression.NodeType == ExpressionType.OrElse)
            {
                GetExpressions(binaryExpression.Left as BinaryExpression, toExpressionList);                // Name == Foo
                toExpressionList.Add(new ExpressionResult() { Condition = ExpressionType.Or });  // Or
                GetExpressions(binaryExpression.Right as BinaryExpression, toExpressionList);               // Name == Goo
            }

            return toExpressionList;
        }
    }
}
