using Microsoft.Extensions.Configuration;
using Npgsql;

namespace Portfolio.Backend.Common.Helpers
{
	public class DatabaseHelper
	{
		private readonly IConfiguration _configuration;
		private readonly string _connStr;
		public DatabaseHelper(IConfiguration configuration)
		{
			_configuration = configuration;
			_connStr = _configuration.GetConnectionString("PostgreSQL");
		}


		public List<T> RunQuery<T>(string query)
		{
			List<T> res = new();
			using (var conn = NpgsqlDataSource.Create(_connStr))
			{
				var command = conn.CreateCommand(query);
				using (var reader = command.ExecuteReader())
				{
					while (reader.Read())
					{
						Type type = typeof(T);
						T temp = Activator.CreateInstance<T>();
						foreach (var prop in type.GetProperties())
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
			using (var conn = NpgsqlDataSource.Create(_connStr))
			{
				var command = conn.CreateCommand(query);
				res = (T)command.ExecuteScalar();

			}
			return res;
		}
	}
}

