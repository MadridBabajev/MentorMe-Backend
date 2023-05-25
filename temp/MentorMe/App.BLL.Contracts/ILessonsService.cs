using App.DAL.Contracts;
using Base.DAL.Contracts;
using BLL.DTO.Lessons;
using Public.DTO.v1.Lessons;

namespace App.BLL.Contracts;

public interface ILessonsService: IBaseRepository<BLLLessonData>, ILessonsRepositoryCustom<BLLLessonData>
{
    Task<BLLLessonData?> GetLessonData(Guid userId, Guid lessonId);

    Task<IEnumerable<BLLLessonListElement>> GetLessonsList(Guid userId);

    public Task<BLLReserveLessonData> GetReserveLessonData(Guid studentId, Guid? tutorId);
    Task<Guid> CreateLesson(ReserveLessonRequest reserveLessonRequest, Guid studentId);
    bool LessonBelongsToUser(BLLLessonData lesson, Guid userId);
    void LeaveReview(UserReview userReview, Guid userId);
    void AddTag(NewTag tag, Guid tutorId);
    void DeleteTag(Guid tagTagId);
    void CancelLesson(Guid lessonId);
    void AcceptDeclineLesson(Guid lessonId, ETutorDecision tutorDecision);
}