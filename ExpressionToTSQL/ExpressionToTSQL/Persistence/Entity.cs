using ExpressionToTSQL.Abstractions;
using ExpressionToTSQL.Model;

using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace ExpressionToTSQL.Persistence
{
    /// <summary>
    /// Delegates of the table in database. Also known as DbSet<T> in Entity Framework
    /// </summary>
    /// <typeparam name="T">The type of entity class</typeparam>
    public class Entity<T> : IQuery<T>
    {
        /// <summary>
        /// The connection string which will be used for database operations
        /// </summary>
        public string ConnectionString { get; set; }

        /// <summary>
        /// Delegates of the table in database. Also known as DbSet<T> in Entity Framework
        /// </summary>
        /// <param name="connectionString">The connection string which will be used for database operations</param>
        public Entity(string connectionString)
        {
            ConnectionString = connectionString;
        }

        /// <summary>
        /// The count of data which will fetch
        /// </summary>
        public int? TakeCount { get; set; }

        /// <summary>
        /// The skip row count of the data which will fetch
        /// </summary>
        public int? SkipCount { get; set; }

        /// <summary>
        /// Where the query statements will be stored
        /// </summary>
        public StringBuilder WhereStatement { get; set; } = new StringBuilder();

        /// <summary>
        /// Where OrderBy Asc statements will be stored
        /// </summary>
        public StringBuilder OrderByAscendingStatement { get; set; } = new StringBuilder();

        /// <summary>
        /// Where OrderBy Desc statements will be stored
        /// </summary>
        public StringBuilder OrderByDescendingStatement { get; set; } = new StringBuilder();

        /// <summary>
        /// Join body of query
        /// </summary>
        public StringBuilder JoinStatement { get; set; } = new StringBuilder();

        /// <summary>
        /// The Select body of query
        /// </summary>
        public string FromStatement { get; set; }

        public SelectExpressionResult<T> Selects { get; set; } = new SelectExpressionResult<T>();
    }
}
