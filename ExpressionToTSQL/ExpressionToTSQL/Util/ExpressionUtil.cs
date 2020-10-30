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
        /// Represents a meaningful result from where expressions 
        /// </summary>
        /// <typeparam name="T">The type of the expression which belong to</typeparam>
        /// <param name="methodCallExpression">Expression which will resolve</param>
        /// <param name="toExpressionList">Where store the sub-expressions (generally same result object)</param>
        /// <param name="expression">The expression of where statement</param>
        /// <returns></returns>
        public static List<WhereExpressionResult> GetWhereExpressions<T>(object expression, List<WhereExpressionResult> toExpressionList) //this method may be call recursive for sub-expressions
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
                            toExpressionList.Add(new WhereExpressionResult() { Parentheses = "!" });
                            toExpressionList.Add(new WhereExpressionResult() { Parentheses = "(" });
                            toExpressionList.AddRange(Extract<T>(methodCallExpression));
                            toExpressionList.Add(new WhereExpressionResult() { Parentheses = ")" });
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
                        toExpressionList.Add(new WhereExpressionResult() { Parentheses = "!" });
                        toExpressionList.Add(new WhereExpressionResult() { Parentheses = "(" });
                        toExpressionList.AddRange(Extract<T>(methodCallExpression));
                        toExpressionList.Add(new WhereExpressionResult() { Parentheses = ")" });
                    }
                    else if (unaryExpression.Operand is BinaryExpression) // Not((x.Name == "Foo") // for "not equal" statement of property
                    {
                        BinaryExpression binaryExpression = unaryExpression.Operand as BinaryExpression;
                        toExpressionList.Add(new WhereExpressionResult() { Parentheses = "!" });
                        toExpressionList.Add(new WhereExpressionResult() { Parentheses = "(" });
                        toExpressionList.AddRange(Extract<T>(binaryExpression));
                        toExpressionList.Add(new WhereExpressionResult() { Parentheses = ")" });
                    }
                }
            }

            return toExpressionList;
        }

        /// <summary>
        /// Represents a meaningful result from order by expressions 
        /// </summary>
        /// <typeparam name="T">The type of the expression which belong to</typeparam>
        /// <param name="expression">The expression of order by statement</param>
        /// <returns></returns>
        public static OrderByExpressionResult GetOrderByExpression<T>(object expression)
        {
            OrderByExpressionResult orderByExpressionResults = new OrderByExpressionResult();

            if (expression is LambdaExpression)
            {
                LambdaExpression lambdaExpression = expression as LambdaExpression;

                if (lambdaExpression.Body is MemberExpression)
                {
                    MemberExpression memberExpression = lambdaExpression.Body as MemberExpression;

                    orderByExpressionResults.MemberName = memberExpression.Member.Name;
                }
            }
            else if (expression is MemberExpression)
            {
                MemberExpression memberExpression = expression as MemberExpression;

                if (memberExpression.NodeType == ExpressionType.MemberAccess)
                    orderByExpressionResults.MemberName = memberExpression.Member.Name;
                else if (memberExpression.NodeType == ExpressionType.Convert)
                    orderByExpressionResults.MemberName = "";
            }
            else if (expression is UnaryExpression)
            {
                UnaryExpression unaryExpression = expression as UnaryExpression;

                if (unaryExpression.Operand is MemberExpression)
                {
                    MemberExpression memberExpression = unaryExpression.Operand as MemberExpression;

                    orderByExpressionResults.MemberName = memberExpression.Member.Name;
                }
            }

            return orderByExpressionResults;
        }

        /// <summary>
        /// Extract a part of expression or sub-expression
        /// </summary>
        /// <typeparam name="T">The type of the expression which belong to</typeparam>
        /// <param name="methodCallExpression">The call of method expression which will resolve</param>
        /// <returns></returns>
        private static List<WhereExpressionResult> Extract<T>(MethodCallExpression methodCallExpression)
        {
            List<WhereExpressionResult> expressionResults = new List<WhereExpressionResult>();

            if (methodCallExpression.NodeType == ExpressionType.Call && methodCallExpression.Object is MemberExpression) //x.Name.StartsWith(F) or stringListItems.Contains(x.Name)
            {
                WhereExpressionResult expressionResult = new WhereExpressionResult();
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
                WhereExpressionResult expressionResult = new WhereExpressionResult();
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
        private static List<WhereExpressionResult> Extract<T>(BinaryExpression binaryExpression)
        {
            List<WhereExpressionResult> expressionResults = new List<WhereExpressionResult>();

            WhereExpressionResult expressionResult = new WhereExpressionResult();

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
                expressionResults.Add(new WhereExpressionResult() { Parentheses = "(" });                         // (
                GetWhereExpressions<T>(binaryExpression.Left as BinaryExpression, expressionResults);                // Name == Goo
                expressionResults.Add(new WhereExpressionResult() { Condition = ExpressionType.Or });             // Or
                GetWhereExpressions<T>(binaryExpression.Right as BinaryExpression, expressionResults);               // Year == 2020
                expressionResults.Add(new WhereExpressionResult() { Parentheses = ")" });                         // )
            }
            else if (binaryExpression.NodeType == ExpressionType.AndAlso)
            {
                expressionResults.Add(new WhereExpressionResult() { Parentheses = "(" });                         // (
                GetWhereExpressions<T>(binaryExpression.Left as BinaryExpression, expressionResults);                // (Name == Foo Or || Name == Goo)
                expressionResults.Add(new WhereExpressionResult() { Condition = ExpressionType.And });            // And
                GetWhereExpressions<T>(binaryExpression.Right as BinaryExpression, expressionResults);               // Year == 2020
                expressionResults.Add(new WhereExpressionResult() { Parentheses = ")" });                         // )
            }

            return expressionResults;
        }
    }
}
