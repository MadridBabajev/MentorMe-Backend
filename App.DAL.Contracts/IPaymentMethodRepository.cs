using Base.DAL.Contracts;
using Public.DTO.v1.Profiles.Secondary;
using StudentPaymentMethod = Domain.Entities.StudentPaymentMethod;

namespace App.DAL.Contracts;

public interface IPaymentMethodRepository : IBaseRepository<StudentPaymentMethod>, IAvailabilityRepositoryCustom<StudentPaymentMethod>
{
    // add here custom methods for repo only
    // Task<Lesson> FindLessonById(Guid lessonId);
    // Task<IEnumerable<Lesson>> GetLessonsList(Guid userId);
    // Task<Guid> CreateLesson(ReserveLessonRequest reserveLessonRequest, Guid studentId);
    // void AddTag(Tag tag);
    // void DeleteTag(Guid tagId);
    // void CancelLesson(Guid lessonId);
    // void AcceptLesson(Guid lessonId);
    // void DeclineLesson(Guid lessonId);
    // void AddReview(Review review);
    Task<IEnumerable<StudentPaymentMethod>> GetAllById(Guid userId);
    Task DeleteById(Guid paymentMethodId);
    Task AddNewPaymentMethod(NewPaymentMethod newPaymentMethod, Guid studentId);
}

public interface IPaymentMethodRepositoryCustom<TEntity>
{
    // add here shared methods between repo and service
    
    // Task<IEnumerable<TEntity>> AllAsync();
    
    // Task<TEntity?> FindAsync(Guid id, Guid userId);
    
    // Task<TEntity?> RemoveAsync(Guid id, Guid userId);
    
    // Task<bool> IsOwnedByUserAsync(Guid id, Guid userId);
}