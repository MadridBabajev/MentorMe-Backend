using App.BLL.Services;
using App.DAL.Contracts;
using App.DAL.EF;
using App.DAL.EF.Seeding;
using Base.Mapper.Contracts;
using BLL.DTO.Lessons;
using BLL.DTO.Profiles;
using BLL.DTO.Subjects;
using Domain.Entities;
using Domain.Enums;
using Domain.Identity;
using Microsoft.AspNetCore.Identity;
using Moq;

namespace ProjectTests.Unit;

public class LessonsServiceUnitTests
{
    private Mock<IAppUOW> _uowMock;
    private Mock<IMapper<BLLLessonData, Lesson>> _dataMapperMock;
    private Mock<IMapper<BLLLessonListElement, Lesson>> _listMapperMock;
    private Mock<IMapper<BLLStudentPaymentMethod, StudentPaymentMethod>> _paymentMethodMapperMock;
    private Mock<IMapper<BLLAvailability, TutorAvailability>> _availabilityMapperMock;
    private Mock<IMapper<BLLSubjectsFilterElement, Subject>> _subjectsFilterMapperMock;
    private Mock<IMapper<BLLPayment, Payment>> _paymentMapperMock;
    private Mock<UserManager<AppUser>> _userManagerMock;
    private Mock<ApplicationDbContext> _ctxMock;
    private DataGuids dataGuids;

    private LessonsService _lessonsService;

    public LessonsServiceUnitTests()
    {
        _uowMock = new Mock<IAppUOW>();
        _dataMapperMock = new Mock<IMapper<BLLLessonData, Lesson>>();
        _listMapperMock = new Mock<IMapper<BLLLessonListElement, Lesson>>();
        _paymentMethodMapperMock = new Mock<IMapper<BLLStudentPaymentMethod, StudentPaymentMethod>>();
        _availabilityMapperMock = new Mock<IMapper<BLLAvailability, TutorAvailability>>();
        _subjectsFilterMapperMock = new Mock<IMapper<BLLSubjectsFilterElement, Subject>>();
        _paymentMapperMock = new Mock<IMapper<BLLPayment, Payment>>();

        _lessonsService = new LessonsService(_uowMock.Object, _dataMapperMock.Object, _listMapperMock.Object,
            _paymentMethodMapperMock.Object, _availabilityMapperMock.Object,
            _subjectsFilterMapperMock.Object, _paymentMapperMock.Object);
        dataGuids = new DataGuids();
    }

    [Fact]
    public async Task GetLessonData_ShouldReturnLessonData_WhenGivenValidLessonId()
    {
        // Arrange
        var lessonId = dataGuids.Lesson1Id;
        var userId = Guid.NewGuid();

        var lesson = new Lesson
        {
            Id = lessonId,
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
        BLLSubjectListElement bllSubjectData = new BLLSubjectListElement();
        BLLProfileCardData bllTutor = new BLLProfileCardData();
        BLLProfileCardData bllStudent = new BLLProfileCardData();

        var studentPaymentMethod = new BLLStudentPaymentMethod
        {
            PaymentMethodType = EPaymentMethod.InApp,
        };
        var bllLessonData = new BLLLessonData
        {
            PaymentId = Guid.NewGuid(),
            StartTime = DateTime.UtcNow.AddDays(1),
            EndTime = DateTime.UtcNow.AddDays(1).AddHours(1),
            LessonState = ELessonState.Upcoming,
            Price = 15,
            ViewedByTutor = false,
            UserCanWriteReview = false,
            StudentPaymentMethod = studentPaymentMethod,
            Subject = bllSubjectData,
            LessonStudent = bllStudent,
            LessonTutor = bllTutor,
            Tags = new List<BLLTag>(),
        };

        _uowMock.Setup(u => u.LessonsRepository.FindLessonById(It.Is<Guid>(id => id == lessonId))).ReturnsAsync(lesson);
        _dataMapperMock.Setup(m => m.Map(It.Is<Lesson>(l => l == lesson))).Returns(bllLessonData);
        _uowMock.Setup(u => u.StudentsRepository.UserIsStudent(It.Is<Guid>(id => id == userId))).ReturnsAsync(false);

        // Act
        var result = await _lessonsService.GetLessonData(userId, lessonId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(bllLessonData, result);
    }

    [Fact]
    public async Task GetLessonsList_ShouldReturnLessonList_WhenCalledWithValidUserId()
    {
        // Arrange
        var userId = Guid.NewGuid();

        var lessons = new List<Lesson>
        {
            new() { Id = dataGuids.Lesson1Id },
            new() { Id = dataGuids.Lesson2Id }
        };

        var bllLessonListElements = lessons.Select(l => new BLLLessonListElement { Id = l.Id });

        _uowMock.Setup(u => u.LessonsRepository.GetLessonsList(It.Is<Guid>(id => id == userId))).ReturnsAsync(lessons);
        _listMapperMock.Setup(m => m.Map(It.IsAny<Lesson>()))
            .Returns((Lesson l) => bllLessonListElements.First(b => b.Id == l.Id));

        // Act
        var result = await _lessonsService.GetLessonsList(userId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(bllLessonListElements, result);
    }

// Test method for GetReserveLessonData method
    [Fact]
    public async Task GetReserveLessonData_ShouldReturnReserveLessonData_WhenCalledWithValidStudentIdAndTutorId()
    {
        // Arrange
        var studentId = Guid.NewGuid();
        var tutorId = Guid.NewGuid();

        var studentPaymentMethods = new List<StudentPaymentMethod>
        {
            new StudentPaymentMethod { PaymentMethodType = EPaymentMethod.InApp },
            new StudentPaymentMethod { PaymentMethodType = EPaymentMethod.Other }
        };

        var tutorAvailabilities = new List<TutorAvailability>
        {
            new TutorAvailability { DayOfTheWeek = EAvailabilityDayOfTheWeek.Monday },
            new TutorAvailability { DayOfTheWeek = EAvailabilityDayOfTheWeek.Tuesday }
        };

        var tutorSubjects = new List<Subject>
        {
            new Subject { Id = dataGuids.SubjectMathId },
            new Subject { Id = dataGuids.SubjectPhysicsId }
        };

        var bllReserveLessonData = new BLLReserveLessonData
        {
            PaymentMethods = studentPaymentMethods.Select(s => _paymentMethodMapperMock.Object.Map(s)),
            Availabilities = tutorAvailabilities.Select(t => _availabilityMapperMock.Object.Map(t)),
            Subjects = tutorSubjects.Select(t => _subjectsFilterMapperMock.Object.Map(t))
        };

        _uowMock.Setup(u => u.StudentsRepository.GetStudentPaymentMethods(It.Is<Guid>(id => id == studentId)))
            .ReturnsAsync(studentPaymentMethods);
        _uowMock.Setup(u => u.TutorsRepository.GetTutorAvailabilities(It.Is<Guid?>(id => id == tutorId)))
            .ReturnsAsync(tutorAvailabilities);
        _uowMock.Setup(u => u.SubjectsRepository.GetUserSubjects(It.Is<Guid?>(id => id == tutorId)))
            .ReturnsAsync(tutorSubjects);

        // Act
        var result = await _lessonsService.GetReserveLessonData(studentId, tutorId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(bllReserveLessonData, result);
    }
}