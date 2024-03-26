using MySql.Data.MySqlClient;

namespace LangLa.SqlConnection
{
	public static class Connection
	{
		private static string connectionString = string.Format("SERVER={0};DATABASE={1};UID={2};PASSWORD={3};",
                Program.AppSettings.Server,
                Program.AppSettings.Database,
                Program.AppSettings.Username,
                Program.AppSettings.Password
            );

		public static MySqlConnection getConnection()
		{
            return new MySqlConnection(connectionString);
		}
	}
}
