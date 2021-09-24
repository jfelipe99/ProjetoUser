using Microsoft.Extensions.Caching.Memory;
using ProjetoUsuario.Data;

namespace ProjetoUsuario.Services
{
    public class GenericServices
    {
        public UserDbContext _context;
        public readonly IMemoryCache _memoryCache;

        public GenericServices(
            UserDbContext context,
            IMemoryCache memoryCache)
        {
            _context = context;
            _memoryCache = memoryCache;
        }      
    }
}