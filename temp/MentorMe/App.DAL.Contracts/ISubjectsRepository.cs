#nullable enable
using Base.DAL.Contracts;
using BLL.DTO.Subjects;
using Domain.Entities;

namespace App.DAL.Contracts;

public interface ISubjectsRepository : IBaseRepository<Subject>, ISubjectsRepositoryCustom<Subject>
{
    // add here custom methods for repo only
    
    public Task<IEnumerable<BLLSubjectListElement>> AllSubjectsAsync();
    public Task<Subject?> FindAsyncWithDetails(Guid subjectId);
    public Task<IEnumerable<Subject?>?> GetUserSubjects(Guid? id);
    Task<bool> CheckIfSubjectIsAdded(Subject subjectId, Guid? userId);
    Task AddStudentSubject(Guid userId, Guid subjectId);
    Task RemoveStudentSubject(Guid userId, Guid subjectId);
    Task AddTutorSubject(Guid userId, Guid subjectId);
    Task RemoveTutorSubject(Guid userId, Guid subjectId);
}

public interface ISubjectsRepositoryCustom<TEntity>
{
    // add here shared methods between repo and service
    
    // Task<IEnumerable<TEntity>> AllSubjectsAsync();
    
    // Task<TEntity?> FindAsync(Guid id);
    
    // Task<TEntity?> RemoveAsync(Guid id, Guid userId);
    
    // Task<bool> IsOwnedByUserAsync(Guid id, Guid userId);
}