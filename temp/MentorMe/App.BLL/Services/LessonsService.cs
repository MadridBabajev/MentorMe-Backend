using App.BLL.Contracts;
using App.BLL.Mappers;
using App.DAL.Contracts;
using AutoMapper;
using Base.BLL;
using Base.Mapper.Contracts;
using BLL.DTO.Lessons;
using BLL.DTO.Profiles;
using BLL.DTO.Subjects;
using Domain.Entities;
using Domain.Enums;
using Public.DTO.v1.Lessons;
using Tag = Domain.Entities.Tag;

namespace App.BLL.Services;

public class LessonsService:
    BaseEntityService<BLLLessonData, Lesson, ILessonsRepository>, ILessonsService
{
    protected readonly IAppUOW Uow;
    protected readonly IMapper<BLLLessonData, Lesson> LessonDataMapper;
    protected readonly IMapper<BLLLessonListElement, Lesson> LessonListMapper;
    protected readonly IMapper<BLLStudentPaymentMethod, StudentPaymentMethod> PaymentMethodMapper;
    protected readonly IMapper<BLLTutorAvailability, TutorAvailability> TutorAvailabilityMapper;
    protected readonly IMapper<BLLSubjectsFilterElement, Subject> SubjectsFilterMapper;
    
    public LessonsService(IAppUOW uow, IMapper<BLLLessonData, Lesson> mapper, IMapper automapper)
        : base(uow.LessonsRepository, mapper)
    {
        Uow = uow;
        LessonDataMapper = mapper;
        LessonListMapper = new LessonListMapper(automapper);
        PaymentMethodMapper = new PaymentMethodMapper(automapper);
        TutorAvailabilityMapper = new TutorAvailabilityMapper(automapper);
        SubjectsFilterMapper = new SubjectsFilterMapper(automapper);
    }

    public async Task<BLLLessonData?> GetLessonData(Guid userId, Guid lessonId)
    {
        var lesson = await Uow.LessonsRepository.FindLessonById(lessonId);
        var mappedLesson = Mapper.Map(lesson)!;
        mappedLesson.ViewedByTutor = await Uow.StudentsRepository.UserIsStudent(userId);
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
    {
        return await Uow.LessonsRepository.CreateLesson(reserveLessonRequest, studentId);
    }

    public bool LessonBelongsToUser(BLLLessonData lesson, Guid userId)
        => lesson.LessonStudent.Id == userId || lesson.LessonTutor.Id == userId;

    public void LeaveReview(UserReview userReview, Guid userId)
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
        
        Uow.LessonsRepository.AddReview(review);
    }

    public void AddTag(NewTag tag, Guid tutorId)
    {
        var domainTag = new Tag
        {
            Name = tag.Name,
            Description = tag.Description,
            LessonId = tag.LessonId,
            TutorId = tutorId
        };
        
        Uow.LessonsRepository.AddTag(domainTag);
    }

    public void DeleteTag(Guid tagId)
    {
        Uow.LessonsRepository.DeleteTag(tagId);
    }

    public void CancelLesson(Guid lessonId)
    {
        // TODO: Calculate penalty here
        Uow.LessonsRepository.CancelLesson(lessonId);
    }

    public void AcceptDeclineLesson(Guid lessonId, ETutorDecision tutorDecision)
    {
        if (tutorDecision == ETutorDecision.Accept)
        {
            Uow.LessonsRepository.AcceptLesson(lessonId);
        }
        else
        {
            Uow.LessonsRepository.DeclineLesson(lessonId);
        }
    }
}