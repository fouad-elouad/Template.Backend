using System;

namespace Template.Backend.Data.Repositories
{
    /// <summary>
    /// IDbFactory interface
    /// </summary>
    public interface IDbFactory : IDisposable
    {
        IDbContext Init();
    }
}
