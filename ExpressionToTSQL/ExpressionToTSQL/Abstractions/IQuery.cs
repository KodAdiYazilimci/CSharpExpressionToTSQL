using ExpressionToTSQL.Model;

using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

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
        /// The Select body of query
        /// </summary>
        string FromStatement { get; set; }

        /// <summary>
        /// Where the query statements will be stored
        /// </summary>
        StringBuilder WhereStatement { get; set; }

        /// <summary>
        /// Where OrderBy Asc statements will be stored
        /// </summary>
        StringBuilder OrderByAscendingStatement { get; set; }

        /// <summary>
        /// Where OrderBy Desc statements will be stored
        /// </summary>
        StringBuilder OrderByDescendingStatement { get; set; }

        /// <summary>
        /// Join body of query
        /// </summary>
        StringBuilder JoinStatement { get; set; }

        /// <summary>
        /// The count of data which will fetch
        /// </summary>
        int? TakeCount { get; set; }

        /// <summary>
        /// The skip row count of the data which will fetch
        /// </summary>
        int? SkipCount { get; set; }

        SelectExpressionResult<T> Selects { get; set; }
    }
}
