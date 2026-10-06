
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace WCNApi.core.Data
{
    /// <summary>
    /// public interface IDBContext
    /// </summary>
    public interface IDbContext
    {
        /// <summary>
        /// Get DBSet
        /// </summary>
        /// <typeparam name="TEntity">Entity type</typeparam>
        /// <returns>DBSet</returns>
        DbSet<TEntity> Set<TEntity>() where TEntity : class;

        /// <summary>
        /// Save changes
        /// </summary>
        /// <returns></returns>
        int SaveChanges();

        /// <summary>
        /// EXCECUTE the SQL command.
        /// </summary>
        /// <param name="sql">The SQL.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns></returns>
        int ExecuteSqlRaw(string sql, params object[] parameters);

        /// <summary>
        /// db entity Entry
        /// </summary>
        /// <param name="entity">entity</param>
        /// <returns></returns>
        EntityEntry Entry(object entity);

        /// <summary>
        /// get database
        /// </summary>
        DatabaseFacade Db { get; }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        void DisposeContext();
    }
}
