#nullable enable
using Base.DAL.Contracts;
using BLL.DTO;
using Domain.Entities;

namespace App.DAL.Contracts;

public interface ISubjectsRepository : IBaseRepository<Subject>, ISubjectsRepositoryCustom<Subject>
{
    // add here custom methods for repo only
    
    public Task<IEnumerable<BLLSubjectListElement>> AllSubjectsAsync();
    public Task<BLLSubjectDetails?> FindAsyncWithDetails(Guid id);
}

public interface ISubjectsRepositoryCustom<TEntity>
{
    // add here shared methods between repo and service
    
    // Task<IEnumerable<TEntity>> AllSubjectsAsync();
    
    // Task<TEntity?> FindAsync(Guid id);
    
    // Task<TEntity?> RemoveAsync(Guid id, Guid userId);
    
    // Task<bool> IsOwnedByUserAsync(Guid id, Guid userId);
}