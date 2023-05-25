#nullable enable
using Base.DAL.Contracts;
using Domain.Entities;
using Public.DTO.v1.Lessons;
using Tag = Domain.Entities.Tag;

namespace App.DAL.Contracts;

public interface ILessonsRepository : IBaseRepository<Lesson>, ILessonsRepositoryCustom<Lesson>
{
    // add here custom methods for repo only
    Task<Lesson> FindLessonById(Guid lessonId);
    Task<IEnumerable<Lesson>> GetLessonsList(Guid userId);
    Task<Guid> CreateLesson(ReserveLessonRequest reserveLessonRequest, Guid studentId);
    void AddTag(Tag tag);
    void DeleteTag(Guid tagId);
    void CancelLesson(Guid lessonId);
    void AcceptLesson(Guid lessonId);
    void DeclineLesson(Guid lessonId);
    void AddReview(Review review);
}

public interface ILessonsRepositoryCustom<TEntity>
{
    // add here shared methods between repo and service
    
    // Task<IEnumerable<TEntity>> AllAsync();
    
    // Task<TEntity?> FindAsync(Guid id, Guid userId);
    
    // Task<TEntity?> RemoveAsync(Guid id, Guid userId);
    
    // Task<bool> IsOwnedByUserAsync(Guid id, Guid userId);
}