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

            expressionResults.Clear();
            Expression<Func<SampleClass, bool>> expressionWithParentheses = (x => x.Name == "Foo" || (x.Name == "Goo" && x.Year == 2020));
            expressionResults = GetExpressions(expressionWithParentheses.Body as BinaryExpression, expressionResults);

            expressionResults.Clear();
            expressionWithParentheses = (x => (x.Name == "Foo" || x.Name == "Goo") && x.Year == 2020);
            expressionResults = GetExpressions(expressionWithParentheses.Body as BinaryExpression, expressionResults);

            expressionResults.Clear();
            Expression<Func<SampleClass, bool>> expressionWithNotEqual = (x => x.Name != "Foo");
            expressionResults = GetExpressions(expressionWithNotEqual.Body as BinaryExpression, expressionResults);

            expressionResults.Clear();
            Expression<Func<SampleClass, bool>> expressionWithParenthesesNotEqual = (x => (x.Name != "Foo" && x.Name != "Goo") && x.Year == 2020);
            expressionResults = GetExpressions(expressionWithParenthesesNotEqual.Body as BinaryExpression, expressionResults);
        }

        private static List<ExpressionResult> GetExpressions(BinaryExpression binaryExpression, List<ExpressionResult> toExpressionList)
        {
            if (binaryExpression.NodeType == ExpressionType.Equal || binaryExpression.NodeType == ExpressionType.NotEqual)
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
                toExpressionList.Add(new ExpressionResult() { Parentheses = "(" });                         // (
                GetExpressions(binaryExpression.Left as BinaryExpression, toExpressionList);                // Name == Goo
                toExpressionList.Add(new ExpressionResult() { Condition = ExpressionType.Or });             // Or
                GetExpressions(binaryExpression.Right as BinaryExpression, toExpressionList);               // Year == 2020
                toExpressionList.Add(new ExpressionResult() { Parentheses = ")" });                         // )
            }
            else if (binaryExpression.NodeType == ExpressionType.AndAlso)
            {
                toExpressionList.Add(new ExpressionResult() { Parentheses = "(" });                         // (
                GetExpressions(binaryExpression.Left as BinaryExpression, toExpressionList);                // (Name == Foo Or || Name == Goo)
                toExpressionList.Add(new ExpressionResult() { Condition = ExpressionType.And });            // And
                GetExpressions(binaryExpression.Right as BinaryExpression, toExpressionList);               // Year == 2020
                toExpressionList.Add(new ExpressionResult() { Parentheses = ")" });                         // )
            }

            return toExpressionList;
        }
    }
}
