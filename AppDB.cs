using System;
using MySqlConnector;

namespace migracao_rebranding
{
    public class AppDB : IDisposable
    {
        public MySqlConnection Connection { get; }

        public AppDB(string connectionString)
        {
            Connection = new MySqlConnection(connectionString);
        }

        public void Dispose()
        {
            Connection.Dispose();
        }
    }
}