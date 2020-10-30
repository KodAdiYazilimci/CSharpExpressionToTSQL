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
        /// Adds a statement to query
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
        /// <param name="query"></param>
        /// <returns></returns>
        public static T FirstOrDefault<T>(this IQuery<T> query)
        {
            StringBuilder sbQuery = new StringBuilder();

            sbQuery.Append("SELECT TOP 1 * FROM ");
            sbQuery.Append(typeof(T).Name);

            if (query.Expressions.Any())
            {
                sbQuery.Append(" WHERE ");

                foreach (var statement in query.Expressions)
                {
                    List<ExpressionResult> expressions = new List<ExpressionResult>();
                    expressions = ExpressionUtil.GetExpressions<T>(statement.Body, expressions);

                    string whereStatement = TextUtil.ConvertToSql(expressions);

                    sbQuery.Append(whereStatement);
                }
            }

            MsSQLDataProvider<T> dataProvider = new MsSQLDataProvider<T>();

            return dataProvider.Get(sbQuery.ToString(), query.ConnectionString);
        }
    }

}
