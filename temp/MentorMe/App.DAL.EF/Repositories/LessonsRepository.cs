
using App.DAL.Contracts;
using Base.DAL.EF;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace App.DAL.EF.Repositories;

public class LessonsRepository: EFBaseRepository<Lesson, ApplicationDbContext>, ILessonsRepository
{
    public LessonsRepository(ApplicationDbContext dataContext) : base(dataContext)
    {
    }
    
    public Task<List<Lesson>> AllAsync(Guid getUserId)
    {
        // TODO Make an actual implementation
        return RepositoryDbContext.Lessons.ToListAsync();
    }

    public Task<Lesson?> FindAsync(Guid id, Guid userId)
    {
        throw new NotImplementedException();
    }
}