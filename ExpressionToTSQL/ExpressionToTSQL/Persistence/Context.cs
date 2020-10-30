using ExpressionToTSQL.Entity;

namespace ExpressionToTSQL.Persistence
{
    /// <summary>
    /// The groups of entities. Also known as DbContext in Entity Framework
    /// </summary>
    public class Context
    {
        private readonly string _connectionString;

        /// <summary>
        /// The groups of entities
        /// </summary>
        /// <param name="connectionString">The MS SQL connection string</param>
        public Context(string connectionString)
        {
            _connectionString = connectionString;
        }

        /// <summary>
        /// The table which in the database, named as "Samples"
        /// </summary>
        public Entity<SampleEntity> Samples
        {
            get
            {
                Entity<SampleEntity> entity = new Entity<SampleEntity>(_connectionString);

                entity.FromStatement = " FROM SampleEntity sampleEntity";

                return entity;
            }
        }
    }
}
