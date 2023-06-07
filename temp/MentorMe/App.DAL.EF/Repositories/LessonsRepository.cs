
using App.DAL.Contracts;
using Base.DAL.EF;
using Domain.Entities;
using DomainTag = Domain.Entities.Tag;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Public.DTO.v1.Lessons;
using Payment = Domain.Entities.Payment;

namespace App.DAL.EF.Repositories;

public class LessonsRepository: EFBaseRepository<Lesson, ApplicationDbContext>, ILessonsRepository
{
    public LessonsRepository(ApplicationDbContext dataContext) : base(dataContext)
    {
    }
    
    public async Task<Lesson> FindLessonById(Guid lessonId)
    {
        var lesson = await RepositoryDbSet
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
        
        UpdateLessonState(lesson);
        await RepositoryDbContext.SaveChangesAsync();

        return lesson;
    }
    
    public async Task<IEnumerable<Lesson>> GetLessonsList(Guid userId)
    {
        var user = await RepositoryDbContext.AppUsers.FirstAsync(u => u.Id == userId);
        List<Lesson> lessons;
        if (user.AppUserType == EUserType.Tutor)
        {
            // If user is a Tutor, fetch lessons directly from Tutor's Lessons
            lessons = await RepositoryDbContext.Lessons
                .Include(l => l.Tutor)
                    .ThenInclude(t => t!.AppUser)
                .Include(l => l.Subject)
                .Include(l => l.LessonParticipations)!
                    .ThenInclude(lp => lp.Student)
                        .ThenInclude(s => s!.AppUser)
                .Where(l => l.Tutor!.AppUserId == userId)
                .ToListAsync();
        }
        else
        {
            lessons = await RepositoryDbContext.Lessons
                .Include(l => l.Tutor)
                    .ThenInclude(t => t!.AppUser)
                .Include(l => l.Subject)
                .Include(l => l.LessonParticipations)!
                    .ThenInclude(lp => lp.Student)
                        .ThenInclude(s => s!.AppUser)
                .Where(l => l.LessonParticipations!.First().Student!.AppUserId == userId)
                .ToListAsync();
        }
        
        lessons.ForEach(UpdateLessonState);
        await RepositoryDbContext.SaveChangesAsync();
        
        return lessons;
    }
    
    private void UpdateLessonState(Lesson lesson)
    {
        // If the lesson has already ended or been canceled, we don't need to do anything.
        if (lesson.LessonState == ELessonState.Finished || lesson.LessonState == ELessonState.Canceled)
        {
            return;
        }

        // If the end time of the lesson is in the past, change the state to Finished.
        if (DateTime.UtcNow > lesson.EndTime 
            && lesson.LessonState == ELessonState.Upcoming)
        {
            lesson.LessonState = ELessonState.Finished;
        }
        // If the lesson state was Pending, change it to Canceled.
        else if (DateTime.UtcNow > lesson.EndTime 
                 && lesson.LessonState == ELessonState.Pending)
        {
            lesson.LessonState = ELessonState.Canceled;
        }
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
            Description = $"Payment for the {lesson.StartTime} lesson",
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

    public async Task AddTag(DomainTag tag)
    {
        RepositoryDbContext.Tags.Add(tag);
        await RepositoryDbContext.SaveChangesAsync();
    }
    
    public async Task DeleteTag(Guid tagId)
    {
        var tag = await RepositoryDbContext.Tags.FirstAsync(t => t.Id == tagId);
        RepositoryDbContext.Tags.Remove(tag);

        await RepositoryDbContext.SaveChangesAsync();
    }

    public async Task CancelLesson(Guid lessonId)
    {
        var lesson = await RepositoryDbSet
            .Include(l => l.Payments)!
                .ThenInclude(lp => lp.Payment)
            .FirstAsync(l => l.Id == lessonId);
        lesson.IsCanceled = true;
        lesson.LessonState = ELessonState.Canceled;

        var payment = lesson.Payments!.First().Payment;

        payment!.PaymentStatus = EPaymentStatus.Refunded;
        
        await RepositoryDbContext.SaveChangesAsync();

    }

    public async Task AcceptLesson(Guid lessonId)
    {
        var lesson = await RepositoryDbSet.FirstAsync(l => l.Id == lessonId);
        lesson.LessonState = ELessonState.Upcoming;
        
        await RepositoryDbContext.SaveChangesAsync();
    }

    public async Task DeclineLesson(Guid lessonId)
    {
        var lesson = await RepositoryDbSet.FirstAsync(l => l.Id == lessonId);
        lesson.LessonState = ELessonState.Canceled;
        
        await RepositoryDbContext.SaveChangesAsync();
    }

    public async Task AddReview(Review review)
    {
        RepositoryDbContext.Reviews.Add(review);
        
        await RepositoryDbContext.SaveChangesAsync();
    }

    public async Task<Payment> GetLessonPayment(Guid paymentId)
    {
        return await RepositoryDbContext.Payments
            .Include(p => p.StudentPaymentMethod)
            .Include(p => p.LessonPayments)!
                .ThenInclude(lp => lp.Student)
                    .ThenInclude(s => s!.AppUser)
            .Include(p => p.LessonPayments)!
                .ThenInclude(lp => lp.Tutor)
                    .ThenInclude(t => t!.AppUser)
            .FirstAsync(p => p.Id == paymentId);
    }
}