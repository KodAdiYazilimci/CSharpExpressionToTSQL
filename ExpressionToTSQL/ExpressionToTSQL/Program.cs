using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

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

            expressionResults.Clear();
            Expression<Func<SampleClass, bool>> expressionWithAnd = (x => x.Name == "Foo" && x.Name.Length == 3);
            expressionResults = GetExpressions(expressionWithAnd.Body as BinaryExpression, expressionResults);

        }

        private static List<ExpressionResult> GetExpressions(BinaryExpression binaryExpression, List<ExpressionResult> toExpressionList)
        {
            if (binaryExpression.NodeType == ExpressionType.Equal)
            {
                ExpressionResult expressionResult = new ExpressionResult();                                                     

                MemberExpression memberExpression = binaryExpression.Left as MemberExpression;

                if (memberExpression.Expression != null && memberExpression.Expression.Type == typeof(SampleClass))
                {
                    expressionResult.MemberName = memberExpression.Member.Name;                                               // Name
                }
                else
                {
                    expressionResult.MemberName = (memberExpression.Expression as MemberExpression).Member.Name;              // Name
                    expressionResult.SubProperty = memberExpression.Member.Name;                                              // Name.Length (Name.Length == 3)
                }

                expressionResult.Condition = binaryExpression.NodeType;                                      // ==
                expressionResult.Value = (binaryExpression.Right as ConstantExpression).Value.ToString();    // Foo

                toExpressionList.Add(expressionResult);
            }
            else if (binaryExpression.NodeType == ExpressionType.OrElse)
            {
                GetExpressions(binaryExpression.Left as BinaryExpression, toExpressionList);                // Name == Foo
                toExpressionList.Add(new ExpressionResult() { Condition = ExpressionType.Or });             // Or
                GetExpressions(binaryExpression.Right as BinaryExpression, toExpressionList);               // Name == Goo
            }
            else if (binaryExpression.NodeType == ExpressionType.AndAlso)
            {
                GetExpressions(binaryExpression.Left as BinaryExpression, toExpressionList);
                toExpressionList.Add(new ExpressionResult() { Condition = ExpressionType.And });
                GetExpressions(binaryExpression.Right as BinaryExpression, toExpressionList);
            }

            return toExpressionList;
        }
    }
}
