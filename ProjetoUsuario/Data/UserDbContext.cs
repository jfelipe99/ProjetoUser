using System.Net.Mime;
using Microsoft.EntityFrameworkCore;
using ProjetoUsuario.Models;

namespace ProjetoUsuario.Data
{
    public class UserDbContext : DbContext
    {
        public DbSet<User> Users {get;set;}

        public UserDbContext(DbContextOptions<UserDbContext> options) :
            base(options)
        {

        }
    }
}