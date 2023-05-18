using App.DAL.Contracts;
using Base.DAL.EF;
using Domain;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace App.DAL.EF.Repositories;

public class StudentsRepository: 
    EFBaseRepository<Student, ApplicationDbContext>, IStudentsRepository
{
    public StudentsRepository(ApplicationDbContext dataContext) : base(dataContext)
    {
    }

    public async Task<Student> FindStudentById(Guid userId)
    {
        return await RepositoryDbSet
            .Include(s => s.AppUser)
            .Include(s => s.StudentSubjects)!
                .ThenInclude(ss => ss.Subject)
            .Include(s => s.Reviews)
            .Include(s => s.Lessons)
            .FirstAsync(s => s.AppUserId == userId);
    }

}