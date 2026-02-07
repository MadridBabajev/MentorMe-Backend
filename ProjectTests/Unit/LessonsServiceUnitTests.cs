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
using Public.DTO.v1.Lessons;
using Payment = Domain.Entities.Payment;

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
        // Success
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

    [Fact] // Success
    public async Task GetLessonsList_ShouldReturnLessonList_WhenCalledWithValidUserId()
    {
        // Arrange
        var userId = Guid.NewGuid();

        var lessons = new List<Lesson>
        {
            new() { Id = Guid.NewGuid() },
            new() { Id = Guid.NewGuid() }
        };

        var bllLessonListElements = lessons.Select(l => new BLLLessonListElement { Id = l.Id }).ToList();

        _uowMock.Setup(u => u.LessonsRepository.GetLessonsList(userId)).ReturnsAsync(lessons);

        _listMapperMock.Setup(m => m.Map(It.IsAny<Lesson>())).Returns<Lesson>(l => bllLessonListElements.First(b => b.Id == l.Id));

        // Act
        var result = await _lessonsService.GetLessonsList(userId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(bllLessonListElements, result);
    }
    
    [Fact]
    public async Task CreateLesson_ShouldReturnLessonId_WhenGivenValidReserveLessonRequestAndStudentId()
    {
        // Arrange
        var studentId = Guid.NewGuid();
        var reserveLessonRequest = new ReserveLessonRequest
        {
            TutorId = Guid.NewGuid(),
            PaymentMethodId = Guid.NewGuid(),
            SubjectId = Guid.NewGuid(),
            LessonStartTime = DateTime.UtcNow.AddDays(1),
            LessonEndTime = DateTime.UtcNow.AddDays(1).AddHours(1)
        };

        var expectedLessonId = Guid.NewGuid();

        _uowMock.Setup(u => u.LessonsRepository.CreateLesson(It.Is<ReserveLessonRequest>(req => req == reserveLessonRequest), It.Is<Guid>(id => id == studentId)))
            .ReturnsAsync(expectedLessonId);

        // Act
        var result = await _lessonsService.CreateLesson(reserveLessonRequest, studentId);

        // Assert
        Assert.Equal(expectedLessonId, result);
    }
    
    [Fact]
    public async Task CancelLesson_ShouldCallRepository_WhenGivenValidLessonId()
    {
        // Arrange
        var lessonId = Guid.NewGuid();

        _uowMock.Setup(u => u.LessonsRepository.CancelLesson(It.Is<Guid>(id => id == lessonId))).Returns(Task.CompletedTask);

        // Act
        await _lessonsService.CancelLesson(lessonId);

        // Assert
        _uowMock.Verify(u => u.LessonsRepository.CancelLesson(It.Is<Guid>(id => id == lessonId)), Times.Once);
    }

    [Theory]
    [InlineData(ETutorDecision.Accept)]
    [InlineData(ETutorDecision.Decline)]
    public async Task AcceptDeclineLesson_ShouldCallRepository_WhenGivenValidLessonIdAndDecision(ETutorDecision decision)
    {
        // Arrange
        var lessonId = Guid.NewGuid();

        _uowMock.Setup(u => u.LessonsRepository.AcceptLesson(It.Is<Guid>(id => id == lessonId))).Returns(Task.CompletedTask);
        _uowMock.Setup(u => u.LessonsRepository.DeclineLesson(It.Is<Guid>(id => id == lessonId))).Returns(Task.CompletedTask);

        // Act
        await _lessonsService.AcceptDeclineLesson(lessonId, decision);

        // Assert
        if (decision == ETutorDecision.Accept)
        {
            _uowMock.Verify(u => u.LessonsRepository.AcceptLesson(It.Is<Guid>(id => id == lessonId)), Times.Once);
            _uowMock.Verify(u => u.LessonsRepository.DeclineLesson(It.IsAny<Guid>()), Times.Never);
        }
        else
        {
            _uowMock.Verify(u => u.LessonsRepository.DeclineLesson(It.Is<Guid>(id => id == lessonId)), Times.Once);
            _uowMock.Verify(u => u.LessonsRepository.AcceptLesson(It.IsAny<Guid>()), Times.Never);
        }
    }
    
}
