using Base.DAL.Contracts;
using Domain;
using Domain.Entities;
using Domain.Enums;

namespace App.DAL.Contracts;

public interface IStudentsRepository : IBaseRepository<Student>, IStudentsRepositoryCustom<Student>
{
    // add here custom methods for repo only
    Task<Student> FindStudentById(Guid userId);

    Task<List<StudentPaymentMethod>> GetStudentPaymentMethods(Guid userId);

    Task<bool> UserIsStudent(Guid userId);
}

public interface IStudentsRepositoryCustom<TEntity>
{
    // add here shared methods between repo and service
    
    // Task<IEnumerable<TEntity>> AllAsync();
    
    // Task<TEntity?> FindAsync(Guid id, Guid userId);
    
    // Task<TEntity?> RemoveAsync(Guid id, Guid userId);
    
    // Task<bool> IsOwnedByUserAsync(Guid id, Guid userId);
}