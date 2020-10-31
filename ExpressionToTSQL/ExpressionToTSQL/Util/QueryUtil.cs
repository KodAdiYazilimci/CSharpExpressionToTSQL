using ExpressionToTSQL.Abstractions;
using ExpressionToTSQL.Model;
using ExpressionToTSQL.Persistence;
using ExpressionToTSQL.Provider;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace ExpressionToTSQL.Util
{
    /// <summary>
    /// Assigns extensions which will complete the Sql query
    /// </summary>
    public static class QueryExtensions
    {
        /// <summary>
        /// Adds a statement to query (can be applied multiple times)
        /// </summary>
        /// <typeparam name="T">The type of entity class</typeparam>
        /// <param name="query">The query will be attached to ownself</param>
        /// <param name="expression">Expression statement</param>
        /// <returns></returns>
        public static IQuery<T> Where<T>(this IQuery<T> query, Expression<Func<T, bool>> expression)
        {
            List<WhereExpressionResult> expressionResults = new List<WhereExpressionResult>();
            expressionResults = ExpressionUtil.GetWhereExpressions<T>(expression.Body, expressionResults);

            if (expressionResults.Any())
            {
                if (query.WhereStatement.Length > 0)
                {
                    query.WhereStatement.Append(" AND ");
                }
                if (expressionResults.Any(x => !string.IsNullOrEmpty(x.SubProperty)))
                {

                    string memberName = typeof(T).Name.ToLower() + "." + expressionResults.FirstOrDefault(x => !string.IsNullOrEmpty(x.MemberName)).MemberName;
                    expressionResults.FirstOrDefault(x => !string.IsNullOrEmpty(x.MemberName)).MemberName = memberName;

                    query.WhereStatement.Append(TextUtil.ConvertToSqlWhereStatement(expressionResults));
                }
                else
                {
                    query.WhereStatement.Append(typeof(T).Name.ToLower());
                    query.WhereStatement.Append(".");
                    query.WhereStatement.Append(TextUtil.ConvertToSqlWhereStatement(expressionResults));
                }
            }
            return query;
        }

        /// <summary>
        /// Fetches 1 row data from database
        /// </summary>
        /// <typeparam name="T">The type of entity class</typeparam>
        /// <param name="query">The query which applied to SQL</param>
        /// <returns></returns>
        public static T FetchFirst<T>(this IQuery<T> query)
        {
            StringBuilder sbQuery = new StringBuilder();

            sbQuery.Append($"SELECT TOP 1 ");
            if (query.Selects.PropertyAssignments.Any())
            {
                for (int i = 0; i < query.Selects.PropertyAssignments.Count; i++)
                {
                    sbQuery.Append(query.Selects.PropertyAssignments[i].FromType.Name.ToLower());
                    sbQuery.Append(".");
                    sbQuery.Append(query.Selects.PropertyAssignments[i].FromProperty);
                    sbQuery.Append(" AS ");
                    sbQuery.Append(query.Selects.PropertyAssignments[i].PropertyName);

                    if (i < query.Selects.PropertyAssignments.Count - 1)
                    {
                        sbQuery.Append(", ");
                    }
                    else
                        sbQuery.Append(" ");
                }
            }
            else
            {
                sbQuery.Append(" * ");
            }
            sbQuery.Append(query.FromStatement);
            sbQuery.Append(query.JoinStatement);
            sbQuery.Append(" WHERE ");
            sbQuery.Append(query.WhereStatement);
            sbQuery.Append(" ORDER BY ");
            sbQuery.Append(query.OrderByAscendingStatement);
            sbQuery.Append(" ");
            sbQuery.Append(query.OrderByDescendingStatement);

            MsSQLDataProvider<T> dataProvider = new MsSQLDataProvider<T>();

            return dataProvider.Get(sbQuery.ToString(), query.ConnectionString);
        }

        /// <summary>
        /// Fetches all row data which matches to statements from database
        /// </summary>
        /// <typeparam name="T">The type of entity class</typeparam>
        /// <param name="query">The query which applied to SQL</param>
        /// <returns></returns>
        public static List<T> FetchList<T>(this IQuery<T> query)
        {
            StringBuilder sbQuery = new StringBuilder();

            sbQuery.Append($"SELECT { (query.TakeCount.HasValue ? "TOP " + query.TakeCount.Value.ToString() : "") }");

            if (query.Selects.PropertyAssignments.Any())
            {
                for (int i = 0; i < query.Selects.PropertyAssignments.Count; i++)
                {
                    sbQuery.Append(query.Selects.PropertyAssignments[i].FromType.Name.ToLower());
                    sbQuery.Append(".");
                    sbQuery.Append(query.Selects.PropertyAssignments[i].FromProperty);
                    sbQuery.Append(" AS ");
                    sbQuery.Append(query.Selects.PropertyAssignments[i].PropertyName);

                    if (i < query.Selects.PropertyAssignments.Count - 1)
                    {
                        sbQuery.Append(", ");
                    }
                    else
                        sbQuery.Append(" ");
                }
            }
            else
            {
                sbQuery.Append(" * ");
            }

            sbQuery.Append(query.FromStatement);
            sbQuery.Append(query.JoinStatement);
            sbQuery.Append(" WHERE ");
            sbQuery.Append(query.WhereStatement);
            sbQuery.Append(" ORDER BY ");
            sbQuery.Append(query.OrderByAscendingStatement);
            sbQuery.Append(" ");
            sbQuery.Append(query.OrderByDescendingStatement);

            if (query.SkipCount.HasValue && query.TakeCount.HasValue)
            {
                sbQuery.Append($" OFFSET {query.SkipCount.Value} ROWS FETCH NEXT {query.TakeCount.Value} ROWS ONLY");
            }

            MsSQLDataProvider<T> dataProvider = new MsSQLDataProvider<T>();

            return dataProvider.GetList(sbQuery.ToString(), query.ConnectionString);
        }

        /// <summary>
        /// Sorts the data which defined property as ascending (can be applied multiple times)
        /// </summary>
        /// <typeparam name="T">The type of entity class</typeparam>
        /// <param name="query">The query which applied to SQL</param>
        /// <param name="expression">Sorting expression</param>
        /// <returns></returns>
        public static IQuery<T> SortBy<T>(this IQuery<T> query, Expression<Func<T, object>> expression)
        {
            if (query.OrderByAscendingStatement.Length > 0)
            {
                query.OrderByAscendingStatement.Append(", ");
            }

            var orderByExpressions = ExpressionUtil.GetOrderByExpression<T>(expression.Body);

            string orderByStatement = typeof(T).Name.ToLower() + "." + orderByExpressions.MemberName + " ASC";

            query.OrderByAscendingStatement.Append(orderByStatement);

            return query;
        }

        /// <summary>
        /// Sorts the data which defined property as descending (can be applied multiple times)
        /// </summary>
        /// <typeparam name="T">The type of entity class</typeparam>
        /// <param name="query">The query which applied to SQL</param>
        /// <param name="expression">Sorting expression</param>
        /// <returns></returns>
        public static IQuery<T> SortByDesc<T>(this IQuery<T> query, Expression<Func<T, object>> expression)
        {
            if (query.OrderByDescendingStatement.Length > 0)
            {
                query.OrderByDescendingStatement.Append(", ");
            }

            var orderByExpressions = ExpressionUtil.GetOrderByExpression<T>(expression.Body);

            query.OrderByDescendingStatement.Append(orderByExpressions.MemberName);
            query.OrderByDescendingStatement.Append(" DESC");

            return query;
        }

        /// <summary>
        /// Defines a fetch limit for the data which will fetch
        /// </summary>
        /// <typeparam name="T">The type of entity class</typeparam>
        /// <param name="query">The query which applied to SQL</param>
        /// <param name="count">The count of data</param>
        /// <returns></returns>
        public static IQuery<T> Take<T>(this IQuery<T> query, int count)
        {
            query.TakeCount = count;

            return query;
        }

        /// <summary>
        /// Skips the data rows.
        /// </summary>
        /// <typeparam name="T">The type of entity class</typeparam>
        /// <param name="query">The query which applied to SQL</param>
        /// <param name="skip">The skip count of data</param>
        /// <returns></returns>
        public static IQuery<T> Skip<T>(this IQuery<T> query, int skip)
        {
            query.SkipCount = skip;

            return query;
        }

        /// <summary>
        /// Generates a relationship query between two table
        /// </summary>
        /// <typeparam name="T">The type of left table</typeparam>
        /// <typeparam name="TOther">The type of right table</typeparam>
        /// <param name="leftTableColumn">The column of left table</param>
        /// <param name="rightOtherTableColumn">The column of right table</param>
        /// <returns></returns>
        public static IQuery<TOther> JoinWith<T, TOther>(this IQuery<T> query, Expression<Func<T, object>> leftTableColumn, Expression<Func<TOther, object>> rightOtherTableColumn)
        {
            IQuery<TOther> result = Activator.CreateInstance(typeof(Entity<TOther>), args: query.ConnectionString) as Entity<TOther>;

            result.FromStatement += query.FromStatement;
            result.WhereStatement.Append(query.WhereStatement);

            JoinExpressionResult joinExpression = ExpressionUtil.GetJoinExpression<TOther>(rightOtherTableColumn.Body);
            joinExpression.FromColumnName = ExpressionUtil.GetJoinExpression<T>(leftTableColumn).ColumnName;

            query.JoinStatement.Append(" JOIN ");
            query.JoinStatement.Append(joinExpression.TableName);
            query.JoinStatement.Append(" ");
            query.JoinStatement.Append(joinExpression.TableName.ToLower());
            query.JoinStatement.Append(" ON ");
            query.JoinStatement.Append(typeof(T).Name.ToLower());
            query.JoinStatement.Append(".");
            query.JoinStatement.Append(joinExpression.FromColumnName);
            query.JoinStatement.Append(" = ");
            query.JoinStatement.Append(joinExpression.TableName.ToLower());
            query.JoinStatement.Append(".");
            query.JoinStatement.Append(joinExpression.ColumnName);

            result.JoinStatement.Append(query.JoinStatement);

            return result;
        }

        /// <summary>
        /// Creates a new type instance for each returned data result item.
        /// </summary>
        /// <typeparam name="T">The current type of the data</typeparam>
        /// <typeparam name="TResult">New type of the data</typeparam>
        /// <param name="query">The query which applied to SQL</param>
        /// <param name="expression">The expression which create a new instance</param>
        /// <returns></returns>
        public static IQuery<TResult> Select<T, TResult>(this IQuery<T> query, Expression<Func<T, TResult>> expression)
        {
            IQuery<TResult> result = Activator.CreateInstance(typeof(Entity<TResult>), args: query.ConnectionString) as Entity<TResult>;

            result.FromStatement += query.FromStatement;
            result.WhereStatement.Append(query.WhereStatement);
            result.JoinStatement.Append(query.JoinStatement);

            result.Selects = ExpressionUtil.GetSelectExpression<TResult>(expression.Body);

            return result;
        }

        /// <summary>
        /// Creates a new type instance for each returned data result item which generated by SQL join statement.
        /// </summary>
        /// <typeparam name="T">The left join side type of the data</typeparam>
        /// <typeparam name="TOther">The right join side type of data</typeparam>
        /// <typeparam name="TResult">New type of the data</typeparam>
        /// <param name="query">The query which applied to SQL</param>
        /// <param name="expression">The expression which create a new instance</param>
        /// <returns></returns>
        public static IQuery<TResult> Select<T, TOther, TResult>(this IQuery<T> query, Expression<Func<T, TOther, TResult>> expression)
        {
            IQuery<TResult> result = Activator.CreateInstance(typeof(Entity<TResult>), args: query.ConnectionString) as Entity<TResult>;

            result.FromStatement += query.FromStatement;
            result.WhereStatement.Append(query.WhereStatement);
            result.JoinStatement.Append(query.JoinStatement);

            result.Selects = ExpressionUtil.GetSelectExpression<TResult>(expression.Body);

            return result;
        }
    }
}
