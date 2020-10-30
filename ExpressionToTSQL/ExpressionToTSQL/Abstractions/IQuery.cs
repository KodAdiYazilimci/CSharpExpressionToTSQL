using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace ExpressionToTSQL.Abstractions
{
    /// <summary>
    /// The query contract of entities. Also known as IQueryable<T>
    /// </summary>
    /// <typeparam name="T">The type of entity class</typeparam>
    public interface IQuery<T>
    {
        /// <summary>
        /// The connection string which will be used to fetch data from database
        /// </summary>
        string ConnectionString { get; set; }

        /// <summary>
        /// Where the query statements will be stored
        /// </summary>
        List<Expression<Func<T, bool>>> Expressions { get; set; }

        /// <summary>
        /// Where OrderBy Asc statements will be stored
        /// </summary>
        List<Expression<Func<T, object>>> OrderByAscendingExpressions { get; set; }

        /// <summary>
        /// Where OrderBy Desc statements will be stored
        /// </summary>
        List<Expression<Func<T,object>>> OrderByDescendingExpressions { get; set; }
    }
}
