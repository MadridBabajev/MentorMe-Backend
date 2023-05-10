using Base.DAL.Contracts;
using Domain.Entities;

namespace App.DAL.Contracts;

public interface ITutorsRepository : IBaseRepository<Tutor>, ITutorsRepositoryCustom<Tutor>
{
    // add here custom methods for repo only
}

public interface ITutorsRepositoryCustom<TEntity>
{
    // add here shared methods between repo and service
    
    // Task<IEnumerable<TEntity>> AllAsync();
    
    // Task<TEntity?> FindAsync(Guid id, Guid userId);
    
    // Task<TEntity?> RemoveAsync(Guid id, Guid userId);
    
    // Task<bool> IsOwnedByUserAsync(Guid id, Guid userId);
}