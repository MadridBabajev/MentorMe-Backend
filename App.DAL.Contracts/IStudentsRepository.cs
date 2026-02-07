using Base.DAL.Contracts;
using BLL.DTO.Profiles;
using Domain;
using Domain.Entities;
using Domain.Enums;
using Public.DTO.v1.Profiles;

namespace App.DAL.Contracts;

public interface IStudentsRepository : IBaseRepository<Student>, IStudentsRepositoryCustom<Student>
{
    // add here custom methods for repo only
    Task<Student> FindStudentById(Guid? userId);

    Task<List<StudentPaymentMethod>> GetStudentPaymentMethods(Guid userId);

    Task<bool> UserIsStudent(Guid userId);
    Task<Student> GetStudentEditProfileData(Guid userId);
    Task UpdateStudentProfileData(Guid studentId, UpdatedProfileData updatedProfileData);
}

public interface IStudentsRepositoryCustom<TEntity>
{
    // add here shared methods between repo and service
    
    // Task<IEnumerable<TEntity>> AllAsync();
    
    // Task<TEntity?> FindAsync(Guid id, Guid userId);
    
    // Task<TEntity?> RemoveAsync(Guid id, Guid userId);
    
    // Task<bool> IsOwnedByUserAsync(Guid id, Guid userId);
}