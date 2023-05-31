using Domain.Entities;
using Domain.Enums;
using Domain.Identity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace App.DAL.EF.Seeding;

/// <summary>
/// Seeds all entities:
/// * 1 - student
/// * 6 - tutors
/// * 5 - subjects
/// * 2 - student subjects
/// * 2 - 4 tutor subjects
/// * 1 - student payment methods
/// * 2 - tutor banking details
/// * 4 - tutor availabilities
/// * 3 - reviews
/// * 2 - lessons
/// * 2 - lesson participation
/// * 1 - cancellation
/// * 1 - tag
/// </summary>

public static class AppDataInit
{
    private static readonly string DescriptionFiller =
        "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Curabitur ornare mauris eu magna mollis, quis blandit ex sagittis. Pellentesque pulvinar, ligula feugiat finibus venenatis, lectus erat hendrerit dui, et malesuada nunc orci mollis ipsum. Donec egestas justo sit amet iaculis dapibus. Etiam arcu massa, hendrerit eget sapien fermentum, eleifend suscipit dolor. Duis egestas nulla lobortis lectus blandit fringilla. Maecenas felis mi, accumsan id felis eu, tincidunt gravida lorem. In non pretium lorem, in imperdiet elit. Phasellus nisl felis, cursus at ultrices id, mattis nec odio. Integer nisi tellus, venenatis non orci at, laoreet porta neque.";

    public static void MigrateDatabase(ApplicationDbContext context)
        => context.Database.Migrate();

    public static void DropDatabase(ApplicationDbContext context)
        => context.Database.EnsureDeleted();

    public static void SeedIdentity(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager, ApplicationDbContext ctx, DataGuids guids)
    {
        var studentData = GetStudentData(guids);
        var tutor1Data = GetTutor1Data(guids);
        var tutor2Data = GetTutor2Data(guids);
        var tutor3Data = GetTutor3Data(guids);
        var tutor4Data = GetTutor4Data(guids);
        var tutor5Data = GetTutor5Data(guids);

        // Create the user with the same email, but different persona
        
        // TODO Tutor with the same email as a student:
            // var studentTutorProfile = GetStudentTutorProfile(guids);

        CreateUser(studentData, userManager, ctx);
        CreateUser(tutor1Data, userManager, ctx);
        CreateUser(tutor2Data, userManager, ctx);
        CreateUser(tutor3Data, userManager, ctx);
        CreateUser(tutor4Data, userManager, ctx);
        CreateUser(tutor5Data, userManager, ctx);
        // CreateUser(studentTutorProfile, userManager);
    }

    private static void CreateUser(
        (Guid Id, string email, string pwd, string mobilePhone, string firstName, string lastName, bool isBlocked, bool
            notificationsEnabled, EUserType userType, ECountry country, string title, string bio) userData,
        UserManager<AppUser> userManager, ApplicationDbContext ctx)
    {
        // TODO Implement better identity seeding 
        // TODO currently it is not possible to create 2 users with the same email
        AppUser? user = userManager.FindByEmailAsync(userData.email).Result;
        if (user == null)
        {
            user = CreateNewUser(userData);
            var result = userManager.CreateAsync(user, userData.pwd).Result;
            if (!result.Succeeded)
            {
                throw new ApplicationException($"Cannot seed users, {result}");
            }
            if (user.AppUserType == EUserType.Student)
            {
                ctx.Students.Add(new Student
                {
                    AppUserId = user.Id,
                    Id = user.Id
                });
            }
            else
            {
                ctx.Tutors.Add(new Tutor
                {
                    AppUserId = user.Id,
                    Id = user.Id
                });
            }
            ctx.SaveChanges();
        }
    }

    private static AppUser CreateNewUser((Guid id, string email, string pwd, string mobilePhone, string firstName, string lastName, bool isBlocked, bool notificationsEnabled, EUserType userType, ECountry country, string title, string bio) userData)
        => new() 
        {
            Id = userData.id,
            Email = userData.email,
            UserName = $"{userData.email}_{userData.userType.ToString()}",
            FirstName = userData.firstName,
            LastName = userData.lastName,
            MobilePhone = userData.mobilePhone,
            AppUserType = userData.userType,
            NotificationsEnabled = userData.notificationsEnabled,
            IsBlocked = userData.isBlocked,
            EmailConfirmed = true,
            Country = userData.country,
            Title = userData.title,
            Bio = userData.bio
        };

    private static (Guid Id, string email, string pwd, string mobilePhone, string firstName, string lastName, bool
        isBlocked, bool notificationsEnabled, EUserType userType, ECountry country, string title, string bio)
        GetStudentData(DataGuids dataGuids)
        => (dataGuids.StudentId, "testStudent@app.com", "Foo.bar.student1", "53983030",
            "Student", "Test", false, false, EUserType.Student, ECountry.Estonia, "Title text", DescriptionFiller);
    
    private static (Guid Id, string email, string pwd, string mobilePhone, string firstName, string lastName, bool
        isBlocked, bool notificationsEnabled, EUserType userType, ECountry country, string title, string bio)
        GetTutor1Data(DataGuids dataGuids)
        => (dataGuids.Tutor1Id, "test1Tutor@app.com", "Foo.bar.tutor1", "53983031", 
            "Tutor1", "Test", false, false, EUserType.Tutor, ECountry.Estonia, "Title text", DescriptionFiller);
    
    private static (Guid Id, string email, string pwd, string mobilePhone, string firstName, string lastName, bool
        isBlocked, bool notificationsEnabled, EUserType userType, ECountry country, string title, string bio)
        GetTutor2Data(DataGuids dataGuids)
        => (dataGuids.Tutor2Id, "test2Tutor@app.com", "Foo.bar.tutor2", "53983032", 
            "Tutor2", "Test", false, false, EUserType.Tutor, ECountry.Estonia, "Title text", DescriptionFiller);
    
    private static (Guid Id, string email, string pwd, string mobilePhone, string firstName, string lastName, bool
        isBlocked, bool notificationsEnabled, EUserType userType, ECountry country, string title, string bio)
        GetTutor3Data(DataGuids dataGuids)
        => (dataGuids.Tutor3Id, "test3Tutor@app.com", "Foo.bar.tutor3", "53983033", 
            "Tutor3", "Test", false, false, EUserType.Tutor, ECountry.Estonia, "Title text", DescriptionFiller);

    private static (Guid Id, string email, string pwd, string mobilePhone, string firstName, string lastName, bool
        isBlocked, bool notificationsEnabled, EUserType userType, ECountry country, string title, string bio)
        GetTutor4Data(DataGuids dataGuids)
        => (dataGuids.Tutor4Id, "test4Tutor@app.com", "Foo.bar.tutor4", "53983034", 
            "Tutor4", "Test", false, false, EUserType.Tutor, ECountry.Estonia, "Title text", DescriptionFiller);

    private static (Guid Id, string email, string pwd, string mobilePhone, string firstName, string lastName, bool
        isBlocked, bool notificationsEnabled, EUserType userType, ECountry country, string title, string bio)
        GetTutor5Data(DataGuids dataGuids)
        => (dataGuids.Tutor5Id, "test5Tutor@app.com", "Foo.bar.tutor5", "53983035", 
            "Tutor5", "Test", true, false, EUserType.Tutor, ECountry.Latvia, "Title text", DescriptionFiller);
    
    private static (Guid Id, string email, string pwd, string mobilePhone, string firstName, string lastName, bool
        isBlocked, bool notificationsEnabled, EUserType userType, ECountry country, string title, string bio) 
        GetStudentTutorProfile(DataGuids dataGuids)
        => (dataGuids.StudentTutorAccountId, "testStudent@app.com", "Foo.bar.student1", "53983030",
            "StudentTutor", "Test", false, false, EUserType.Tutor, ECountry.Estonia, "Title text", DescriptionFiller);

    // TODO seed lessons, dialogs, etc.
    public static void SeedAppData(ApplicationDbContext context, IWebHostEnvironment env, DataGuids guids)
    {
        SeedSubjects(context, env, guids);
        SeedStudentTutorSubjects(context, guids);
        SeedStudentPaymentMethods(context, guids);
        SeedTutorBankingDetails(context, guids);
        SeedTutorAvailabilities(context, guids);
        SeedLessonsTagsAndParticipations(context, guids);
        SeedReviews(context, guids);
        SeedPayments(context, guids);
        context.SaveChanges();
    }

    private static void SeedSubjects(ApplicationDbContext context, IWebHostEnvironment env, DataGuids guids)
    {
        if (context.Subjects.Any()) return;

        string basePath = Path.Combine(env.WebRootPath, "imgs");
        
        context.Subjects.AddRange(new Subject
        {
            Id = guids.SubjectMathId,
            Name = "Maths",
            Description = DescriptionFiller,
            SubjectPicture = LoadImageAsBytes(Path.Combine(basePath, "maths_img.jpg"))
        }, new Subject
        {
            Id = guids.SubjectPhysicsId,
            Name = "Physics",
            Description = DescriptionFiller,
            SubjectPicture = LoadImageAsBytes(Path.Combine(basePath, "physics_img.jpg"))
        }, new Subject
        {
            Id = guids.SubjectEnglishId,
            Name = "English",
            Description = DescriptionFiller,
            SubjectPicture = LoadImageAsBytes(Path.Combine(basePath, "english_img.jpg"))
        }, new Subject
        {
            Id = guids.SubjectFrontEndId,
            Name = "Frontend Programming",
            Description = DescriptionFiller,
            SubjectPicture = LoadImageAsBytes(Path.Combine(basePath, "frontend_img.jpg"))
        }, new Subject
        {
            Id = guids.SubjectBackendId,
            Name = "Backend Programming",
            Description = DescriptionFiller,
            SubjectPicture = LoadImageAsBytes(Path.Combine(basePath, "backend_img.jpg"))
        });
        context.SaveChanges();
    }
    
    private static void SeedStudentTutorSubjects(ApplicationDbContext context, DataGuids guids)
    {
        if (context.StudentSubjects.Any() || context.TutorSubjects.Any()) return;

        // Get the subjects from the database
        var mathsSubject = context.Subjects.FirstOrDefault(s => s.Id == guids.SubjectMathId);
        var physicsSubject = context.Subjects.FirstOrDefault(s => s.Id == guids.SubjectPhysicsId);
        var englishSubject = context.Subjects.FirstOrDefault(s => s.Id == guids.SubjectEnglishId);
        var frontendProgrammingSubject = context.Subjects.FirstOrDefault(s => s.Id == guids.SubjectFrontEndId);
        var backendProgrammingSubject = context.Subjects.FirstOrDefault(s => s.Id == guids.SubjectBackendId);

        if (mathsSubject == null || physicsSubject == null || englishSubject == null || frontendProgrammingSubject == null || backendProgrammingSubject == null) return;

        // Seed StudentSubject data
        context.StudentSubjects.AddRange(new StudentSubject
            {
                StudentId = guids.StudentId,
                SubjectId = mathsSubject.Id
            },
            new StudentSubject
            {
                StudentId = guids.StudentId,
                SubjectId = englishSubject.Id
            });

        // Seed TutorSubject data
        context.TutorSubjects.AddRange(new TutorSubject
            {
                TutorId = guids.Tutor1Id,
                SubjectId = mathsSubject.Id
            },
            new TutorSubject
            {
                TutorId = guids.Tutor1Id,
                SubjectId = physicsSubject.Id
            },
            new TutorSubject
            {
                TutorId = guids.Tutor2Id,
                SubjectId = frontendProgrammingSubject.Id
            },
            new TutorSubject
            {
                TutorId = guids.Tutor2Id,
                SubjectId = backendProgrammingSubject.Id
            });
        context.SaveChanges();
    }
    
    private static void SeedStudentPaymentMethods(ApplicationDbContext context, DataGuids dataGuids)
    {
        if (context.StudentPaymentMethods.Any()) return;

        context.StudentPaymentMethods.AddRange(
            new StudentPaymentMethod
            {
                Id = dataGuids.StudentPaymentMethod1Id,
                StudentId = dataGuids.StudentId,
                PaymentMethodType = EPaymentMethod.InApp,
                Details = "In-app payment details",
                CardCvv = "123",
                CardExpirationDate = "11/25",
                CardNumber = "5555 5555 5555 4444",
                IsBlocked = false
            },
            new StudentPaymentMethod
            {
                Id = dataGuids.StudentPaymentMethod2Id,
                StudentId = dataGuids.StudentId,
                PaymentMethodType = EPaymentMethod.Cash,
                Details = "Cash payment",
                CardCvv = "N/A",
                CardExpirationDate = "N/A",
                CardNumber = "N/A",
                IsBlocked = false
            });
    }
    
    private static void SeedTutorBankingDetails(ApplicationDbContext context, DataGuids dataGuids)
    {
        if (context.TutorBankingDetails.Any()) return;

        context.TutorBankingDetails.AddRange(
            new TutorBankingDetails
            {
                TutorId = dataGuids.Tutor1Id,
                BankAccountNumber = "1111222233334444",
                BankAccountName = "Tutor1 Test",
                IsValidated = true,
                BankAccountType = EBankAccountType.Personal
            },
            new TutorBankingDetails
            {
                TutorId = dataGuids.Tutor2Id,
                BankAccountNumber = "2222333344445555",
                BankAccountName = "Tutor2 Test",
                IsValidated = true,
                BankAccountType = EBankAccountType.Business
            });
    }
    
    private static void SeedTutorAvailabilities(ApplicationDbContext context, DataGuids dataGuids)
    {
        if (context.TutorAvailabilities.Any()) return;

        context.TutorAvailabilities.AddRange(
            new TutorAvailability
            {
                TutorId = dataGuids.Tutor1Id,
                FromHours = TimeSpan.FromHours(9),
                ToHours = TimeSpan.FromHours(12),
                DayOfTheWeek = EAvailabilityDayOfTheWeek.Monday
            },
            new TutorAvailability
            {
                TutorId = dataGuids.Tutor1Id,
                FromHours = TimeSpan.FromHours(14),
                ToHours = TimeSpan.FromHours(18),
                DayOfTheWeek = EAvailabilityDayOfTheWeek.Wednesday
            },
            new TutorAvailability
            {
                TutorId = dataGuids.Tutor2Id,
                FromHours = TimeSpan.FromHours(10),
                ToHours = TimeSpan.FromHours(15),
                DayOfTheWeek = EAvailabilityDayOfTheWeek.Tuesday
            },
            new TutorAvailability
            {
                TutorId = dataGuids.Tutor2Id,
                FromHours = TimeSpan.FromHours(16),
                ToHours = TimeSpan.FromHours(20),
                DayOfTheWeek = EAvailabilityDayOfTheWeek.Thursday
            },
            new TutorAvailability
            {
                TutorId = dataGuids.Tutor2Id,
                FromHours = TimeSpan.FromHours(13),
                ToHours = TimeSpan.FromHours(15),
                DayOfTheWeek = EAvailabilityDayOfTheWeek.Saturday
            });
    }
    
    private static void SeedLessonsTagsAndParticipations(ApplicationDbContext context, DataGuids dataGuids)
    {
        if (context.Lessons.Any()) return;

        // Seed Lessons
        var lesson1 = new Lesson
        {
            Id = dataGuids.Lesson1Id,
            StartTime = DateTime.UtcNow.AddDays(1),
            EndTime = DateTime.UtcNow.AddDays(1).AddHours(1),
            LessonDuration = 60,
            ParticipantCount = 1,
            IsCanceled = false,
            LessonType = ELessonType.OneOnOne,
            LessonState = ELessonState.Upcoming,
            TutorId = dataGuids.Tutor1Id,
            SubjectId = dataGuids.SubjectMathId
        };
        
        var lesson2 = new Lesson
        {
            Id = dataGuids.Lesson2Id,
            StartTime = DateTime.UtcNow.AddDays(1),
            EndTime = DateTime.UtcNow.AddDays(1).AddHours(1),
            LessonDuration = 60,
            ParticipantCount = 1,
            IsCanceled = false,
            LessonType = ELessonType.OneOnOne,
            LessonState = ELessonState.Pending,
            TutorId = dataGuids.Tutor1Id,
            SubjectId = dataGuids.SubjectMathId
        };
        
        var lesson3 = new Lesson
        {
            Id = dataGuids.Lesson3Id,
            StartTime = DateTime.UtcNow.AddDays(-2),
            EndTime = DateTime.UtcNow.AddDays(-2).AddHours(1),
            LessonDuration = 60,
            ParticipantCount = 1,
            IsCanceled = true,
            LessonType = ELessonType.OneOnOne,
            LessonState = ELessonState.Canceled,
            TutorId = dataGuids.Tutor2Id,
            SubjectId = dataGuids.SubjectEnglishId
        };
    
        var lesson4 = new Lesson
        {
            Id = dataGuids.Lesson4Id,
            StartTime = DateTime.UtcNow.AddDays(2),
            EndTime = DateTime.UtcNow.AddDays(2).AddHours(1),
            LessonDuration = 60,
            ParticipantCount = 1,
            IsCanceled = false,
            LessonType = ELessonType.OneOnOne,
            LessonState = ELessonState.Upcoming,
            TutorId = dataGuids.Tutor2Id,
            SubjectId = dataGuids.SubjectFrontEndId
        };

        var lesson5 = new Lesson
        {
            Id = dataGuids.Lesson5Id,
            StartTime = DateTime.UtcNow.AddDays(-1),
            EndTime = DateTime.UtcNow.AddDays(-1).AddHours(1),
            LessonDuration = 60,
            ParticipantCount = 1,
            IsCanceled = false,
            LessonType = ELessonType.OneOnOne,
            LessonState = ELessonState.Finished,
            TutorId = dataGuids.Tutor2Id,
            SubjectId = dataGuids.SubjectFrontEndId
        };
        
        var lesson6 = new Lesson
        {
            Id = dataGuids.Lesson6Id,
            StartTime = DateTime.UtcNow.AddDays(-1),
            EndTime = DateTime.UtcNow.AddDays(-1).AddHours(1),
            LessonDuration = 60,
            ParticipantCount = 1,
            IsCanceled = false,
            LessonType = ELessonType.OneOnOne,
            LessonState = ELessonState.Finished,
            TutorId = dataGuids.Tutor2Id,
            SubjectId = dataGuids.SubjectBackendId
        };

        context.Lessons.AddRange(lesson1, lesson2, lesson3, lesson4, lesson5, lesson6);
        
        // Seed Lesson Participations
        context.LessonParticipations.AddRange(
            new LessonParticipation
            {
                StudentId = dataGuids.StudentId,
                LessonId = lesson1.Id
            },
            new LessonParticipation
            {
                StudentId = dataGuids.StudentId,
                LessonId = lesson2.Id
            },
            new LessonParticipation
            {
                StudentId = dataGuids.StudentId,
                LessonId = lesson3.Id
            },
            new LessonParticipation
            {
                StudentId = dataGuids.StudentId,
                LessonId = lesson4.Id
            },
            new LessonParticipation
            {
                StudentId = dataGuids.StudentId,
                LessonId = lesson5.Id
            },
            new LessonParticipation
            {
                StudentId = dataGuids.StudentId,
                LessonId = lesson6.Id
            }
        );
        
        // Seed Tag for an upcoming lesson
        context.Tags.Add(
            new Tag
            {
                Name = "Important Topic",
                Description = "We'll cover a crucial concept in this lesson. Please come prepared with questions!",
                TutorId = dataGuids.Tutor1Id,
                LessonId = lesson1.Id
            }
        );
        
        // Seed Cancellations
        context.Cancellations.Add(
            new Cancellation
            {
                LessonId = lesson2.Id,
                Reason = "Sickness",
                Penalty = 3,
                CancellationType = ECancellationType.ByStudent
            });
        context.SaveChanges();
    }
    
    private static void SeedReviews(ApplicationDbContext context, DataGuids dataGuids)
    {
        if (context.Reviews.Any()) return;

        // Find the finished lesson
        var finishedLesson = context.Lessons.FirstOrDefault(l => l.Id == dataGuids.Lesson5Id);

        if (finishedLesson == null) return;

        // Get the student who participated in the lesson
        var studentParticipation = context.LessonParticipations.FirstOrDefault(lp => lp.LessonId == finishedLesson.Id);
        if (studentParticipation == null) return;

        // Seed a 5-star review from the student for the tutor
        context.Reviews.Add(new Review
        {
            LessonId = finishedLesson.Id,
            TutorId = finishedLesson.TutorId,
            StudentId = studentParticipation.StudentId,
            Rating = 5,
            Comment = "Great lesson!",
            ReviewType = EReviewType.ReviewOfTutor
        });

        // Seed a 5-star review from the tutor for the student
        context.Reviews.Add(new Review
        {
            LessonId = finishedLesson.Id,
            TutorId = finishedLesson.TutorId,
            StudentId = studentParticipation.StudentId,
            Rating = 5,
            Comment = "Excellent student!",
            ReviewType = EReviewType.ReviewOfStudent
        });
    }
    
    private static void SeedPayments(ApplicationDbContext context, DataGuids dataGuids)
    {
        if (context.Payments.Any()) return;

        var lessons = context.Lessons.ToList();
        var studentPaymentMethodId = dataGuids.StudentPaymentMethod1Id;

        foreach (var lesson in lessons)
        {
            var payment = new Payment
            {
                Amount = 15,
                Description = $"Payment for the {lesson.StartTime} lesson",
                StudentPaymentMethodId = studentPaymentMethodId,
                PaymentStatus = EPaymentStatus.Reserved 
            };

            context.Payments.Add(payment);

            var lessonPayment = new LessonPayment
            {
                PaymentId = payment.Id,
                LessonId = lesson.Id,
                StudentId = dataGuids.StudentId, 
                TutorId = lesson.TutorId 
            };

            context.LessonPayments.Add(lessonPayment);

            lesson.Payments = new List<LessonPayment>();
            lesson.Payments.Add(lessonPayment);
        }

        context.SaveChanges();
    }
    
    private static byte[] LoadImageAsBytes(string imagePath)
    {
        using var image = File.OpenRead(imagePath);
        using var memoryStream = new MemoryStream();
        image.CopyTo(memoryStream);
        return memoryStream.ToArray();
    }
}
