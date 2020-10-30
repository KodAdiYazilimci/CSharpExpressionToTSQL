using ExpressionToTSQL.Entity;
using ExpressionToTSQL.Model;
using ExpressionToTSQL.Util;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace ExpressionToTSQL.Other
{
    /// <summary>
    /// This is not unit test class
    /// </summary>
    static class Test
    {
        static void TestExpressions()
        {
            List<ExpressionResult> expressionResults = new List<ExpressionResult>();
            string rawText = string.Empty;

            Expression<Func<SampleEntity, bool>> expression = (x => x.Name == "Foo");
            expressionResults = ExpressionUtil.GetExpressions<SampleEntity>(expression.Body, expressionResults);
            rawText = expressionResults.ConvertToSql();

            expressionResults.Clear();
            expression = (x => !(x.Name == "Foo"));
            expressionResults = ExpressionUtil.GetExpressions<SampleEntity>(expression.Body, expressionResults);
            rawText = expressionResults.ConvertToSql();

            expressionResults.Clear();
            Expression<Func<SampleEntity, bool>> expressionWithOr = (x => x.Name == "Foo" || x.Name == "Goo");
            expressionResults = ExpressionUtil.GetExpressions<SampleEntity>(expressionWithOr.Body, expressionResults);
            rawText = expressionResults.ConvertToSql();

            expressionResults.Clear();
            Expression<Func<SampleEntity, bool>> expressionWithAnd = (x => x.Name == "Foo" && x.Name.Length == 3);
            expressionResults = ExpressionUtil.GetExpressions<SampleEntity>(expressionWithAnd.Body, expressionResults);
            rawText = expressionResults.ConvertToSql();

            expressionResults.Clear();
            Expression<Func<SampleEntity, bool>> expressionWithParentheses = (x => x.Name == "Foo" || (x.Name == "Goo" && x.Year == 2020));
            expressionResults = ExpressionUtil.GetExpressions<SampleEntity>(expressionWithParentheses.Body, expressionResults);
            rawText = expressionResults.ConvertToSql();

            expressionResults.Clear();
            expressionWithParentheses = (x => !(x.Name == "Foo" || (x.Name == "Goo" && x.Year == 2020)));
            expressionResults = ExpressionUtil.GetExpressions<SampleEntity>(expressionWithParentheses.Body, expressionResults);
            rawText = expressionResults.ConvertToSql();

            expressionResults.Clear();
            expressionWithParentheses = (x => (x.Name == "Foo" || x.Name == "Goo") && x.Year == 2020);
            expressionResults = ExpressionUtil.GetExpressions<SampleEntity>(expressionWithParentheses.Body, expressionResults);
            rawText = expressionResults.ConvertToSql();

            expressionResults.Clear();
            Expression<Func<SampleEntity, bool>> expressionWithNotEqual = (x => x.Name != "Foo");
            expressionResults = ExpressionUtil.GetExpressions<SampleEntity>(expressionWithNotEqual.Body, expressionResults);
            rawText = expressionResults.ConvertToSql();

            expressionResults.Clear();
            Expression<Func<SampleEntity, bool>> expressionWithParenthesesNotEqual = (x => (x.Name != "Foo" && x.Name != "Goo") && x.Year == 2020);
            expressionResults = ExpressionUtil.GetExpressions<SampleEntity>(expressionWithParenthesesNotEqual.Body, expressionResults);
            rawText = expressionResults.ConvertToSql(); // Result: ( ( Name != Foo and Name != Goo) and Year = 2020) 

            expressionResults.Clear();
            Expression<Func<SampleEntity, bool>> expressionLessThan = (x => x.Year < 2020);
            expressionResults = ExpressionUtil.GetExpressions<SampleEntity>(expressionLessThan.Body, expressionResults);
            rawText = expressionResults.ConvertToSql();

            expressionResults.Clear();
            expressionLessThan = (x => !(x.Year < 2020));
            expressionResults = ExpressionUtil.GetExpressions<SampleEntity>(expressionLessThan.Body, expressionResults);
            rawText = expressionResults.ConvertToSql();

            expressionResults.Clear();
            Expression<Func<SampleEntity, bool>> expressionGreaterThan = (x => x.Year > 2020);
            expressionResults = ExpressionUtil.GetExpressions<SampleEntity>(expressionGreaterThan.Body, expressionResults);
            rawText = expressionResults.ConvertToSql();

            expressionResults.Clear();
            Expression<Func<SampleEntity, bool>> expressionLessThanOrEqual = (x => x.Year <= 2020);
            expressionResults = ExpressionUtil.GetExpressions<SampleEntity>(expressionLessThanOrEqual.Body, expressionResults);
            rawText = expressionResults.ConvertToSql();

            expressionResults.Clear();
            Expression<Func<SampleEntity, bool>> expressionGreaterThanOrEqual = (x => x.Year >= 2020);
            expressionResults = ExpressionUtil.GetExpressions<SampleEntity>(expressionGreaterThanOrEqual.Body, expressionResults);
            rawText = expressionResults.ConvertToSql();

            expressionResults.Clear();
            Expression<Func<SampleEntity, bool>> expressionToLower = (x => x.Name.ToLower() == "foo");
            expressionResults = ExpressionUtil.GetExpressions<SampleEntity>(expressionToLower.Body, expressionResults);
            rawText = expressionResults.ConvertToSql();

            expressionResults.Clear();
            expressionToLower = (x => !(x.Name.ToLower() == "foo"));
            expressionResults = ExpressionUtil.GetExpressions<SampleEntity>(expressionToLower.Body, expressionResults);
            rawText = expressionResults.ConvertToSql();

            expressionResults.Clear();
            expressionToLower = (x => !(x.Name.ToLower() != "foo"));
            expressionResults = ExpressionUtil.GetExpressions<SampleEntity>(expressionToLower.Body, expressionResults);
            rawText = expressionResults.ConvertToSql();

            expressionResults.Clear();
            Expression<Func<SampleEntity, bool>> expressionToUpper = (x => x.Name.ToUpper() == "FOO");
            expressionResults = ExpressionUtil.GetExpressions<SampleEntity>(expressionToUpper.Body, expressionResults);
            rawText = expressionResults.ConvertToSql();

            expressionResults.Clear();
            Expression<Func<SampleEntity, bool>> expressionSubString = (x => x.Name.Substring(0, 3) == "Fooooo");
            expressionResults = ExpressionUtil.GetExpressions<SampleEntity>(expressionSubString.Body, expressionResults);
            rawText = expressionResults.ConvertToSql();

            expressionResults.Clear();
            Expression<Func<SampleEntity, bool>> expressionStartsWith = (x => x.Name.StartsWith('F'));
            expressionResults = ExpressionUtil.GetExpressions<SampleEntity>(expressionStartsWith.Body, expressionResults);
            rawText = expressionResults.ConvertToSql();

            expressionResults.Clear();
            expressionStartsWith = (x => !x.Name.StartsWith('F'));
            expressionResults = ExpressionUtil.GetExpressions<SampleEntity>(expressionStartsWith.Body, expressionResults);
            rawText = expressionResults.ConvertToSql();

            expressionResults.Clear();
            Expression<Func<SampleEntity, bool>> expressionEndsWith = (x => x.Name.EndsWith("o"));
            expressionResults = ExpressionUtil.GetExpressions<SampleEntity>(expressionEndsWith.Body, expressionResults);
            rawText = expressionResults.ConvertToSql();

            expressionResults.Clear();
            expressionEndsWith = (x => !x.Name.EndsWith("o"));
            expressionResults = ExpressionUtil.GetExpressions<SampleEntity>(expressionEndsWith.Body, expressionResults);
            rawText = expressionResults.ConvertToSql();

            expressionResults.Clear();
            List<string> stringListItems = new List<string>() { "foo", "goo", "too" };
            Expression<Func<SampleEntity, bool>> expressionStringListContains = (x => stringListItems.Contains(x.Name));
            expressionResults = ExpressionUtil.GetExpressions<SampleEntity>(expressionStringListContains, expressionResults);
            rawText = expressionResults.ConvertToSql();

            expressionResults.Clear();
            string[] stringArrayItems = new string[] { "foo", "goo", "too" };
            Expression<Func<SampleEntity, bool>> expressionStringArrayContains = (x => stringArrayItems.Contains(x.Name));
            expressionResults = ExpressionUtil.GetExpressions<SampleEntity>(expressionStringArrayContains, expressionResults);
            rawText = expressionResults.ConvertToSql();

            expressionResults.Clear();
            List<int> integerListItems = new List<int>() { 1990, 2000, 2010, 2020 };
            Expression<Func<SampleEntity, bool>> expressionIntegerListContains = (x => integerListItems.Contains(x.Year));
            expressionResults = ExpressionUtil.GetExpressions<SampleEntity>(expressionIntegerListContains, expressionResults);
            rawText = expressionResults.ConvertToSql();

            expressionResults.Clear();
            int[] integerArrayItems = new int[] { 1990, 2000, 2010, 2020 };
            Expression<Func<SampleEntity, bool>> expressionIntegerArrayContains = (x => integerArrayItems.Contains(x.Year));
            expressionResults = ExpressionUtil.GetExpressions<SampleEntity>(expressionIntegerArrayContains, expressionResults);
            rawText = expressionResults.ConvertToSql();

            expressionResults.Clear();
            integerArrayItems = new int[] { 1, 2, 3 };
            expressionIntegerArrayContains = (x => !integerArrayItems.Contains(x.Year));
            expressionResults = ExpressionUtil.GetExpressions<SampleEntity>(expressionIntegerArrayContains, expressionResults);
            rawText = expressionResults.ConvertToSql();
        }

        public static string ConvertToRawText(this List<ExpressionResult> expressionResults)
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
