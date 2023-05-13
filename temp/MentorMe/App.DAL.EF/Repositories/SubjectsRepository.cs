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

    public async Task<Subject?> FindAsyncWithDetails(Guid id)
    {
        var x = await RepositoryDbContext.Subjects
            .Include(s => s.TutorSubjects)
            .Include(s => s.StudentSubjects)
            .FirstOrDefaultAsync(e => e.Id == id);
        return x;
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