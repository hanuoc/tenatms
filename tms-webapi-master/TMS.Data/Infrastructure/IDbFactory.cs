using System;

namespace TMS.Data.Infrastructure
{
    public interface IDbFactory : IDisposable
    {
        TMSDbContext Init();
    }
}