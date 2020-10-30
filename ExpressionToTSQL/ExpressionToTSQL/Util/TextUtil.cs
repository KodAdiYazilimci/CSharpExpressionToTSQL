using ExpressionToTSQL.Model;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace ExpressionToTSQL.Util
{
    public static class TextUtil
    {
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

        public static string ConvertToSql(this List<ExpressionResult> expressionResults)
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
