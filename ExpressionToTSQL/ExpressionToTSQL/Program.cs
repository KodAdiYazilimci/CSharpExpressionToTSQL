using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Text;

namespace ExpressionToTSQL
{
    static class Program
    {
        static void Main(string[] args)
        {
            List<ExpressionResult> expressionResults = new List<ExpressionResult>();
            string rawText = string.Empty;

            Expression<Func<SampleClass, bool>> expression = (x => x.Name == "Foo");
            expressionResults = GetExpressions(expression.Body, expressionResults);
            rawText = expressionResults.ConvertToRawText();

            expressionResults.Clear();
            expression = (x => !(x.Name == "Foo"));
            expressionResults = GetExpressions(expression.Body, expressionResults);
            rawText = expressionResults.ConvertToRawText();

            expressionResults.Clear();
            Expression<Func<SampleClass, bool>> expressionWithOr = (x => x.Name == "Foo" || x.Name == "Goo");
            expressionResults = GetExpressions(expressionWithOr.Body, expressionResults);
            rawText = expressionResults.ConvertToRawText();

            expressionResults.Clear();
            Expression<Func<SampleClass, bool>> expressionWithAnd = (x => x.Name == "Foo" && x.Name.Length == 3);
            expressionResults = GetExpressions(expressionWithAnd.Body, expressionResults);
            rawText = expressionResults.ConvertToRawText();

            expressionResults.Clear();
            Expression<Func<SampleClass, bool>> expressionWithParentheses = (x => x.Name == "Foo" || (x.Name == "Goo" && x.Year == 2020));
            expressionResults = GetExpressions(expressionWithParentheses.Body, expressionResults);
            rawText = expressionResults.ConvertToRawText();

            expressionResults.Clear();
            expressionWithParentheses = (x => !(x.Name == "Foo" || (x.Name == "Goo" && x.Year == 2020)));
            expressionResults = GetExpressions(expressionWithParentheses.Body, expressionResults);
            rawText = expressionResults.ConvertToRawText();

            expressionResults.Clear();
            expressionWithParentheses = (x => (x.Name == "Foo" || x.Name == "Goo") && x.Year == 2020);
            expressionResults = GetExpressions(expressionWithParentheses.Body, expressionResults);
            rawText = expressionResults.ConvertToRawText();

            expressionResults.Clear();
            Expression<Func<SampleClass, bool>> expressionWithNotEqual = (x => x.Name != "Foo");
            expressionResults = GetExpressions(expressionWithNotEqual.Body, expressionResults);
            rawText = expressionResults.ConvertToRawText();

            expressionResults.Clear();
            Expression<Func<SampleClass, bool>> expressionWithParenthesesNotEqual = (x => (x.Name != "Foo" && x.Name != "Goo") && x.Year == 2020);
            expressionResults = GetExpressions(expressionWithParenthesesNotEqual.Body, expressionResults);
            rawText = expressionResults.ConvertToRawText(); // Result: ( ( Name != Foo and Name != Goo) and Year = 2020) 

            expressionResults.Clear();
            Expression<Func<SampleClass, bool>> expressionLessThan = (x => x.Year < 2020);
            expressionResults = GetExpressions(expressionLessThan.Body, expressionResults);
            rawText = expressionResults.ConvertToRawText();

            expressionResults.Clear();
            expressionLessThan = (x => !(x.Year < 2020));
            expressionResults = GetExpressions(expressionLessThan.Body, expressionResults);
            rawText = expressionResults.ConvertToRawText();

            expressionResults.Clear();
            Expression<Func<SampleClass, bool>> expressionGreaterThan = (x => x.Year > 2020);
            expressionResults = GetExpressions(expressionGreaterThan.Body, expressionResults);
            rawText = expressionResults.ConvertToRawText();

            expressionResults.Clear();
            Expression<Func<SampleClass, bool>> expressionLessThanOrEqual = (x => x.Year <= 2020);
            expressionResults = GetExpressions(expressionLessThanOrEqual.Body, expressionResults);
            rawText = expressionResults.ConvertToRawText();

            expressionResults.Clear();
            Expression<Func<SampleClass, bool>> expressionGreaterThanOrEqual = (x => x.Year >= 2020);
            expressionResults = GetExpressions(expressionGreaterThanOrEqual.Body, expressionResults);
            rawText = expressionResults.ConvertToRawText();

            expressionResults.Clear();
            Expression<Func<SampleClass, bool>> expressionToLower = (x => x.Name.ToLower() == "foo");
            expressionResults = GetExpressions(expressionToLower.Body, expressionResults);
            rawText = expressionResults.ConvertToRawText();

            expressionResults.Clear();
            expressionToLower = (x => !(x.Name.ToLower() == "foo"));
            expressionResults = GetExpressions(expressionToLower.Body, expressionResults);
            rawText = expressionResults.ConvertToRawText();

            expressionResults.Clear();
            expressionToLower = (x => !(x.Name.ToLower() != "foo"));
            expressionResults = GetExpressions(expressionToLower.Body, expressionResults);
            rawText = expressionResults.ConvertToRawText();

            expressionResults.Clear();
            Expression<Func<SampleClass, bool>> expressionToUpper = (x => x.Name.ToUpper() == "FOO");
            expressionResults = GetExpressions(expressionToUpper.Body, expressionResults);
            rawText = expressionResults.ConvertToRawText();

            expressionResults.Clear();
            Expression<Func<SampleClass, bool>> expressionSubString = (x => x.Name.Substring(0, 3) == "Fooooo");
            expressionResults = GetExpressions(expressionSubString.Body, expressionResults);
            rawText = expressionResults.ConvertToRawText();

            expressionResults.Clear();
            Expression<Func<SampleClass, bool>> expressionStartsWith = (x => x.Name.StartsWith('F'));
            expressionResults = GetExpressions(expressionStartsWith.Body, expressionResults);
            rawText = expressionResults.ConvertToRawText();

            expressionResults.Clear();
            expressionStartsWith = (x => !x.Name.StartsWith('F'));
            expressionResults = GetExpressions(expressionStartsWith.Body, expressionResults);
            rawText = expressionResults.ConvertToRawText();

            expressionResults.Clear();
            Expression<Func<SampleClass, bool>> expressionEndsWith = (x => x.Name.EndsWith("o"));
            expressionResults = GetExpressions(expressionEndsWith.Body, expressionResults);
            rawText = expressionResults.ConvertToRawText();

            expressionResults.Clear();
            expressionEndsWith = (x => !x.Name.EndsWith("o"));
            expressionResults = GetExpressions(expressionEndsWith.Body, expressionResults);
            rawText = expressionResults.ConvertToRawText();

            expressionResults.Clear();
            List<string> stringListItems = new List<string>() { "foo", "goo", "too" };
            Expression<Func<SampleClass, bool>> expressionStringListContains = (x => stringListItems.Contains(x.Name));
            expressionResults = GetExpressions(expressionStringListContains, expressionResults);
            rawText = expressionResults.ConvertToRawText();

            expressionResults.Clear();
            string[] stringArrayItems = new string[] { "foo", "goo", "too" };
            Expression<Func<SampleClass, bool>> expressionStringArrayContains = (x => stringArrayItems.Contains(x.Name));
            expressionResults = GetExpressions(expressionStringArrayContains, expressionResults);
            rawText = expressionResults.ConvertToRawText();

            expressionResults.Clear();
            List<int> integerListItems = new List<int>() { 1990, 2000, 2010, 2020 };
            Expression<Func<SampleClass, bool>> expressionIntegerListContains = (x => integerListItems.Contains(x.Year));
            expressionResults = GetExpressions(expressionIntegerListContains, expressionResults);
            rawText = expressionResults.ConvertToRawText();

            expressionResults.Clear();
            int[] integerArrayItems = new int[] { 1990, 2000, 2010, 2020 };
            Expression<Func<SampleClass, bool>> expressionIntegerArrayContains = (x => integerArrayItems.Contains(x.Year));
            expressionResults = GetExpressions(expressionIntegerArrayContains, expressionResults);
            rawText = expressionResults.ConvertToRawText();

            expressionResults.Clear();
            integerArrayItems = new int[] { 1, 2, 3 };
            expressionIntegerArrayContains = (x => !integerArrayItems.Contains(x.Year));
            expressionResults = GetExpressions(expressionIntegerArrayContains, expressionResults);
            rawText = expressionResults.ConvertToRawText();
        }

        private static List<ExpressionResult> GetExpressions(object expression, List<ExpressionResult> toExpressionList)
        {
            if (expression is BinaryExpression)
            {
                BinaryExpression binaryExpression = expression as BinaryExpression;
                Extract(binaryExpression, toExpressionList);
            }
            else if (expression is MethodCallExpression)
            {
                MethodCallExpression methodCallExpression = expression as MethodCallExpression;
                ExpressionResult expressionResult = Extract(methodCallExpression);
                toExpressionList.Add(expressionResult);
            }
            else if (expression is LambdaExpression)
            {
                LambdaExpression lambdaExpression = expression as LambdaExpression;

                if (lambdaExpression.Body is MethodCallExpression)
                {
                    MethodCallExpression methodCallExpression = lambdaExpression.Body as MethodCallExpression;
                    ExpressionResult expressionResult = Extract(methodCallExpression);
                    toExpressionList.Add(expressionResult);
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
                            ExpressionResult expressionResult = Extract(methodCallExpression);
                            toExpressionList.Add(expressionResult);
                            toExpressionList.Add(new ExpressionResult() { Parentheses = ")" });
                        }
                    }
                }
            }
            else if (expression is UnaryExpression)
            {
                UnaryExpression unaryExpression = expression as UnaryExpression;

                if (unaryExpression.NodeType == ExpressionType.Not)
                {
                    if (unaryExpression.Operand is MethodCallExpression)
                    {
                        MethodCallExpression methodCallExpression = unaryExpression.Operand as MethodCallExpression;
                        toExpressionList.Add(new ExpressionResult() { Parentheses = "!" });
                        toExpressionList.Add(new ExpressionResult() { Parentheses = "(" });
                        ExpressionResult expressionResult = Extract(methodCallExpression);
                        toExpressionList.Add(expressionResult);
                        toExpressionList.Add(new ExpressionResult() { Parentheses = ")" });
                    }
                    else if (unaryExpression.Operand is BinaryExpression)
                    {
                        BinaryExpression binaryExpression = unaryExpression.Operand as BinaryExpression;
                        toExpressionList.Add(new ExpressionResult() { Parentheses = "!" });
                        toExpressionList.Add(new ExpressionResult() { Parentheses = "(" });
                        Extract(binaryExpression, toExpressionList);
                        toExpressionList.Add(new ExpressionResult() { Parentheses = ")" });
                    }
                }
            }

            return toExpressionList;
        }

        private static ExpressionResult Extract(MethodCallExpression methodCallExpression)
        {
            ExpressionResult expressionResult = new ExpressionResult();

            expressionResult.SubProperty = methodCallExpression.Method.Name;

            if (methodCallExpression.NodeType == ExpressionType.Call && methodCallExpression.Object is MemberExpression)
            {
                if (methodCallExpression.Object is MemberExpression)
                {
                    expressionResult.MemberName = (methodCallExpression.Object as MemberExpression).Member.Name;
                }
                if (methodCallExpression.Arguments.FirstOrDefault() is MemberExpression)
                {
                    expressionResult.MemberName = (methodCallExpression.Arguments.FirstOrDefault() as MemberExpression).Member.Name;
                }

                MemberExpression memberExpression = methodCallExpression.Object as MemberExpression;

                Expression subExpression = memberExpression.Expression;
                string expressionParameterKeyword = memberExpression.Member.Name;

                if (subExpression is ConstantExpression)
                {
                    object subExpressionValue = (subExpression as ConstantExpression).Value;

                    if (methodCallExpression.Object.Type == typeof(List<string>))
                    {
                        List<string> values = subExpressionValue.GetType().GetField(expressionParameterKeyword).GetValue(subExpressionValue) as List<string>;
                        expressionResult.SubPropertyArgumentType = typeof(string);
                        expressionResult.SubPropertyArguments.AddRange(values);
                    }
                    else if (methodCallExpression.Object.Type == typeof(List<int>))
                    {
                        List<int> values = subExpressionValue.GetType().GetField(expressionParameterKeyword).GetValue(subExpressionValue) as List<int>;
                        expressionResult.SubPropertyArgumentType = typeof(int);
                        expressionResult.SubPropertyArguments.AddRange(values.Select(x => x.ToString()).ToList());
                    }
                }
                else
                {
                    if (methodCallExpression.Arguments.FirstOrDefault() is ConstantExpression)
                    {
                        ConstantExpression constantExpression = methodCallExpression.Arguments.FirstOrDefault() as ConstantExpression;

                        expressionResult.SubPropertyArgumentType = constantExpression.Value.GetType();
                        expressionResult.Value = constantExpression.Value.ToString();

                    }
                }
            }
            else if (methodCallExpression.NodeType == ExpressionType.Call && methodCallExpression.Method.DeclaringType == typeof(Enumerable))
            {
                Expression parameterExpression = methodCallExpression.Arguments.FirstOrDefault(x => x is MemberExpression && (x as MemberExpression).Expression.NodeType == ExpressionType.Parameter);

                expressionResult.MemberName = (parameterExpression as MemberExpression).Member.Name;

                Expression constantExpression = methodCallExpression.Arguments.FirstOrDefault(x => x is MemberExpression && (x as MemberExpression).Expression.NodeType == ExpressionType.Constant);

                MemberExpression memberExpression = constantExpression as MemberExpression;

                string expressionParameterKeyword = memberExpression.Member.Name;

                if (memberExpression.Type == typeof(string[]))
                {
                    object values = (memberExpression.Expression as ConstantExpression).Value;
                    object items = values.GetType().GetField(expressionParameterKeyword).GetValue(values);

                    expressionResult.SubPropertyArguments.AddRange((items as string[]));
                    expressionResult.SubPropertyArgumentType = typeof(string);
                }
                else if (memberExpression.Type == typeof(int[]))
                {
                    object values = (memberExpression.Expression as ConstantExpression).Value;
                    object items = values.GetType().GetField(expressionParameterKeyword).GetValue(values);

                    expressionResult.SubPropertyArguments.AddRange((items as int[]).Select(x => x.ToString()).ToList());
                    expressionResult.SubPropertyArgumentType = typeof(int);
                }
            }

            return expressionResult;
        }

        private static void Extract(BinaryExpression binaryExpression, List<ExpressionResult> toExpressionList)
        {
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

                    if (memberExpression.Expression != null && memberExpression.Expression.Type == typeof(SampleClass))
                    {
                        expressionResult.MemberName = memberExpression.Member.Name;                                               // Name
                    }
                    else
                    {
                        expressionResult.MemberName = (memberExpression.Expression as MemberExpression).Member.Name;              // Name
                        expressionResult.SubProperty = memberExpression.Member.Name;                                              // Name.Length (Name.Length == 3)
                    }
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
        }

        private static string ConvertToRawText(this List<ExpressionResult> expressionResults)
        {
            StringBuilder sbText = new StringBuilder();

            foreach (var exp in expressionResults)
            {
                if (!string.IsNullOrEmpty(exp.Parentheses))
                {
                    sbText.Append(exp.Parentheses);
                }

                if (!sbText.ToString().EndsWith(" "))
                    sbText.Append(" ");

                if (!string.IsNullOrEmpty(exp.MemberName))
                {
                    sbText.Append(exp.MemberName);
                }

                if (!string.IsNullOrEmpty(exp.SubProperty))
                {
                    sbText.Append(".");
                    sbText.Append(exp.SubProperty);

                    if (exp.SubPropertyArguments.Any())
                    {
                        sbText.Append("(");
                        sbText.Append(String.Join(',', exp.SubPropertyArguments));
                        sbText.Append(")");
                    }
                }

                if (!sbText.ToString().EndsWith(" "))
                    sbText.Append(" ");

                switch (exp.Condition)
                {
                    case ExpressionType.Equal:
                        sbText.Append("=");
                        break;
                    case ExpressionType.NotEqual:
                        sbText.Append("!=");
                        break;
                    case ExpressionType.And:
                        sbText.Append("and");
                        break;
                    case ExpressionType.Or:
                        sbText.Append("or");
                        break;
                    case ExpressionType.LessThan:
                        sbText.Append("<");
                        break;
                    case ExpressionType.GreaterThan:
                        sbText.Append(">");
                        break;
                    case ExpressionType.LessThanOrEqual:
                        sbText.Append("<=");
                        break;
                    case ExpressionType.GreaterThanOrEqual:
                        sbText.Append(">=");
                        break;
                    default:
                        break;
                }

                if (!sbText.ToString().EndsWith(" "))
                    sbText.Append(" ");

                sbText.Append(exp.Value);
            }

            return sbText.ToString();
        }
    }
}
