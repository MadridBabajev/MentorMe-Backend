using App.DAL.EF;
using Domain.Entities;
using Domain.Enums;
using Domain.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Subject = Domain.Entities.Subject;

namespace ProjectTests.Helpers;

public static class SeedTestData
{
    private static UserManager<AppUser>? _userManager;
    private static ApplicationDbContext? _context;

    public static void Initialize(IServiceProvider serviceProvider)
    {
        _userManager = serviceProvider.GetRequiredService<UserManager<AppUser>>();
        _context = serviceProvider.GetRequiredService<ApplicationDbContext>();
        SeedUsers();
        SeedSubjects();
    }

    private static void SeedUsers()
    {
        var testTutor = new AppUser
        {
            Email = "tutor1@test.com",
            UserName = "tutor1@test.com",
            MobilePhone = "1234567891",
            FirstName = "Tutor1",
            LastName = "Test1",
            IsBlocked = false,
            NotificationsEnabled = true,
            AppUserType = EUserType.Tutor,
            EmailConfirmed = true,
            Country = ECountry.Estonia,
            Title = "Test title",
            Bio = "Test bio"
        };
        
        var testStudent = new AppUser
        {
            Email = "student1@test.com",
            UserName = "student1@test.com",
            MobilePhone = "1234567891",
            FirstName = "Student2",
            LastName = "Test2",
            IsBlocked = false,
            NotificationsEnabled = true,
            AppUserType = EUserType.Student,
            EmailConfirmed = true,
            Country = ECountry.Estonia,
            Title = "Test title",
            Bio = "Test bio"
        };

        _userManager!.CreateAsync(testTutor, "Test1234!").Wait();
        _userManager.CreateAsync(testStudent, "Test1234!").Wait();
        
        _context!.Tutors.Add(new Tutor
        {
            AppUserId = testTutor.Id,
            Id = testTutor.Id
        });
        
        _context.Students.Add(new Student
        {
            AppUserId = testStudent.Id,
            Id = testStudent.Id
        });
    }

    private static void SeedSubjects()
    {
        _context!.Subjects.AddRange(new Subject
        {
            Name = "TestSubject1",
            Description = "filler"
        }, new Subject
        {
            Name = "TestSubject2",
            Description = "filler"
        }, new Subject
        {
            Name = "TestSubject3",
            Description = "filler"
        });
        
        _context.SaveChanges();
    }
}
