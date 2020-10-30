using ExpressionToTSQL.Model;

using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace ExpressionToTSQL.Util
{
    /// <summary>
    /// The examination operations of expressions
    /// </summary>
    public class ExpressionUtil
    {
        /// <summary>
        /// Represents a meaningful result from expressions 
        /// </summary>
        /// <typeparam name="T">The type of the expression which belong to</typeparam>
        /// <param name="methodCallExpression">Expression which will resolve</param>
        /// <param name="toExpressionList">Where store the sub-expressions (generally same result object)</param>
        /// <returns></returns>
        public static List<ExpressionResult> GetExpressions<T>(object expression, List<ExpressionResult> toExpressionList) //this method may be call recursive for sub-expressions
        {
            if (expression is BinaryExpression) // x.Name == "Foo" // for equality statement of the property
            {
                BinaryExpression binaryExpression = expression as BinaryExpression;
                toExpressionList.AddRange(Extract<T>(binaryExpression));
            }
            else if (expression is MethodCallExpression) //x.Name.StartsWith(F) // the property calls any method
            {
                MethodCallExpression methodCallExpression = expression as MethodCallExpression;
                toExpressionList.AddRange(Extract<T>(methodCallExpression));
            }
            else if (expression is LambdaExpression) //stringListItems.Contains(x.Name) // the property will be searched in a array
            {
                LambdaExpression lambdaExpression = expression as LambdaExpression;

                if (lambdaExpression.Body is MethodCallExpression)
                {
                    MethodCallExpression methodCallExpression = lambdaExpression.Body as MethodCallExpression;
                    toExpressionList.AddRange(Extract<T>(methodCallExpression));
                }
                else if (lambdaExpression.Body is UnaryExpression)
                {
                    UnaryExpression unaryExpression = lambdaExpression.Body as UnaryExpression;

                    if (unaryExpression.NodeType == ExpressionType.Not)
                    {
                        if (unaryExpression.Operand is MethodCallExpression)
                        {
                            MethodCallExpression methodCallExpression = unaryExpression.Operand as MethodCallExpression;
                            toExpressionList.Add(new ExpressionResult() { Parentheses = "!" });
                            toExpressionList.Add(new ExpressionResult() { Parentheses = "(" });
                            toExpressionList.AddRange(Extract<T>(methodCallExpression));
                            toExpressionList.Add(new ExpressionResult() { Parentheses = ")" });
                        }
                    }
                }
            }
            else if (expression is UnaryExpression) //Not((x.Name == "Foo") // for "not equal" statement of property
            {
                UnaryExpression unaryExpression = expression as UnaryExpression;

                if (unaryExpression.NodeType == ExpressionType.Not) //Not((x.Name == "Foo") // for "not equal" statement of property
                {
                    if (unaryExpression.Operand is MethodCallExpression) // Not(x.Name.StartsWith(F))  // for "not equal" statement of property which calls a method
                    {
                        MethodCallExpression methodCallExpression = unaryExpression.Operand as MethodCallExpression;
                        toExpressionList.Add(new ExpressionResult() { Parentheses = "!" });
                        toExpressionList.Add(new ExpressionResult() { Parentheses = "(" });
                        toExpressionList.AddRange(Extract<T>(methodCallExpression));
                        toExpressionList.Add(new ExpressionResult() { Parentheses = ")" });
                    }
                    else if (unaryExpression.Operand is BinaryExpression) // Not((x.Name == "Foo") // for "not equal" statement of property
                    {
                        BinaryExpression binaryExpression = unaryExpression.Operand as BinaryExpression;
                        toExpressionList.Add(new ExpressionResult() { Parentheses = "!" });
                        toExpressionList.Add(new ExpressionResult() { Parentheses = "(" });
                        toExpressionList.AddRange(Extract<T>(binaryExpression));
                        toExpressionList.Add(new ExpressionResult() { Parentheses = ")" });
                    }
                }
            }

            return toExpressionList;
        }

        /// <summary>
        /// Extract a part of expression or sub-expression
        /// </summary>
        /// <typeparam name="T">The type of the expression which belong to</typeparam>
        /// <param name="methodCallExpression">The call of method expression which will resolve</param>
        /// <returns></returns>
        private static List<ExpressionResult> Extract<T>(MethodCallExpression methodCallExpression)
        {
            List<ExpressionResult> expressionResults = new List<ExpressionResult>();

            if (methodCallExpression.NodeType == ExpressionType.Call && methodCallExpression.Object is MemberExpression) //x.Name.StartsWith(F) or stringListItems.Contains(x.Name)
            {
                ExpressionResult expressionResult = new ExpressionResult();
                expressionResult.SubProperty = methodCallExpression.Method.Name;

                if (methodCallExpression.Object is MemberExpression)
                {
                    expressionResult.MemberName = (methodCallExpression.Object as MemberExpression).Member.Name; // x.Name.StartsWith(F) => "Name" or stringListItems.Contains(x.Name) => stringListItems
                }
                if (methodCallExpression.Arguments.FirstOrDefault() is MemberExpression)
                {
                    expressionResult.MemberName = (methodCallExpression.Arguments.FirstOrDefault() as MemberExpression).Member.Name; // stringListItems.Contains(x.Name) => "Name"
                }

                MemberExpression memberExpression = methodCallExpression.Object as MemberExpression;

                Expression subExpression = memberExpression.Expression;
                string expressionParameterKeyword = memberExpression.Member.Name;

                if (subExpression is ConstantExpression) //stringListItems.Contains(x.Name)
                {
                    object subExpressionValue = (subExpression as ConstantExpression).Value;

                    if (methodCallExpression.Object.Type == typeof(List<string>))
                    {
                        List<string> values = subExpressionValue.GetType().GetField(expressionParameterKeyword).GetValue(subExpressionValue) as List<string>;
                        expressionResult.SubPropertyArgumentType = typeof(string);
                        expressionResult.SubPropertyArguments.AddRange(values); // stringListItems.Contains(x.Name) => elements of "stringListItems"
                    }
                    else if (methodCallExpression.Object.Type == typeof(List<int>))
                    {
                        List<int> values = subExpressionValue.GetType().GetField(expressionParameterKeyword).GetValue(subExpressionValue) as List<int>;
                        expressionResult.SubPropertyArgumentType = typeof(int);
                        expressionResult.SubPropertyArguments.AddRange(values.Select(x => x.ToString()).ToList());
                    }
                }
                else // x.Name.StartsWith(F)
                {
                    if (methodCallExpression.Arguments.FirstOrDefault() is ConstantExpression)
                    {
                        ConstantExpression constantExpression = methodCallExpression.Arguments.FirstOrDefault() as ConstantExpression;

                        expressionResult.SubPropertyArgumentType = constantExpression.Value.GetType();
                        expressionResult.Value = constantExpression.Value.ToString(); // x.Name.StartsWith(F) => "F"

                    }
                }

                expressionResults.Add(expressionResult);
            }
            else if (methodCallExpression.NodeType == ExpressionType.Call && methodCallExpression.Method.DeclaringType == typeof(Enumerable)) //stringArrayItems.Contains(x.Name)
            {
                ExpressionResult expressionResult = new ExpressionResult();
                expressionResult.SubProperty = methodCallExpression.Method.Name;

                Expression parameterExpression = methodCallExpression.Arguments.FirstOrDefault(x => x is MemberExpression && (x as MemberExpression).Expression.NodeType == ExpressionType.Parameter);

                expressionResult.MemberName = (parameterExpression as MemberExpression).Member.Name;

                Expression constantExpression = methodCallExpression.Arguments.FirstOrDefault(x => x is MemberExpression && (x as MemberExpression).Expression.NodeType == ExpressionType.Constant);

                MemberExpression memberExpression = constantExpression as MemberExpression;

                string expressionParameterKeyword = memberExpression.Member.Name;

                if (memberExpression.Type == typeof(string[]))
                {
                    object values = (memberExpression.Expression as ConstantExpression).Value;
                    object items = values.GetType().GetField(expressionParameterKeyword).GetValue(values);

                    expressionResult.SubPropertyArguments.AddRange((items as string[])); // stringArrayItems.Contains(x.Name) => elements of "stringArrayItems"
                    expressionResult.SubPropertyArgumentType = typeof(string);
                }
                else if (memberExpression.Type == typeof(int[]))
                {
                    object values = (memberExpression.Expression as ConstantExpression).Value;
                    object items = values.GetType().GetField(expressionParameterKeyword).GetValue(values);

                    expressionResult.SubPropertyArguments.AddRange((items as int[]).Select(x => x.ToString()).ToList());
                    expressionResult.SubPropertyArgumentType = typeof(int);
                }

                expressionResults.Add(expressionResult);
            }

            return expressionResults;
        }

        /// <summary>
        /// Extract a part of expression or sub-expression
        /// </summary>
        /// <typeparam name="T">The type of the expression which belong to</typeparam>
        /// <param name="binaryExpression">The binary expression which will resolve</param>
        /// <returns></returns>
        private static List<ExpressionResult> Extract<T>(BinaryExpression binaryExpression)
        {
            List<ExpressionResult> expressionResults = new List<ExpressionResult>();

            ExpressionResult expressionResult = new ExpressionResult();

            if (
                    binaryExpression.NodeType == ExpressionType.Equal
                    ||
                    binaryExpression.NodeType == ExpressionType.NotEqual
                    ||
                    binaryExpression.NodeType == ExpressionType.LessThan
                    ||
                    binaryExpression.NodeType == ExpressionType.GreaterThan
                    ||
                    binaryExpression.NodeType == ExpressionType.LessThanOrEqual
                    ||
                    binaryExpression.NodeType == ExpressionType.GreaterThanOrEqual)
            {
                if (binaryExpression.Left is MemberExpression)
                {
                    MemberExpression memberExpression = binaryExpression.Left as MemberExpression;

                    if (memberExpression.Expression != null && memberExpression.Expression.Type == typeof(T))
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
                    expressionResult.SubPropertyArgumentType = binaryExpression.Right.Type;
                    expressionResults.Add(expressionResult);
                }
                else if (binaryExpression.Left is MethodCallExpression)
                {
                    MethodCallExpression methodCallExpression = binaryExpression.Left as MethodCallExpression;
                    expressionResult.MemberName = (methodCallExpression.Object as MemberExpression).Member.Name;    //Name                        

                    expressionResult.SubProperty = methodCallExpression.Method.Name;                                    // ToLower
                    if (methodCallExpression.Arguments != null && methodCallExpression.Arguments.Any())
                    {
                        expressionResult.SubPropertyArguments.AddRange(methodCallExpression.Arguments.Select(x => (x as ConstantExpression).Value).ToList());
                    }
                    expressionResult.Condition = binaryExpression.NodeType;                                      // ==
                    expressionResult.Value = (binaryExpression.Right as ConstantExpression).Value.ToString();    // Foo
                    expressionResult.SubPropertyArgumentType = binaryExpression.Right.Type;
                    expressionResults.Add(expressionResult);
                }
            }
            else if (binaryExpression.NodeType == ExpressionType.OrElse)
            {
                expressionResults.Add(new ExpressionResult() { Parentheses = "(" });                         // (
                GetExpressions<T>(binaryExpression.Left as BinaryExpression, expressionResults);                // Name == Goo
                expressionResults.Add(new ExpressionResult() { Condition = ExpressionType.Or });             // Or
                GetExpressions<T>(binaryExpression.Right as BinaryExpression, expressionResults);               // Year == 2020
                expressionResults.Add(new ExpressionResult() { Parentheses = ")" });                         // )
            }
            else if (binaryExpression.NodeType == ExpressionType.AndAlso)
            {
                expressionResults.Add(new ExpressionResult() { Parentheses = "(" });                         // (
                GetExpressions<T>(binaryExpression.Left as BinaryExpression, expressionResults);                // (Name == Foo Or || Name == Goo)
                expressionResults.Add(new ExpressionResult() { Condition = ExpressionType.And });            // And
                GetExpressions<T>(binaryExpression.Right as BinaryExpression, expressionResults);               // Year == 2020
                expressionResults.Add(new ExpressionResult() { Parentheses = ")" });                         // )
            }

            return expressionResults;
        }
    }
}
