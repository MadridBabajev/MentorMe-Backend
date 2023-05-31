#nullable enable
using Base.DAL.Contracts;
using BLL.DTO.Lessons;
using Domain.Entities;
using Public.DTO.v1.Lessons;
using Payment = Domain.Entities.Payment;
using Tag = Domain.Entities.Tag;

namespace App.DAL.Contracts;

public interface ILessonsRepository : IBaseRepository<Lesson>, ILessonsRepositoryCustom<Lesson>
{
    // add here custom methods for repo only
    Task<Lesson> FindLessonById(Guid lessonId);
    Task<IEnumerable<Lesson>> GetLessonsList(Guid userId);
    Task<Guid> CreateLesson(ReserveLessonRequest reserveLessonRequest, Guid studentId);
    Task AddTag(Tag tag);
    Task DeleteTag(Guid tagId);
    Task CancelLesson(Guid lessonId);
    Task AcceptLesson(Guid lessonId);
    Task DeclineLesson(Guid lessonId);
    Task AddReview(Review review);
    Task<Payment> GetLessonPayment(Guid paymentId);
}

public interface ILessonsRepositoryCustom<TEntity>
{
    // add here shared methods between repo and service
    
    // Task<IEnumerable<TEntity>> AllAsync();
    
    // Task<TEntity?> FindAsync(Guid id, Guid userId);
    
    // Task<TEntity?> RemoveAsync(Guid id, Guid userId);
    
    // Task<bool> IsOwnedByUserAsync(Guid id, Guid userId);
}