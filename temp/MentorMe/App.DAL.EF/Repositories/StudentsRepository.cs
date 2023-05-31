using App.DAL.Contracts;
using Base.DAL.EF;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Public.DTO.v1.Profiles;

namespace App.DAL.EF.Repositories;

public class StudentsRepository: 
    EFBaseRepository<Student, ApplicationDbContext>, IStudentsRepository
{
    public StudentsRepository(ApplicationDbContext dataContext) : base(dataContext)
    {
    }

    public async Task<Student> FindStudentById(Guid? userId)
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
        return user != null;
    }

    public async Task<Student> GetStudentEditProfileData(Guid userId) 
        => (await RepositoryDbSet
            .Include(s => s.AppUser)
            .FirstOrDefaultAsync(s => s.AppUserId == userId))!;

    public async Task UpdateStudentProfileData(Guid studentId, UpdatedProfileData updatedProfileData)
    {
        var student = await RepositoryDbSet
            .Include(s => s.AppUser)
            .FirstOrDefaultAsync(s => s.AppUserId == studentId);

        if (student == null)
        {
            throw new Exception("Student not found");
        }
        
        student.AppUser!.FirstName = updatedProfileData.FirstName;
        student.AppUser.LastName = updatedProfileData.LastName;
        student.AppUser.MobilePhone = updatedProfileData.MobilePhone;
        student.AppUser.Title = updatedProfileData.Title;
        student.AppUser.Bio = updatedProfileData.Bio;
        student.AppUser.ProfilePicture = updatedProfileData.ProfilePicture;
        
        RepositoryDbSet.Update(student);
        await RepositoryDbContext.SaveChangesAsync();
    }
}