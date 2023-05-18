using Base.DAL.Contracts;
using Domain.Entities;
using Public.DTO.v1.Profiles;

namespace App.DAL.Contracts;

public interface ITutorsRepository : IBaseRepository<Tutor>, ITutorsRepositoryCustom<Tutor>
{
    // add here custom methods for repo only
    Task<IEnumerable<Tutor>> AllFilteredAsync(TutorSearchFilters filters);

    Task<Tutor> FindTutorById(Guid? userId);
}

public interface ITutorsRepositoryCustom<TEntity>
{
    // add here shared methods between repo and service
    
    // Task<IEnumerable<TEntity>> AllAsync();
    
    // Task<TEntity?> FindAsync(Guid id, Guid userId);
    
    // Task<TEntity?> RemoveAsync(Guid id, Guid userId);
    
    // Task<bool> IsOwnedByUserAsync(Guid id, Guid userId);
}