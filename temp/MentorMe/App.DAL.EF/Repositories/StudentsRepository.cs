using App.DAL.Contracts;
using Base.DAL.EF;
using Domain;
using Domain.Entities;
using Domain.Enums;
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
    
    public async Task<List<StudentPaymentMethod>> GetStudentPaymentMethods(Guid userId)
    {
        var student = await RepositoryDbSet
            .Include(s => s.PaymentMethods)
            .FirstOrDefaultAsync(t => t.AppUserId == userId);

        if(student == null)
            throw new Exception($"Tutor with id {userId} not found.");

        return student.PaymentMethods!.ToList();
    }
    
    public async Task<bool> UserIsStudent(Guid userId)
    {
        var user = await RepositoryDbSet.FirstOrDefaultAsync(u => u.AppUserId == userId);
        return user == null;
    }
}