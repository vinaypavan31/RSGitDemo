
using ECart.Models;
using Microsoft.EntityFrameworkCore;

namespace ECart.Helpers
{
    public class DataContext : DbContext

    {

        public DataContext(DbContextOptions<DataContext> options): base(options)
        {

        }
        
        public DbSet<User>  User{get; set;}
        public DbSet<Product> Product {get; set;}
        public DbSet<UserDetails> UserDetails{get;set;}
        
     

    }

}