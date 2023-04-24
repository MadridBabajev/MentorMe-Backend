using Domain.Enums;
using Domain.Identity;
using Helpers;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace App.DAL.EF.Seeding;

public static class AppDataInit
{
    private static readonly Guid TutorId = Guid.NewGuid();
    private static readonly Guid StudentId = Guid.NewGuid();
    private static readonly Guid StudentTutorAccountId = Guid.NewGuid();

    public static void MigrateDatabase(ApplicationDbContext context)
        => context.Database.Migrate();

    public static void DropDatabase(ApplicationDbContext context)
        => context.Database.EnsureDeleted();

    public static void SeedIdentity(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager)
    {
        var tutorData = GetTutorData();
        var studentData = GetStudentData();
        
        // Create the user with the same email, but different persona
        var studentTutorProfile = GetStudentTutorProfile();

        CreateUser(tutorData, userManager);
        CreateUser(studentData, userManager);
        // await CreateUser(studentTutorProfile, userManager);
    }

    private static void CreateUser((Guid Id, string email, string pwd, string mobilePhone, string firstName, string lastName, bool isBlocked, bool notificationsEnabled, EUserType userType) userData, UserManager<AppUser> userManager)
    {
        // TODO Implement better identity seeding 
        // TODO currently it is not possible to create 2 users with the same email
        var user = userManager.FindByEmailAsync(userData.email).Result;
        if (user == null)
        {
            user = new AppUser
            {
                Id = userData.Id,
                Email = userData.email,
                UserName = userData.email,
                FirstName = userData.firstName,
                LastName = userData.lastName,
                MobilePhone = userData.mobilePhone,
                AppUserType = userData.userType,
                NotificationsEnabled = userData.notificationsEnabled,
                IsBlocked = userData.isBlocked,
                EmailConfirmed = true,
            };
            var result = userManager.CreateAsync(user, userData.pwd).Result;
            if (!result.Succeeded)
            {
                throw new ApplicationException($"Cannot seed users, {result}");
            }
        }
        
        // List<AppUser> appUsers = await userManager.Users
        //     .Where(u => u.Email == userData.email).ToListAsync();
        // AppUser? user;
        // IdentityResult result;
        // if (!appUsers.Any())
        // {
        //     user = CreateNewUser(userData);
        //     result = userManager.CreateAsync(user, userData.pwd).Result;
        //     if (!result.Succeeded)
        //     {
        //         throw new ApplicationException($"Cannot seed users, {result}");
        //     }
        //     return;
        // }
        // user = IdentityHelpers.FilterOutUsers(userData.userType == EUserType.Tutor, appUsers);
        //
        // if (user!.AppUserType != userData.userType)
        // {
        //     throw new ApplicationException("Cannot seed users, user with the same type and email already exists");
        // }
        // user = CreateNewUser(userData);
        // result = userManager.CreateAsync(user, userData.pwd).Result;
        // if (!result.Succeeded)
        // {
        //     throw new ApplicationException($"Cannot seed users, {result}");
        // }
        //
    }

    private static AppUser CreateNewUser((Guid Id, string email, string pwd, string mobilePhone, string firstName, string lastName, bool isBlocked, bool notificationsEnabled, EUserType userType) userData)
        => new() 
        {
            Id = userData.Id,
            Email = userData.email,
            UserName = userData.email,
            FirstName = userData.firstName,
            LastName = userData.lastName,
            MobilePhone = userData.mobilePhone,
            AppUserType = userData.userType,
            NotificationsEnabled = userData.notificationsEnabled,
            IsBlocked = userData.isBlocked,
            EmailConfirmed = true
        };

    private static (Guid Id, string email, string pwd, string mobilePhone, string firstName, string lastName, bool isBlocked, bool notificationsEnabled, EUserType userType) GetTutorData()
        => (TutorId, "testTutor@app.com", "Foo.bar.tutor1", "+37253983031", 
            "Tutor", "Test", false, false, EUserType.Tutor);
    
    private static (Guid Id, string email, string pwd, string mobilePhone, string firstName, string lastName, bool isBlocked, bool notificationsEnabled, EUserType userType) GetStudentData()
        => (StudentId, "testStudent@app.com", "Foo.bar.student1", "+37253983032",
            "Student", "Test", false, false, EUserType.Student);

    private static (Guid Id, string email, string pwd, string mobilePhone, string firstName, string lastName, bool isBlocked, bool notificationsEnabled, EUserType userType) GetStudentTutorProfile()
        => (StudentTutorAccountId, "testStudent@app.com", "Foo.bar.student1", "+37253983033",
            "Student2", "Test", false, false, EUserType.Tutor);
    
    // private static AppUser CreateNewUser ()

    public static void SeedAppData(ApplicationDbContext context)
    {
        // SeedAppDataTrainingPlan(context); exp.
        
        context.SaveChanges();
    }

    // TODO seed lessons, dialogs, etc.
    // private static void SeedAppDataTrainingPlan(ApplicationDbContext context)
    // {
    //     if (context.TrainingPlans.Any()) return;
    //     
    //     context.TrainingPlans.Add(new TrainingPlan()
    //         {
    //             PlanName = "Test Plan",
    //             AppUserId = _adminId
    //         }
    //     );
    // }
}
