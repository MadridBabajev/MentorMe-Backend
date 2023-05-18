using Base.DAL.Contracts;
using Domain;
using Domain.Entities;

namespace App.DAL.Contracts;

public interface IStudentsRepository : IBaseRepository<Student>, IStudentsRepositoryCustom<Student>
{
    // add here custom methods for repo only
    Task<Student> FindStudentById(Guid userId);
}

public interface IStudentsRepositoryCustom<TEntity>
{
    // add here shared methods between repo and service
    
    // Task<IEnumerable<TEntity>> AllAsync();
    
    // Task<TEntity?> FindAsync(Guid id, Guid userId);
    
    // Task<TEntity?> RemoveAsync(Guid id, Guid userId);
    
    // Task<bool> IsOwnedByUserAsync(Guid id, Guid userId);
}