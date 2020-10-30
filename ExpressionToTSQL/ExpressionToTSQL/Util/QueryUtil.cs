using ExpressionToTSQL.Abstractions;
using ExpressionToTSQL.Model;
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
            query.Expressions.Add(expression);
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

            sbQuery.Append("SELECT TOP 1 * FROM ");
            sbQuery.Append(typeof(T).Name);

            if (query.Expressions.Any())
            {
                sbQuery.Append(" WHERE ");

                foreach (var statement in query.Expressions)
                {
                    List<WhereExpressionResult> expressions = new List<WhereExpressionResult>();
                    expressions = ExpressionUtil.GetWhereExpressions<T>(statement.Body, expressions);

                    string whereStatement = TextUtil.ConvertToSqlWhereStatement(expressions);

                    sbQuery.Append(whereStatement);
                }
            }

            if (query.OrderByAscendingExpressions.Any() || query.OrderByDescendingExpressions.Any())
            {
                sbQuery.Append(" ORDER BY ");

                var orderByExpressions = query.OrderByAscendingExpressions.Select(x => new OrderByExpressionResult()
                {
                    MemberName = ExpressionUtil.GetOrderByExpression<T>(x.Body).MemberName,
                    IsAscending = true
                }).Union(query.OrderByDescendingExpressions.Select(x => new OrderByExpressionResult()
                {
                    MemberName = ExpressionUtil.GetOrderByExpression<T>(x.Body).MemberName,
                    IsAscending = false
                })).ToList();

                sbQuery.Append(TextUtil.ConvertToSqlOrderByStatement(orderByExpressions));
            }

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

            sbQuery.Append("SELECT * FROM ");
            sbQuery.Append(typeof(T).Name);

            if (query.Expressions.Any())
            {
                sbQuery.Append(" WHERE ");

                foreach (var statement in query.Expressions)
                {
                    List<WhereExpressionResult> expressions = new List<WhereExpressionResult>();
                    expressions = ExpressionUtil.GetWhereExpressions<T>(statement.Body, expressions);

                    string whereStatement = TextUtil.ConvertToSqlWhereStatement(expressions);

                    sbQuery.Append(whereStatement);
                }
            }

            if (query.OrderByAscendingExpressions.Any() || query.OrderByDescendingExpressions.Any())
            {
                sbQuery.Append(" ORDER BY ");

                var orderByExpressions = query.OrderByAscendingExpressions.Select(x => new OrderByExpressionResult()
                {
                    MemberName = ExpressionUtil.GetOrderByExpression<T>(x.Body).MemberName,
                    IsAscending = true
                }).Union(query.OrderByDescendingExpressions.Select(x => new OrderByExpressionResult()
                {
                    MemberName = ExpressionUtil.GetOrderByExpression<T>(x.Body).MemberName,
                    IsAscending = false
                })).ToList();

                sbQuery.Append(TextUtil.ConvertToSqlOrderByStatement(orderByExpressions));
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
            query.OrderByAscendingExpressions.Add(expression);

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
            query.OrderByDescendingExpressions.Add(expression);

            return query;
        }
    }
}
