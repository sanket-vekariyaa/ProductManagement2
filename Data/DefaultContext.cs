using Microsoft.EntityFrameworkCore;
using ProductManagement.Providers;
using ProductManagement.Model;

namespace ProductManagement.Data
{
    public class DefaultContext : ContextTables { public DefaultContext(Connection connection) : base() { CurrentConnection = connection; } }
    public class ContextTables : ContextProvider
    {
        public DbSet<Products> Product { get; set; }
        public DbSet<Category> Category { get; set; }
        public DbSet<ProductImage> ProductImage { get; set; }   
    }
}
