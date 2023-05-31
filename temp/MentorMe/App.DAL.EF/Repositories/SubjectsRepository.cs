using App.DAL.Contracts;
using Base.DAL.EF;
using BLL.DTO.Subjects;
using Domain.Entities;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace App.DAL.EF.Repositories;

public class SubjectsRepository: 
    EFBaseRepository<Subject, ApplicationDbContext>, ISubjectsRepository
{
    public SubjectsRepository(ApplicationDbContext dataContext) : base(dataContext)
    {
    }

    public async Task<Subject?> FindAsyncWithDetails(Guid subjectId)
    {
        var x = await RepositoryDbContext.Subjects
            .Include(s => s.TutorSubjects)
            .Include(s => s.StudentSubjects)
            .FirstOrDefaultAsync(e => e.Id == subjectId);
        return x;
    }
    
    public async Task<IEnumerable<Subject?>?> GetUserSubjects(Guid? id)
    {
        var user = await RepositoryDbContext.AppUsers
            .Include(u => u.Tutor)
                .ThenInclude(t => t!.TutorSubjects)!
                    .ThenInclude(t => t.Subject)
            .Include(u => u.Student)
                .ThenInclude(s => s!.StudentSubjects)!
                    .ThenInclude(s => s.Subject)
            .FirstOrDefaultAsync(u => u.Id == id);

        if (user == null) return null;

        List<Subject?> subjects = new();

        if (user.AppUserType == EUserType.Tutor && user.Tutor != null)
        {
            subjects = user.Tutor.TutorSubjects!
                .Select(t => t.Subject)
                .ToList();
        }
        else if (user.AppUserType == EUserType.Student && user.Student != null)
        {
            subjects = user.Student.StudentSubjects!
                .Select(s => s.Subject)
                .ToList();
        }

        return subjects;
    }

    public async Task<bool> CheckIfSubjectIsAdded(Subject subject, Guid? userId)
    {
        // Check if user is a tutor and has the subject
        var isTutorAdded = await RepositoryDbContext.TutorSubjects
            .AnyAsync(ts => ts.Tutor!.AppUserId == userId && ts.SubjectId == subject.Id);

        if (isTutorAdded)
        {
            return true;
        }

        // Check if user is a student and has the subject
        var isStudentAdded = await RepositoryDbContext.StudentSubjects
            .AnyAsync(ss => ss.Student!.AppUserId == userId && ss.SubjectId == subject.Id);

        return isStudentAdded;
    }

    public async Task AddStudentSubject(Guid userId, Guid subjectId)
    {
        var student = await RepositoryDbContext.Students.FindAsync(userId);
        if (student == null)
        {
            throw new Exception("Student not found");
        }
    
        var subject = await RepositoryDbSet.FindAsync(subjectId);
        if (subject == null)
        {
            throw new Exception("Subject not found");
        }
    
        var studentSubject = new StudentSubject
        {
            StudentId = userId,
            SubjectId = subjectId
        };
        
        student.StudentSubjects ??= new List<StudentSubject>();
        student.StudentSubjects!.Add(studentSubject);

        await RepositoryDbContext.StudentSubjects.AddAsync(studentSubject);
        await RepositoryDbContext.SaveChangesAsync();
    }

    public async Task RemoveStudentSubject(Guid userId, Guid subjectId)
    {
        var student = await RepositoryDbContext.Students
            .Include(s => s.StudentSubjects)
            .FirstOrDefaultAsync(s => s.AppUserId == userId);
        
        if (student == null)
        {
            throw new Exception("Student not found");
        }

        var studentSubject = student.StudentSubjects?.FirstOrDefault(ss => ss.SubjectId == subjectId);
        if (studentSubject == null)
        {
            throw new Exception("Student's subject not found");
        }

        student.StudentSubjects!.Remove(studentSubject);
        RepositoryDbContext.StudentSubjects.Remove(studentSubject);
        
        await RepositoryDbContext.SaveChangesAsync();
    }

    public async Task AddTutorSubject(Guid userId, Guid subjectId)
    {
        var tutor = await RepositoryDbContext.Tutors.FindAsync(userId);
        if (tutor == null)
        {
            throw new Exception("Tutor not found");
        }

        var subject = await RepositoryDbSet.FindAsync(subjectId);
        if (subject == null)
        {
            throw new Exception("Subject not found");
        }

        var tutorSubject = new TutorSubject
        {
            TutorId = userId,
            SubjectId = subjectId
        };
        
        tutor.TutorSubjects ??= new List<TutorSubject>();
        tutor.TutorSubjects!.Add(tutorSubject); 

        await RepositoryDbContext.TutorSubjects.AddAsync(tutorSubject);
        await RepositoryDbContext.SaveChangesAsync();
    }

    public async Task RemoveTutorSubject(Guid userId, Guid subjectId)
    {
        var tutor = await RepositoryDbContext.Tutors
            .Include(t => t.TutorSubjects)
            .FirstOrDefaultAsync(t => t.AppUserId == userId);
        
        if (tutor == null)
        {
            throw new Exception("Tutor not found");
        }

        var tutorSubject = tutor.TutorSubjects?.FirstOrDefault(ts => ts.SubjectId == subjectId);
        if (tutorSubject == null)
        {
            throw new Exception("Tutor's subject not found");
        }

        tutor.TutorSubjects!.Remove(tutorSubject);
        RepositoryDbContext.TutorSubjects.Remove(tutorSubject);
        
        await RepositoryDbContext.SaveChangesAsync();
    }

    public async Task<IEnumerable<BLLSubjectListElement>> AllSubjectsAsync() 
        => await RepositoryDbContext.Subjects
            .Select(s => new BLLSubjectListElement
            {
                Id = s.Id,
                Name = s.Name,
                SubjectPicture = s.SubjectPicture
            })
            .ToListAsync();
    
}