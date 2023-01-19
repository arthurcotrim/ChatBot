using ChatBot.Models;
using Microsoft.EntityFrameworkCore;

namespace ChatBot
{
    public class AppDbContext : DbContext
    {
        private readonly IConfiguration configuration;

        public AppDbContext(IConfiguration _configuration)
        {
            configuration = _configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
                optionsBuilder.UseMySql(configuration.GetConnectionString("ConnectionMysql"), ServerVersion.Parse("8.0.32"));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        { }

        public DbSet<ChatModel> ChatModel { get; set; }
    }
}
