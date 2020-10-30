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
        public string ConnectionString { get; set; }
        public Entity(string connectionString)
        {
            ConnectionString = connectionString;
        }

        /// <summary>
        /// Where the query statements will be stored
        /// </summary>
        public List<Expression<Func<T, bool>>> Expressions { get; set; } = new List<Expression<Func<T, bool>>>();
    }
}
