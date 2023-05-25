
using App.DAL.Contracts;
using Base.DAL.EF;
using Domain.Entities;
using DomainTag = Domain.Entities.Tag;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Public.DTO.v1.Lessons;

namespace App.DAL.EF.Repositories;

public class LessonsRepository: EFBaseRepository<Lesson, ApplicationDbContext>, ILessonsRepository
{
    public LessonsRepository(ApplicationDbContext dataContext) : base(dataContext)
    {
    }
    
    public async Task<Lesson> FindLessonById(Guid lessonId)
    {
        return await RepositoryDbSet
            .Include(l => l.Tutor)
                .ThenInclude(t => t!.AppUser)
            .Include(l => l.Tutor)
                .ThenInclude(t => t!.Reviews)
            .Include(l => l.LessonParticipations)!
                .ThenInclude(lp => lp.Student)
                    .ThenInclude(s => s!.AppUser)
            .Include(l => l.LessonParticipations)!
                .ThenInclude(lp => lp.Student)
                    .ThenInclude(s => s!.Reviews)
            .Include(l => l.Payments)!
                .ThenInclude(lp => lp.Payment)
                    .ThenInclude(p => p!.StudentPaymentMethod)
            .Include(l => l.Subject)
            .Include(l => l.Tags)
            .FirstAsync(l => l.Id == lessonId);
    }
    
    public async Task<IEnumerable<Lesson>> GetLessonsList(Guid userId)
    {
        var user = await RepositoryDbContext.AppUsers.FirstAsync(u => u.Id == userId);
        if (user.AppUserType == EUserType.Tutor)
        {
            // If user is a Tutor, fetch lessons directly from Tutor's Lessons
            return await RepositoryDbContext.Lessons
                .Include(l => l.Tutor)
                    .ThenInclude(t => t!.AppUser)
                .Include(l => l.Subject)
                .Include(l => l.LessonParticipations)!
                    .ThenInclude(lp => lp.Student)
                        .ThenInclude(s => s!.AppUser)
                .Where(l => l.Tutor!.AppUserId == userId)
                .ToListAsync();
        }

        // If user is a Student, fetch lessons from LessonParticipation
        return (await RepositoryDbContext.Lessons
            .Include(l => l.Tutor)
                .ThenInclude(t => t!.AppUser)
            .Include(l => l.Subject)
            .Include(l => l.LessonParticipations)!
                .ThenInclude(lp => lp.Student)
                    .ThenInclude(s => s!.AppUser)
            .Where(l => l.LessonParticipations!.First().Student!.AppUserId == userId)
            .ToListAsync());
    }

    public async Task<Guid> CreateLesson(ReserveLessonRequest reserveLessonRequest, Guid studentId)
    {
        
        // Create a lesson
        
        var lesson = new Lesson
        {
            StartTime = reserveLessonRequest.LessonStartTime.AddHours(3),
            EndTime = reserveLessonRequest.LessonEndTime.AddHours(3),
            LessonDuration = 60,
            IsCanceled = false,
            LessonType = ELessonType.OneOnOne,
            LessonState = ELessonState.Pending,
            TutorId = reserveLessonRequest.TutorId,
            SubjectId = reserveLessonRequest.SubjectId,
            Tags = new List<DomainTag>()
        };

        RepositoryDbSet.Add(lesson);
        
        await RepositoryDbContext.SaveChangesAsync();
        
        // Create lesson participation for the user
        
        var lessonStudent = await RepositoryDbContext.Students
            .FirstAsync(s => s.AppUserId == studentId);
        
        var lessonTutor = await RepositoryDbContext.Tutors
            .FirstAsync(t => t.AppUserId == reserveLessonRequest.TutorId);

        var lessonParticipation = new LessonParticipation
        {
            StudentId = lessonStudent.Id,
            LessonId = lesson.Id
        };
        
        lessonStudent.Lessons ??= new List<LessonParticipation>();
        lesson.LessonParticipations ??= new List<LessonParticipation>();
        lesson.Payments ??= new List<LessonPayment>();
        lessonStudent.LessonPayments ??= new List<LessonPayment>();
        lessonTutor.LessonPayments ??= new List<LessonPayment>();
        
        lessonStudent.Lessons.Add(lessonParticipation);
        lesson.LessonParticipations.Add(lessonParticipation);

        // Create payment for the lesson
        
        var payment = new Payment
        {
            Date = lesson.EndTime,
            Amount = lesson.LessonDuration * lessonTutor.HourlyRate / 60,
            Description = $"Payment for the lesson, which will happen on {lesson.StartTime}",
            StudentPaymentMethodId = reserveLessonRequest.PaymentMethodId,
            PaymentStatus = EPaymentStatus.Reserved,
            LessonPayments = new List<LessonPayment>()
        };

        RepositoryDbContext.Payments.Add(payment);
        
        // await RepositoryDbContext.SaveChangesAsync();
        
        // Create a lesson payment for all entities associated with the lesson 

        var lessonPayment = new LessonPayment
        {
            StudentId = lessonStudent.Id,
            TutorId = lessonTutor.Id,
            LessonId = lesson.Id,
            PaymentId = payment.Id
        };
        
        lessonStudent.LessonPayments.Add(lessonPayment);
        lessonTutor.LessonPayments.Add(lessonPayment);
        payment.LessonPayments.Add(lessonPayment);
        lesson.Payments.Add(lessonPayment);
        
        await RepositoryDbContext.SaveChangesAsync();

        return lesson.Id;
    }

    public async void AddTag(DomainTag tag)
    {
        RepositoryDbContext.Tags.Add(tag);
        await RepositoryDbContext.SaveChangesAsync();
    }
    
    public async void DeleteTag(Guid tagId)
    {
        var tag = await RepositoryDbContext.Tags.FirstAsync(t => t.Id == tagId);
        RepositoryDbContext.Tags.Remove(tag);

        await RepositoryDbContext.SaveChangesAsync();
    }

    public async void CancelLesson(Guid lessonId)
    {
        var lesson = await RepositoryDbSet
            .Include(l => l.Payments)!
            .ThenInclude(lp => lp.Payment)
            .FirstAsync(l => l.Id == lessonId);
        lesson.IsCanceled = true;
        lesson.LessonState = ELessonState.Canceled;
        
        var cancellation = new Cancellation
        {
            Reason = "",
            Penalty = .0,
            CancellationType = ECancellationType.BySystem,
            LessonId = lessonId
        };
        
        RepositoryDbContext.Cancellations.Add(cancellation);
        
        lesson.Cancellation = cancellation;

        var payment = lesson.Payments!.First().Payment;

        payment!.PaymentStatus = EPaymentStatus.Refunded;
        
        await RepositoryDbContext.SaveChangesAsync();
    }

    public async void AcceptLesson(Guid lessonId)
    {
        var lesson = await RepositoryDbSet.FirstAsync(l => l.Id == lessonId);
        lesson.LessonState = ELessonState.Upcoming;
        
        await RepositoryDbContext.SaveChangesAsync();
    }

    public async void DeclineLesson(Guid lessonId)
    {
        var lesson = await RepositoryDbSet.FirstAsync(l => l.Id == lessonId);
        lesson.LessonState = ELessonState.Canceled;
        
        await RepositoryDbContext.SaveChangesAsync();
    }

    public async void AddReview(Review review)
    {
        RepositoryDbContext.Reviews.Add(review);
        
        await RepositoryDbContext.SaveChangesAsync();
    }
}