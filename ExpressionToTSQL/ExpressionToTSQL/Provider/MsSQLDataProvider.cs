using System;
using System.Data;
using System.Data.SqlClient;

namespace ExpressionToTSQL.Provider
{
    /// <summary>
    /// The data operations for Microsoft SQL Server
    /// </summary>
    /// <typeparam name="T">The type of entity class</typeparam>
    public class MsSQLDataProvider<T>
    {
        /// <summary>
        /// Fetches 1 entity data of data list from database
        /// </summary>
        /// <param name="query">The SQL query which will using via fetching data</param>
        /// <param name="connectionString">The MS SQL Server connection string</param>
        /// <returns></returns>
        public T Get(string query, string connectionString)
        {
            T result = Activator.CreateInstance<T>();

            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(query, sqlConnection);
                DataTable dataTable = new DataTable();
                sqlDataAdapter.Fill(dataTable);

                foreach (DataRow row in dataTable.Rows)
                {
                    var properties = typeof(T).GetProperties();

                    foreach (var property in properties)
                    {
                        if (row[property.Name] != null)
                        {
                            property.SetValue(result, row[property.Name]);
                        }
                    }
                }
            }

            return result;
        }
    }
}
