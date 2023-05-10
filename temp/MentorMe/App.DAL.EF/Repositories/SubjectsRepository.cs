using App.DAL.Contracts;
using Base.DAL.EF;
using BLL.DTO;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace App.DAL.EF.Repositories;

public class SubjectsRepository: 
    EFBaseRepository<Subject, ApplicationDbContext>, ISubjectsRepository
{
    public SubjectsRepository(ApplicationDbContext dataContext) : base(dataContext)
    {
    }

    public async Task<BLLSubjectDetails?> FindAsyncWithDetails(Guid id)
    {
        return await RepositoryDbContext.Subjects
            .Select(s => new BLLSubjectDetails
            {
                Id = s.Id,
                Name = s.Name,
                Description = s.Description,
                TaughtBy = s.TutorSubjects!.Count,
                LearnedBy = s.StudentSubjects!.Count,
                SubjectPicture = s.SubjectPicture
            })
            .FirstOrDefaultAsync(e => e.Id == id);
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