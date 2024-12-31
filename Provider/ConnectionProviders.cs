using Microsoft.EntityFrameworkCore;
using Npgsql;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ProductManagement.Providers
{
    [Table($"Config{nameof(Connection)}")]
    public class Connection
    {
        //[Key] public int Id { get; set; } 
        [Required] public byte DatabaseType { get; set; } = 1;
        [StringLength(100)] public string Server { get; set; } = "localhost";
        [StringLength(100)] public string Database { get; set; } = "ProductManagement";
        [StringLength(100)] public string User { get; set; } = "postgres";
        [StringLength(100)] public string Password { get; set; } = "Avni@003";
        [Required] public bool IsActive { get; set; } = true;
        [Required] public bool IsDeleted { get; set; } = false;
        public int? Port { get; set; } = 5432;
    }
    public class ContextProvider : DbContext
    {
        internal class ConnectionContext : ContextProvider { public ConnectionContext(Connection connection) : base() { CurrentConnection = connection; } public DbSet<Connection> Connection { get; set; } }
        public enum DatabaseType : byte {Postgres = 1 ,Mongodb = 2}
        public static Connection CurrentConnection { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder = (DatabaseType)CurrentConnection.DatabaseType switch
            {
                DatabaseType.Postgres => optionsBuilder.UseNpgsql(ConnectionBuilder.GetPostgresSqlConnection(CurrentConnection)), 
                //DatabaseType.Mongodb => optionsBuilder.UseMongoDb(ConnectionBuilder.GetMongoDbConnection(CurrentConnection))
            };
            base.OnConfiguring(optionsBuilder);
        }
    }
    public static class ConnectionBuilder
    {
        public static string GetPostgresSqlConnection(Connection connection)
        {
            NpgsqlConnectionStringBuilder connectionBuilder = new()
            {
                Host = connection.Server,
                Database = connection.Database,
                Username = connection.User,
                Password = connection.Password
            };
            return connectionBuilder.ConnectionString;
        }
        //public static string GetMongoDbConnection(Connection connection)
        //{
        //    var connectionString = new MongoUrlBuilder()
        //    {
        //        Server = new MongoServerAddress(connection.Server, connection.Port ?? 27017),
        //        Username = connection.User,
        //        Password = connection.Password,
        //        DatabaseName = connection.Database
        //    };
        //    return connectionString.ToString();
        //}
    }
}
