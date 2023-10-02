using System;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace Portfolio.Backend.Common.Helpers
{
	public class DatabaseHelper
	{
		private readonly IConfiguration _configuration;
		private readonly string _connStr;
		public DatabaseHelper(IConfiguration configuration)
		{
			_configuration = configuration;
			_connStr = _configuration.GetConnectionString("SqlServer");
		}


		public List<T> RunQuery<T>(string query)
		{
            List<T> res = new();
            using (SqlConnection conn = new(_connStr))
			{
				conn.Open();
				SqlCommand command = new(query, conn);
				using(SqlDataReader reader = command.ExecuteReader())
				{
					while(reader.Read())
					{
						Type type = typeof(T);
						T temp = Activator.CreateInstance<T>();
						foreach(var prop in type.GetProperties())
						{
							prop.SetValue(temp, Convert.ChangeType(reader[prop.Name], prop.PropertyType));
						}
						res.Add(temp);
					}
				}
			}
			return res;
		}

        public T RunQueryScalar<T>(string query)
        {
            T res = Activator.CreateInstance<T>();
            using (SqlConnection conn = new(_connStr))
            {
                conn.Open();
                SqlCommand command = new(query, conn);
				res = (T)command.ExecuteScalar();
                
            }
            return res;
        }
    }
}

