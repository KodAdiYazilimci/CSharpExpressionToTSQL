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
            rawText = expressionResults.ConvertToSql();

            expressionResults.Clear();
            expression = (x => !(x.Name == "Foo"));
            expressionResults = GetExpressions(expression.Body, expressionResults);
            rawText = expressionResults.ConvertToSql();

            expressionResults.Clear();
            Expression<Func<SampleClass, bool>> expressionWithOr = (x => x.Name == "Foo" || x.Name == "Goo");
            expressionResults = GetExpressions(expressionWithOr.Body, expressionResults);
            rawText = expressionResults.ConvertToSql();

            expressionResults.Clear();
            Expression<Func<SampleClass, bool>> expressionWithAnd = (x => x.Name == "Foo" && x.Name.Length == 3);
            expressionResults = GetExpressions(expressionWithAnd.Body, expressionResults);
            rawText = expressionResults.ConvertToSql();

            expressionResults.Clear();
            Expression<Func<SampleClass, bool>> expressionWithParentheses = (x => x.Name == "Foo" || (x.Name == "Goo" && x.Year == 2020));
            expressionResults = GetExpressions(expressionWithParentheses.Body, expressionResults);
            rawText = expressionResults.ConvertToSql();

            expressionResults.Clear();
            expressionWithParentheses = (x => !(x.Name == "Foo" || (x.Name == "Goo" && x.Year == 2020)));
            expressionResults = GetExpressions(expressionWithParentheses.Body, expressionResults);
            rawText = expressionResults.ConvertToSql();

            expressionResults.Clear();
            expressionWithParentheses = (x => (x.Name == "Foo" || x.Name == "Goo") && x.Year == 2020);
            expressionResults = GetExpressions(expressionWithParentheses.Body, expressionResults);
            rawText = expressionResults.ConvertToSql();

            expressionResults.Clear();
            Expression<Func<SampleClass, bool>> expressionWithNotEqual = (x => x.Name != "Foo");
            expressionResults = GetExpressions(expressionWithNotEqual.Body, expressionResults);
            rawText = expressionResults.ConvertToSql();

            expressionResults.Clear();
            Expression<Func<SampleClass, bool>> expressionWithParenthesesNotEqual = (x => (x.Name != "Foo" && x.Name != "Goo") && x.Year == 2020);
            expressionResults = GetExpressions(expressionWithParenthesesNotEqual.Body, expressionResults);
            rawText = expressionResults.ConvertToSql(); // Result: ( ( Name != Foo and Name != Goo) and Year = 2020) 

            expressionResults.Clear();
            Expression<Func<SampleClass, bool>> expressionLessThan = (x => x.Year < 2020);
            expressionResults = GetExpressions(expressionLessThan.Body, expressionResults);
            rawText = expressionResults.ConvertToSql();

            expressionResults.Clear();
            expressionLessThan = (x => !(x.Year < 2020));
            expressionResults = GetExpressions(expressionLessThan.Body, expressionResults);
            rawText = expressionResults.ConvertToSql();

            expressionResults.Clear();
            Expression<Func<SampleClass, bool>> expressionGreaterThan = (x => x.Year > 2020);
            expressionResults = GetExpressions(expressionGreaterThan.Body, expressionResults);
            rawText = expressionResults.ConvertToSql();

            expressionResults.Clear();
            Expression<Func<SampleClass, bool>> expressionLessThanOrEqual = (x => x.Year <= 2020);
            expressionResults = GetExpressions(expressionLessThanOrEqual.Body, expressionResults);
            rawText = expressionResults.ConvertToSql();

            expressionResults.Clear();
            Expression<Func<SampleClass, bool>> expressionGreaterThanOrEqual = (x => x.Year >= 2020);
            expressionResults = GetExpressions(expressionGreaterThanOrEqual.Body, expressionResults);
            rawText = expressionResults.ConvertToSql();

            expressionResults.Clear();
            Expression<Func<SampleClass, bool>> expressionToLower = (x => x.Name.ToLower() == "foo");
            expressionResults = GetExpressions(expressionToLower.Body, expressionResults);
            rawText = expressionResults.ConvertToSql();

            expressionResults.Clear();
            expressionToLower = (x => !(x.Name.ToLower() == "foo"));
            expressionResults = GetExpressions(expressionToLower.Body, expressionResults);
            rawText = expressionResults.ConvertToSql();

            expressionResults.Clear();
            expressionToLower = (x => !(x.Name.ToLower() != "foo"));
            expressionResults = GetExpressions(expressionToLower.Body, expressionResults);
            rawText = expressionResults.ConvertToSql();

            expressionResults.Clear();
            Expression<Func<SampleClass, bool>> expressionToUpper = (x => x.Name.ToUpper() == "FOO");
            expressionResults = GetExpressions(expressionToUpper.Body, expressionResults);
            rawText = expressionResults.ConvertToSql();

            expressionResults.Clear();
            Expression<Func<SampleClass, bool>> expressionSubString = (x => x.Name.Substring(0, 3) == "Fooooo");
            expressionResults = GetExpressions(expressionSubString.Body, expressionResults);
            rawText = expressionResults.ConvertToSql();

            expressionResults.Clear();
            Expression<Func<SampleClass, bool>> expressionStartsWith = (x => x.Name.StartsWith('F'));
            expressionResults = GetExpressions(expressionStartsWith.Body, expressionResults);
            rawText = expressionResults.ConvertToSql();

            expressionResults.Clear();
            expressionStartsWith = (x => !x.Name.StartsWith('F'));
            expressionResults = GetExpressions(expressionStartsWith.Body, expressionResults);
            rawText = expressionResults.ConvertToSql();

            expressionResults.Clear();
            Expression<Func<SampleClass, bool>> expressionEndsWith = (x => x.Name.EndsWith("o"));
            expressionResults = GetExpressions(expressionEndsWith.Body, expressionResults);
            rawText = expressionResults.ConvertToSql();

            expressionResults.Clear();
            expressionEndsWith = (x => !x.Name.EndsWith("o"));
            expressionResults = GetExpressions(expressionEndsWith.Body, expressionResults);
            rawText = expressionResults.ConvertToSql();

            expressionResults.Clear();
            List<string> stringListItems = new List<string>() { "foo", "goo", "too" };
            Expression<Func<SampleClass, bool>> expressionStringListContains = (x => stringListItems.Contains(x.Name));
            expressionResults = GetExpressions(expressionStringListContains, expressionResults);
            rawText = expressionResults.ConvertToSql();

            expressionResults.Clear();
            string[] stringArrayItems = new string[] { "foo", "goo", "too" };
            Expression<Func<SampleClass, bool>> expressionStringArrayContains = (x => stringArrayItems.Contains(x.Name));
            expressionResults = GetExpressions(expressionStringArrayContains, expressionResults);
            rawText = expressionResults.ConvertToSql();

            expressionResults.Clear();
            List<int> integerListItems = new List<int>() { 1990, 2000, 2010, 2020 };
            Expression<Func<SampleClass, bool>> expressionIntegerListContains = (x => integerListItems.Contains(x.Year));
            expressionResults = GetExpressions(expressionIntegerListContains, expressionResults);
            rawText = expressionResults.ConvertToSql();

            expressionResults.Clear();
            int[] integerArrayItems = new int[] { 1990, 2000, 2010, 2020 };
            Expression<Func<SampleClass, bool>> expressionIntegerArrayContains = (x => integerArrayItems.Contains(x.Year));
            expressionResults = GetExpressions(expressionIntegerArrayContains, expressionResults);
            rawText = expressionResults.ConvertToSql();

            expressionResults.Clear();
            integerArrayItems = new int[] { 1, 2, 3 };
            expressionIntegerArrayContains = (x => !integerArrayItems.Contains(x.Year));
            expressionResults = GetExpressions(expressionIntegerArrayContains, expressionResults);
            rawText = expressionResults.ConvertToSql();
        }

        private static List<ExpressionResult> GetExpressions(object expression, List<ExpressionResult> toExpressionList) //this method may be call recursive for sub-expressions
        {
            if (expression is BinaryExpression) // x.Name == "Foo" // for equality statement of the property
            {
                BinaryExpression binaryExpression = expression as BinaryExpression;
                toExpressionList.AddRange(Extract(binaryExpression));
            }
            else if (expression is MethodCallExpression) //x.Name.StartsWith(F) // the property calls any method
            {
                MethodCallExpression methodCallExpression = expression as MethodCallExpression;
                toExpressionList.AddRange(Extract(methodCallExpression));
            }
            else if (expression is LambdaExpression) //stringListItems.Contains(x.Name) // the property will be searched in a array
            {
                LambdaExpression lambdaExpression = expression as LambdaExpression;

                if (lambdaExpression.Body is MethodCallExpression)
                {
                    MethodCallExpression methodCallExpression = lambdaExpression.Body as MethodCallExpression;
                    toExpressionList.AddRange(Extract(methodCallExpression));
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
                            toExpressionList.AddRange(Extract(methodCallExpression));
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
                        toExpressionList.AddRange(Extract(methodCallExpression));
                        toExpressionList.Add(new ExpressionResult() { Parentheses = ")" });
                    }
                    else if (unaryExpression.Operand is BinaryExpression) // Not((x.Name == "Foo") // for "not equal" statement of property
                    {
                        BinaryExpression binaryExpression = unaryExpression.Operand as BinaryExpression;
                        toExpressionList.Add(new ExpressionResult() { Parentheses = "!" });
                        toExpressionList.Add(new ExpressionResult() { Parentheses = "(" });
                        toExpressionList.AddRange(Extract(binaryExpression));
                        toExpressionList.Add(new ExpressionResult() { Parentheses = ")" });
                    }
                }
            }

            return toExpressionList;
        }

        private static List<ExpressionResult> Extract(MethodCallExpression methodCallExpression)
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

        private static List<ExpressionResult> Extract(BinaryExpression binaryExpression)
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

                    expressionResults.Add(expressionResult);
                }
            }
            else if (binaryExpression.NodeType == ExpressionType.OrElse)
            {
                expressionResults.Add(new ExpressionResult() { Parentheses = "(" });                         // (
                GetExpressions(binaryExpression.Left as BinaryExpression, expressionResults);                // Name == Goo
                expressionResults.Add(new ExpressionResult() { Condition = ExpressionType.Or });             // Or
                GetExpressions(binaryExpression.Right as BinaryExpression, expressionResults);               // Year == 2020
                expressionResults.Add(new ExpressionResult() { Parentheses = ")" });                         // )
            }
            else if (binaryExpression.NodeType == ExpressionType.AndAlso)
            {
                expressionResults.Add(new ExpressionResult() { Parentheses = "(" });                         // (
                GetExpressions(binaryExpression.Left as BinaryExpression, expressionResults);                // (Name == Foo Or || Name == Goo)
                expressionResults.Add(new ExpressionResult() { Condition = ExpressionType.And });            // And
                GetExpressions(binaryExpression.Right as BinaryExpression, expressionResults);               // Year == 2020
                expressionResults.Add(new ExpressionResult() { Parentheses = ")" });                         // )
            }

            return expressionResults;
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

        private static string ConvertToSql(this List<ExpressionResult> expressionResults)
        {
            StringBuilder sbText = new StringBuilder();

            foreach (var exp in expressionResults)
            {
                if (!string.IsNullOrEmpty(exp.Parentheses))
                {
                    sbText.Append(exp.Parentheses);
                }

                if (!string.IsNullOrEmpty(exp.SubProperty))
                {
                    StringBuilder function = new StringBuilder();

                    if (exp.SubProperty == nameof(String.Length))
                    {
                        function.Append("LEN(");
                        function.Append(exp.MemberName);
                        function.Append(")");
                        function.Append(" ");
                        function.Append(GetConditionChar(exp.Condition));
                        function.Append(" ");
                        function.Append(exp.Value);
                    }
                    else if (exp.SubProperty == nameof(String.ToLower))
                    {
                        function.Append("LOWER(");
                        function.Append(exp.MemberName);
                        function.Append(")");
                        function.Append(" ");
                        function.Append(GetConditionChar(exp.Condition));
                        function.Append(" ");
                        if (exp.SubPropertyArgumentType == typeof(string))
                            function.Append("'");
                        function.Append(exp.Value);
                        if (exp.SubPropertyArgumentType == typeof(string))
                            function.Append("'");
                    }
                    else if (exp.SubProperty == nameof(String.ToUpper))
                    {
                        function.Append("UPPER(");
                        function.Append(exp.MemberName);
                        function.Append(")");
                        function.Append(" ");
                        function.Append(GetConditionChar(exp.Condition));
                        function.Append(" ");
                        if (exp.SubPropertyArgumentType == typeof(string))
                            function.Append("'");
                        function.Append(exp.Value);
                        if (exp.SubPropertyArgumentType == typeof(string))
                            function.Append("'");
                    }
                    else if (exp.SubProperty == nameof(String.Substring))
                    {
                        function.Append("SUBSTRING(");
                        function.Append(exp.MemberName);
                        function.Append(",");
                        function.Append(string.Join(',', exp.SubPropertyArguments));
                        function.Append(")");
                        function.Append(" ");
                        function.Append(GetConditionChar(exp.Condition));
                        function.Append(" ");
                        if (exp.SubPropertyArgumentType == typeof(string))
                            function.Append("'");
                        function.Append(exp.Value);
                        if (exp.SubPropertyArgumentType == typeof(string))
                            function.Append("'");
                    }
                    else if (exp.SubProperty == nameof(String.StartsWith))
                    {
                        function.Append(exp.MemberName);
                        function.Append(" LIKE '");
                        function.Append(exp.Value);
                        function.Append("'%");
                    }
                    else if (exp.SubProperty == nameof(String.EndsWith))
                    {
                        function.Append(exp.MemberName);
                        function.Append(" LIKE '%");
                        function.Append(exp.Value);
                        function.Append("'");
                    }
                    else if (exp.SubProperty == nameof(String.Contains))
                    {
                        function.Append(exp.MemberName);
                        function.Append(" IN (");
                        if (exp.SubPropertyArgumentType == typeof(string))
                            function.Append("'");
                        function.Append(string.Join(exp.SubPropertyArgumentType == typeof(string) ? "','" : "", exp.SubPropertyArguments));
                        if (exp.SubPropertyArgumentType == typeof(string))
                            function.Append("'");
                        function.Append(")");
                    }

                    sbText.Append(function.ToString());
                }
                else
                {
                    sbText.Append(exp.MemberName);
                    sbText.Append(" ");
                    sbText.Append(GetConditionChar(exp.Condition));
                    sbText.Append(" ");
                    sbText.Append(exp.Value);
                }
            }

            return sbText.ToString();
        }

        private static string GetConditionChar(ExpressionType expressionType)
        {
            switch (expressionType)
            {
                case ExpressionType.Equal: return "=";
                case ExpressionType.NotEqual: return "!=";
                case ExpressionType.And: return "AND";
                case ExpressionType.Or: return "OR";
                case ExpressionType.LessThan: return "<";
                case ExpressionType.GreaterThan: return ">";
                case ExpressionType.LessThanOrEqual: return "<=";
                case ExpressionType.GreaterThanOrEqual: return ">=";
                default:
                    return string.Empty;
            }
        }
    }
}
