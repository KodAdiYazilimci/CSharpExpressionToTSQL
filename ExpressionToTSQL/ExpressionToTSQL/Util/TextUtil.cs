using ExpressionToTSQL.Model;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace ExpressionToTSQL.Util
{
    /// <summary>
    /// The tools of text which will use for queries
    /// </summary>
    public static class TextUtil
    {
        /// <summary>
        /// Converts the where statement expressions to SQL text
        /// </summary>
        /// <param name="expressionResults">The result list of expressions which converted from lambda expressions</param>
        /// <returns></returns>
        public static string ConvertToSqlWhereStatement(this List<WhereExpressionResult> expressionResults)
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
                    if (exp.SubPropertyArgumentType == typeof(string))
                        sbText.Append("'");
                    sbText.Append(exp.Value);
                    if (exp.SubPropertyArgumentType == typeof(string))
                        sbText.Append("'");
                }
            }

            return sbText.ToString();
        }

        /// <summary>
        /// Converts the order by statement expressions to SQL text
        /// </summary>
        /// <param name="expressionResults"></param>
        /// <returns></returns>
        public static string ConvertToSqlOrderByStatement(this List<OrderByExpressionResult> expressionResults)
        {
            StringBuilder sbOrderBy = new StringBuilder();

            if (expressionResults.Any())
            {
                for (int i = 0; i < expressionResults.Count; i++)
                {
                    sbOrderBy.Append(expressionResults[i].MemberName);
                    sbOrderBy.Append(" ");
                    sbOrderBy.Append(expressionResults[i].IsAscending ? "ASC" : "DESC");

                    if (i < expressionResults.Count - 1)
                    {
                        sbOrderBy.Append(", ");
                    }
                }
            }

            return sbOrderBy.ToString();
        }

        /// <summary>
        /// Converts expression operators to Sql operators
        /// </summary>
        /// <param name="expressionType"></param>
        /// <returns></returns>
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
