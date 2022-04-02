using Microsoft.EntityFrameworkCore;
using MyLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MyLibrary.Core
{
    public interface IMyRestaurantContext
    {
        DbSet<RefreshToken> RefreshTokens { get; set; }
       
        void Create<TEntity>(TEntity entity) where TEntity : MyRestaurantObject;
        TEntity Modify<TEntity>(TEntity entity) where TEntity : MyRestaurantObject;
        void Delete<TEntity>(TEntity entity) where TEntity : MyRestaurantObject;
        void DeleteRange<TEntity>(IEnumerable<TEntity> entities) where TEntity : MyRestaurantObject;
        Task<TEntity?> GetFirstOrDefaultAsync<TEntity>(Expression<Func<TEntity, bool>> expression, CancellationToken cancellationToken = default) where TEntity : MyRestaurantObject;
        Task<IEnumerable<TEntity>> GetAllAsync<TEntity>(Expression<Func<TEntity, bool>>? expression = null, CancellationToken cancellationToken = default) where TEntity : MyRestaurantObject;
        Task CommitAsync(CancellationToken cancellationToken = default);
    }
}
