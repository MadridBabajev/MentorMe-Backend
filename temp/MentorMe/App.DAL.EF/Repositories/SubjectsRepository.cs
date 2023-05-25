using App.DAL.Contracts;
using Base.DAL.EF;
using BLL.DTO;
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

    public async Task<Subject?> FindAsyncWithDetails(Guid id)
    {
        var x = await RepositoryDbContext.Subjects
            .Include(s => s.TutorSubjects)
            .Include(s => s.StudentSubjects)
            .FirstOrDefaultAsync(e => e.Id == id);
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
    
    public async Task<IEnumerable<BLLSubjectListElement>> AllSubjectsAsync()
    {
        return await RepositoryDbContext.Subjects
            .Select(s => new BLLSubjectListElement
            {
                Id = s.Id,
                Name = s.Name,
                SubjectPicture = s.SubjectPicture
            })
            .ToListAsync();
    }
}