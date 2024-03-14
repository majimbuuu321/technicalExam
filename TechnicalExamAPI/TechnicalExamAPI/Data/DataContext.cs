using Microsoft.EntityFrameworkCore;
using TechnicalExamAPI.Models;

namespace TechnicalExamAPI.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }

        public DbSet<Customer> customer => Set<Customer>();
        public DbSet<Account> account => Set<Account>();
    }
}
