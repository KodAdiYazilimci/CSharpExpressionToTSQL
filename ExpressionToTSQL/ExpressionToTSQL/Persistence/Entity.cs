using ExpressionToTSQL.Abstractions;

using System;
using System.Collections.Generic;
using System.Linq.Expressions;

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
        /// Where the query statements will be stored
        /// </summary>
        public List<Expression<Func<T, bool>>> Expressions { get; set; } = new List<Expression<Func<T, bool>>>();

        /// <summary>
        /// Where OrderBy Asc statements will be stored
        /// </summary>
        public List<Expression<Func<T, object>>> OrderByAscendingExpressions { get; set; } = new List<Expression<Func<T, object>>>();

        /// <summary>
        /// Where OrderBy Desc statements will be stored
        /// </summary>
        public List<Expression<Func<T, object>>> OrderByDescendingExpressions { get; set; } = new List<Expression<Func<T, object>>>();
    }
}
