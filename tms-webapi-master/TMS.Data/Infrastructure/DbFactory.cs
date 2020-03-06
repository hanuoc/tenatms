namespace TMS.Data.Infrastructure
{
    public class DbFactory : Disposable, IDbFactory
    {
        private TMSDbContext dbContext;

        public TMSDbContext Init()
        {
            return dbContext ?? (dbContext = new TMSDbContext());
        }

        protected override void DisposeCore()
        {
            if (dbContext != null)
                dbContext.Dispose();
        }
    }
}