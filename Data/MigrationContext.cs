using Microsoft.EntityFrameworkCore;
using ProductManagement.Providers;

namespace ProductManagement.Data
{
    public class MigrationContext : ContextTables
    {
        public class PostgresContext : ContextTables
        {
            public PostgresContext() : base() { CurrentConnection = new Connection { DatabaseType = (byte)DatabaseType.Postgres, Server = "localhost", Database = "ProductManagement", User = "postgres", Password = "Avni@003",Port=5432}; }
            public PostgresContext(Connection connection) : base() { CurrentConnection = connection; }
            public PostgresContext(Connection connection, bool isMigrate) : base() { CurrentConnection = connection; if (isMigrate) { Database.Migrate(); } }
        }
    }
}
// Add-Migration FirstMigration -Context PostgresContext -o Migrations\SqlServer
// update-database -Context PostgresContext