using Microsoft.EntityFrameworkCore;

namespace Data
{
    public interface IDbFactory
    {
        DbContext Initialize();
    }
    public class DbFactory : IDbFactory
    {
        private readonly DbContext _appDbContext;
        public DbFactory(DbContext dbContext)
        {
            _appDbContext = dbContext;
        }
        public DbContext Initialize()
        {
            return _appDbContext;
        }
    }
}
