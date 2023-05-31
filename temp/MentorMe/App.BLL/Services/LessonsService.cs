using App.BLL.Contracts;
using App.DAL.Contracts;
using Base.BLL;
using Base.Mapper.Contracts;
using BLL.DTO.Lessons;
using BLL.DTO.Profiles;
using BLL.DTO.Subjects;
using Domain.Entities;
using Public.DTO.v1.Lessons;
using Payment = Domain.Entities.Payment;
using Tag = Domain.Entities.Tag;

namespace App.BLL.Services;

public class LessonsService:
    BaseEntityService<BLLLessonData, Lesson, ILessonsRepository>, ILessonsService
{
    protected readonly IAppUOW Uow;
    protected readonly IMapper<BLLLessonListElement, Lesson> LessonListMapper;
    protected readonly IMapper<BLLStudentPaymentMethod, StudentPaymentMethod> PaymentMethodMapper;
    protected readonly IMapper<BLLAvailability, TutorAvailability> TutorAvailabilityMapper;
    protected readonly IMapper<BLLSubjectsFilterElement, Subject> SubjectsFilterMapper;
    protected readonly IMapper<BLLPayment, Payment> PaymentMapper;
    
    public LessonsService(IAppUOW uow, IMapper<BLLLessonData, Lesson> dataMapper, IMapper<BLLLessonListElement, Lesson> listMapper,
        IMapper<BLLStudentPaymentMethod, StudentPaymentMethod> paymentMethodMapper, IMapper<BLLAvailability, TutorAvailability> availabilityMapper,
        IMapper<BLLSubjectsFilterElement, Subject> subjectsFilterMapper, IMapper<BLLPayment, Payment> paymentMapper)
        : base(uow.LessonsRepository, dataMapper)
    {
        Uow = uow;
        LessonListMapper = listMapper;
        PaymentMethodMapper = paymentMethodMapper;
        TutorAvailabilityMapper = availabilityMapper;
        SubjectsFilterMapper = subjectsFilterMapper;
        PaymentMapper = paymentMapper;
    }

    public async Task<BLLLessonData?> GetLessonData(Guid userId, Guid lessonId)
    {
        var lesson = await Uow.LessonsRepository.FindLessonById(lessonId);
        var mappedLesson = Mapper.Map(lesson)!;
        mappedLesson.ViewedByTutor = !await Uow.StudentsRepository.UserIsStudent(userId);
        return mappedLesson;
    }

    public async Task<IEnumerable<BLLLessonListElement>> GetLessonsList(Guid userId)
    {
        var lessons = await Uow.LessonsRepository.GetLessonsList(userId);
        return lessons.Select(l => LessonListMapper.Map(l))!;
    }

    public async Task<BLLReserveLessonData> GetReserveLessonData(Guid studentId, Guid? tutorId)
    {
        var studentPaymentMethods = await Uow.StudentsRepository.GetStudentPaymentMethods(studentId);
        var tutorAvailabilities = await Uow.TutorsRepository.GetTutorAvailabilities(tutorId);
        var tutorSubjects = await Uow.SubjectsRepository.GetUserSubjects(tutorId);
        return new BLLReserveLessonData
        {
            PaymentMethods = studentPaymentMethods.Select(s => PaymentMethodMapper.Map(s)),
            Availabilities = tutorAvailabilities.Select(t => TutorAvailabilityMapper.Map(t)),
            Subjects = tutorSubjects!.Select(t => SubjectsFilterMapper.Map(t))
        };
    }

    public async Task<Guid> CreateLesson(ReserveLessonRequest reserveLessonRequest, Guid studentId) 
        => await Uow.LessonsRepository.CreateLesson(reserveLessonRequest, studentId);

    public bool LessonBelongsToUser(BLLLessonData lesson, Guid userId)
        => lesson.LessonStudent.Id == userId || lesson.LessonTutor.Id == userId;

    public Task LeaveReview(UserReview userReview, Guid userId)
    {
        var review = new Review
        {
            Rating = userReview.Rating,
            Comment = userReview.Comment,
            ReviewType = userReview.ReviewType,
            LessonId = userReview.LessonId,
            TutorId = userReview.TutorId,
            StudentId = userReview.StudentId
        };
        
        return Uow.LessonsRepository.AddReview(review);
        
    }

    public Task AddTag(NewTag tag, Guid tutorId)
    {
        var domainTag = new Tag
        {
            Name = tag.Name,
            Description = tag.Description,
            LessonId = tag.LessonId,
            TutorId = tutorId
        };
        
        return Uow.LessonsRepository.AddTag(domainTag);
    }

    public Task DeleteTag(Guid tagId)
        => Uow.LessonsRepository.DeleteTag(tagId);

    public Task CancelLesson(Guid lessonId)
    {
        // TODO: Calculate penalty here
        return Uow.LessonsRepository.CancelLesson(lessonId);
    }

    public Task AcceptDeclineLesson(Guid lessonId, ETutorDecision tutorDecision)
    {
        if (tutorDecision == ETutorDecision.Accept)
        {
            return Uow.LessonsRepository.AcceptLesson(lessonId);
        }
        return Uow.LessonsRepository.DeclineLesson(lessonId);
    }

    public async Task<BLLPayment> GetPaymentData(Guid paymentId) 
        => PaymentMapper.Map(await Uow.LessonsRepository.GetLessonPayment(paymentId))!;
    
}