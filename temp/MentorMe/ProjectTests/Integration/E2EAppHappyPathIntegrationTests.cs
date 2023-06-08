using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using Domain.Enums;
using Microsoft.AspNetCore.Mvc.Testing;
using ProjectTests.Helpers;
using Public.DTO.v1.Identity;
using Public.DTO.v1.Lessons;
using Public.DTO.v1.Profiles.Secondary;
using Public.DTO.v1.Subjects;
using Xunit.Abstractions;

namespace ProjectTests.Integration;

public class E2EAppHappyPathIntegrationTests : IClassFixture<CustomWebAppFactory<Program>>
{
    private readonly HttpClient _client;
    private readonly ITestOutputHelper _testOutputHelper;

    private readonly JsonSerializerOptions _camelCaseJsonSerializerOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };

    public E2EAppHappyPathIntegrationTests(CustomWebAppFactory<Program> factory, ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
        _client = factory.CreateClient(new WebApplicationFactoryClientOptions
        {
            AllowAutoRedirect = false
        });
    }

    [Fact(DisplayName = "End to End Test: Step 1 - Registration and Authorization")]
    public async Task E2E_Step1_RegisterAndAuthorizeUsers()
    {
        // Arrange - Register User 1
        Register registerDataUser1 = new Register
        {
            Email = "test1@test.com",
            MobilePhone = "1234567891",
            Password = "Test1234!",
            Firstname = "Tutor1",
            Lastname = "Test1",
            IsTutor = true,
            ConfirmPassword = "Test1234!",
            Country = "Estonia"
        };
        await RegisterAndAuthorizeUser(registerDataUser1);

        // Arrange - Register User 2
        Register registerDataUser2 = new Register
        {
            Email = "test2@test.com",
            MobilePhone = "1234567892",
            Password = "Test1234!",
            Firstname = "Student2",
            Lastname = "Test2",
            IsTutor = false,
            ConfirmPassword = "Test1234!",
            Country = "Estonia"
        };
        await RegisterAndAuthorizeUser(registerDataUser2);
    }

    [Fact(DisplayName = "End to End Test: Step 2 - Adding and Removing Subjects")]
    public async Task E2E_Step2_AddRemoveSubjects()
    {
        // Arrange - Get subjects
        var subjectListElements = await GetAllSubjects();

        // Arrange - Login User 1
        var loginUser1 = new Login
        {
            Email = "tutor1@test.com",
            Password = "Test1234!",
            IsTutor = true
        };

        await LoginAndAuthorizeUser(loginUser1);
        var subjectDetailsUser1 =
            await GetSubjectDetails(subjectListElements[0].Id); // Fetch subject details for first subject

        // Arrange - Login User 2
        var loginUser2 = new Login
        {
            Email = "student1@test.com",
            Password = "Test1234!",
            IsTutor = false
        };

        await LoginAndAuthorizeUser(loginUser2);
        var subjectDetailsUser2 =
            await GetSubjectDetails(subjectListElements[1].Id); // Fetch subject details for second subject

        // Act - Add subject for User 1 and 2
        await AddRemoveSubject(subjectDetailsUser1.Id, ESubjectAction.AddSubject);
        await AddRemoveSubject(subjectDetailsUser2.Id, ESubjectAction.AddSubject);
        subjectDetailsUser1 = await GetSubjectDetails(subjectDetailsUser1.Id); // Refresh subject details
        subjectDetailsUser2 = await GetSubjectDetails(subjectDetailsUser2.Id); // Refresh subject details

        // Assert - Verify if the subject was added successfully
        Assert.True(subjectDetailsUser1.IsAdded);
        Assert.True(subjectDetailsUser2.IsAdded);

        // Act - Remove subject for User 1 and 2
        await AddRemoveSubject(subjectDetailsUser1.Id, ESubjectAction.RemoveSubject);
        await AddRemoveSubject(subjectDetailsUser2.Id, ESubjectAction.RemoveSubject);
        subjectDetailsUser1 = await GetSubjectDetails(subjectDetailsUser1.Id); // Refresh subject details
        subjectDetailsUser2 = await GetSubjectDetails(subjectDetailsUser2.Id); // Refresh subject details

        // Assert - Verify if the subject was removed successfully
        Assert.False(subjectDetailsUser1.IsAdded);
        Assert.False(subjectDetailsUser2.IsAdded);
    }

    [Fact(DisplayName = "End to End Test: Step 3 - Adding and Removing Payment Methods")]
    public async Task E2E_Step3_AddRemovePaymentMethods()
    {
        // Arrange - Login student
        var loginUser2 = new Login
        {
            Email = "student1@test.com",
            Password = "Test1234!",
            IsTutor = false
        };

        await LoginAndAuthorizeUser(loginUser2);

        // Act - Add Payment Method for User 2
        var newPaymentMethod = new NewPaymentMethod
        {
            CardNumber = "4111111111111111",
            Details = "test data",
            CardCvv = "02/28",
            CardExpirationDate = "test data"
        };

        await AddPaymentMethod(newPaymentMethod);

        // Assert - Verify if the payment method was added successfully
        var paymentMethods = await GetPaymentMethods();
        Assert.True(paymentMethods.Count > 0);

        // Act - Remove Payment Method for User 2
        await RemovePaymentMethod(paymentMethods.First().Id);

        // Assert - Verify if the payment method was removed successfully
        paymentMethods = await GetPaymentMethods();
        Assert.Empty(paymentMethods);
    }

    [Fact(DisplayName = "End to End Test: Step 4 - Adding and Removing Availabilities")]
    public async Task E2E_Step4_AddRemoveAvailabilities()
    {
        // Arrange - Login tutor
        var loginUser1 = new Login
        {
            Email = "tutor1@test.com",
            Password = "Test1234!",
            IsTutor = true
        };

        await LoginAndAuthorizeUser(loginUser1);

        // Act - Add Availability for User 1 (Tutor)
        var newAvailability = new NewAvailability
        {
            DayOfTheWeek = (int)EAvailabilityDayOfTheWeek.Monday,
            FromHours = new TimeSpan(9, 0, 0).ToString("hh\\:mm"),
            ToHours = new TimeSpan(17, 0, 0).ToString("hh\\:mm")
        };

        await AddAvailability(newAvailability);

        // Assert - Verify if the availability was added successfully
        var availabilities = await GetAvailabilities();
        Assert.True(availabilities.Count > 0);

        // Act - Remove Availability for User 1 (Tutor)
        await RemoveAvailability(availabilities.First().Id);

        // Assert - Verify if the availability was removed successfully
        availabilities = await GetAvailabilities();
        Assert.Empty(availabilities);
    }

    [Fact(DisplayName = "End to End Test: Step 5 - Reserving a Lesson")]
    public async Task E2E_Step5_ReserveLesson()
    {
        // Arrange - Login as tutor
        var loginUser1 = new Login
        {
            Email = "tutor1@test.com",
            Password = "Test1234!",
            IsTutor = true
        };
        var tutorId = await LoginAndAuthorizeUser(loginUser1);

        // Act - Add subject for tutor
        var subjectListElements = await GetAllSubjects();
        await AddRemoveSubject(subjectListElements[0].Id, ESubjectAction.AddSubject);

        // Arrange - Add availability for tutor
        var newAvailability = new NewAvailability
        {
            DayOfTheWeek = (int)EAvailabilityDayOfTheWeek.Monday,
            FromHours = new TimeSpan(9, 0, 0).ToString("hh\\:mm"),
            ToHours = new TimeSpan(17, 0, 0).ToString("hh\\:mm")
        };
        await AddAvailability(newAvailability);

        // Arrange - Login as student
        var loginUser2 = new Login
        {
            Email = "student1@test.com",
            Password = "Test1234!",
            IsTutor = false
        };
        await LoginAndAuthorizeUser(loginUser2);

        // Act - Add payment method for student
        var newPaymentMethod = new NewPaymentMethod
        {
            CardNumber = "4111111111111111",
            Details = "test data",
            CardCvv = "02/28",
            CardExpirationDate = "test data"
        };
        await AddPaymentMethod(newPaymentMethod);

        // Arrange - Get payment methods
        var paymentMethods = await GetPaymentMethods();

        // Act - Reserve a lesson
        var reserveLessonRequest = new ReserveLessonRequest
        {
            TutorId = tutorId,
            PaymentMethodId = paymentMethods.First().Id,
            SubjectId = subjectListElements[0].Id,
            LessonStartTime = DateTime.Now.AddDays(1).AddHours(10), // Next day at 10 AM
            LessonEndTime = DateTime.Now.AddDays(1).AddHours(11) // Ends at 11 AM
        };
        var lessonId = await ReserveLesson(reserveLessonRequest);

        // Assert - Verify if the lesson was reserved successfully
        Assert.NotNull(lessonId);
    }

    [Fact(DisplayName = "End to End Test: Step 6 - Accepting and declining a Lesson")]
    public async Task E2E_Step6_AcceptDeclineLesson()
    {
        // Arrange - Login as tutor
        var loginUser1 = new Login
        {
            Email = "tutor1@test.com",
            Password = "Test1234!",
            IsTutor = true
        };
        var tutorId = await LoginAndAuthorizeUser(loginUser1);

        // Arrange - Login as student
        var loginUser2 = new Login
        {
            Email = "student1@test.com",
            Password = "Test1234!",
            IsTutor = false
        };
        await LoginAndAuthorizeUser(loginUser2);
        
        // Arrange - Add Payment Method for User 2
        var newPaymentMethod = new NewPaymentMethod
        {
            CardNumber = "4111111111111111",
            Details = "test data",
            CardCvv = "02/28",
            CardExpirationDate = "test data"
        };

        await AddPaymentMethod(newPaymentMethod);

        // Arrange - Reserve two lessons
        var subjectListElements = await GetAllSubjects();
        var paymentMethods = await GetPaymentMethods();
        var reserveLessonRequest = new ReserveLessonRequest
        {
            TutorId = tutorId,
            PaymentMethodId = paymentMethods.First().Id,
            SubjectId = subjectListElements[0].Id,
            LessonStartTime = DateTime.Now.AddDays(1).AddHours(10), // Next day at 10 AM
            LessonEndTime = DateTime.Now.AddDays(1).AddHours(11) // Ends at 11 AM
        };
        var lessonId1 = await ReserveLesson(reserveLessonRequest);
        var lessonId2 = await ReserveLesson(reserveLessonRequest);

        // Act - Login as tutor again to accept or decline the lessons
        await LoginAndAuthorizeUser(loginUser1);

        // Act - Accept the first lesson
        var acceptDeclineRequest = new AcceptDeclineRequest
        {
            LessonId = lessonId1!.Value,
            TutorDecision = ETutorDecision.Accept
        };
        await AcceptDeclineLesson(acceptDeclineRequest);

        // Assert - Verify if the first lesson was accepted
        var lessonData1 = await GetLessonData(lessonId1.Value);
        Assert.Equal(ELessonState.Upcoming, lessonData1.LessonState);

        // Act - Decline the second lesson
        acceptDeclineRequest.LessonId = lessonId2!.Value;
        acceptDeclineRequest.TutorDecision = ETutorDecision.Decline;
        await AcceptDeclineLesson(acceptDeclineRequest);

        // Assert - Verify if the second lesson was declined
        var lessonData2 = await GetLessonData(lessonId2.Value);
        Assert.Equal(ELessonState.Canceled, lessonData2.LessonState);
    }
    
    [Fact(DisplayName = "End to End Test: Step 7 - Adding and Removing Tags")]
    public async Task E2E_Step7_AddRemoveTags()
    {
        // Arrange - Login as tutor
        var loginUser1 = new Login
        {
            Email = "tutor1@test.com",
            Password = "Test1234!",
            IsTutor = true
        };
        var tutorId = await LoginAndAuthorizeUser(loginUser1);

        // Arrange - Login as student
        var loginUser2 = new Login
        {
            Email = "student1@test.com",
            Password = "Test1234!",
            IsTutor = false
        };
        await LoginAndAuthorizeUser(loginUser2);
        
        // Arrange - Add Payment Method for User 2
        var newPaymentMethod = new NewPaymentMethod
        {
            CardNumber = "4111111111111111",
            Details = "test data",
            CardCvv = "02/28",
            CardExpirationDate = "test data"
        };

        await AddPaymentMethod(newPaymentMethod);

        // Arrange - Reserve two lessons
        var subjectListElements = await GetAllSubjects();
        var paymentMethods = await GetPaymentMethods();
        var reserveLessonRequest = new ReserveLessonRequest
        {
            TutorId = tutorId,
            PaymentMethodId = paymentMethods.First().Id,
            SubjectId = subjectListElements[0].Id,
            LessonStartTime = DateTime.Now.AddDays(1).AddHours(10), // Next day at 10 AM
            LessonEndTime = DateTime.Now.AddDays(1).AddHours(11) // Ends at 11 AM
        };
        var lessonId1 = await ReserveLesson(reserveLessonRequest);

        // Act - Create Tag
        var newTag = new NewTag
        {
            LessonId = lessonId1 ?? Guid.NewGuid(),
            Name = "Test topic",
            Description = "Test description"
        };

        await CreateTag(newTag);

        // Assert - Verify if the tag was created successfully
        var lessonData = await GetLessonData(lessonId1!.Value);
        Assert.NotEmpty(lessonData.Tags);

        // Act - Remove Tag
        await RemoveTag(lessonData.Tags.First().Id);

        // Assert - Verify if the tag was removed successfully
        lessonData = await GetLessonData(lessonId1.Value);
        Assert.Empty(lessonData.Tags);
    }

    // ===== Authorization methods =====
    private async Task RegisterAndAuthorizeUser(Register registerData)
    {
        var content = JsonContent.Create(registerData);
        var url = "/api/v1/identity/account/register?expiresInSeconds=3600";

        // Act - Register
        var response = await _client.PostAsync(url, content);

        // Assert
        response.EnsureSuccessStatusCode();

        // Act - Login
        var loginData = new
        {
            registerData.Email,
            registerData.Password,
            registerData.IsTutor
        };

        content = JsonContent.Create(loginData);
        url = "/api/v1/identity/account/login?expiresInSeconds=3600";
        response = await _client.PostAsync(url, content);

        // Assert
        response.EnsureSuccessStatusCode();

        // Extract JWT from response and add it to HttpClient headers for subsequent requests
        var loginResponse = await response.Content.ReadFromJsonAsync<JWTResponse>();
        SetJwtForHttpClient(loginResponse!.JWT);
    }

    private async Task<Guid> LoginAndAuthorizeUser(Login loginData)
    {
        var content = JsonContent.Create(loginData);
        var url = "/api/v1/identity/account/login?expiresInSeconds=3600";
        var response = await _client.PostAsync(url, content);
        response.EnsureSuccessStatusCode();

        // Extract JWT from response and add it to HttpClient headers for subsequent requests
        var loginResponse = await response.Content.ReadFromJsonAsync<JWTResponse>();
        SetJwtForHttpClient(loginResponse!.JWT);

        // Extract UserId from JWT
        return JwtTokenHelper.GetUserIdFromToken(loginResponse.JWT);
    }

    private void SetJwtForHttpClient(string jwt)
    {
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwt);
    }

    // ===== Lesson methods =====
    private async Task<Guid?> ReserveLesson(ReserveLessonRequest reserveLessonRequest)
    {
        var content = JsonContent.Create(reserveLessonRequest);
        var url = "/api/v1/Lessons/ReserveLesson";
        var response = await _client.PostAsync(url, content);
        response.EnsureSuccessStatusCode();
        var reserveLessonResponse =
            await response.Content.ReadFromJsonAsync<ReserveLessonResponse>(_camelCaseJsonSerializerOptions);
        return reserveLessonResponse?.LessonId;
    }

    private async Task AcceptDeclineLesson(AcceptDeclineRequest acceptDeclineRequest)
    {
        var content = JsonContent.Create(acceptDeclineRequest);
        var url = "/api/v1/Lessons/AcceptDeclineLesson";
        var response = await _client.PutAsync(url, content);
        response.EnsureSuccessStatusCode();
    }

    private async Task<LessonData> GetLessonData(Guid lessonId)
    {
        var url = $"/api/v1/Lessons/GetLessonData/{lessonId}";
        var response = await _client.GetAsync(url);
        response.EnsureSuccessStatusCode();
        return (await response.Content.ReadFromJsonAsync<LessonData>(_camelCaseJsonSerializerOptions))!;
    }
    
    private async Task CreateTag(NewTag newTag)
    {
        var content = JsonContent.Create(newTag);
        var url = "/api/v1/Lessons/AddTag";
        var response = await _client.PostAsync(url, content);
        response.EnsureSuccessStatusCode();
    }

    private async Task RemoveTag(Guid tagId)
    {
        var url = $"/api/v1/Lessons/RemoveTag?tagId={tagId}";
        var response = await _client.DeleteAsync(url);
        response.EnsureSuccessStatusCode();
    }

    // ===== Subject methods =====
    private async Task<List<SubjectDetails>> GetAllSubjects()
    {
        var url = "/api/v1/subjects/GetAllSubjects";
        var response = await _client.GetAsync(url);
        response.EnsureSuccessStatusCode();
        var subjects = await response.Content.ReadFromJsonAsync<List<SubjectDetails>>(_camelCaseJsonSerializerOptions);
        return subjects!;
    }

    private async Task<SubjectDetails> GetSubjectDetails(Guid subjectId)
    {
        var url = $"/api/v1/subjects/GetSubjectDetails/{subjectId}";
        var response = await _client.GetAsync(url);
        response.EnsureSuccessStatusCode();
        var subjectDetails = await response.Content.ReadFromJsonAsync<SubjectDetails>(_camelCaseJsonSerializerOptions);
        return subjectDetails!;
    }

    private async Task AddRemoveSubject(Guid subjectId, ESubjectAction action)
    {
        var userSubjectAction = new UserSubjectAction
        {
            SubjectId = subjectId,
            SubjectAction = action
        };

        var url = "/api/v1/subjects/AddRemoveSubject";
        var content = JsonContent.Create(userSubjectAction);
        var response = await _client.PutAsync(url, content);
        response.EnsureSuccessStatusCode();
    }

    // ===== Payment method methods =====
    private async Task AddPaymentMethod(NewPaymentMethod newPaymentMethod)
    {
        var content = JsonContent.Create(newPaymentMethod);
        var url = "/api/v1/paymentMethod/AddPaymentMethod";
        var response = await _client.PostAsync(url, content);
        response.EnsureSuccessStatusCode();
    }

    private async Task RemovePaymentMethod(Guid paymentMethodId)
    {
        var url = $"/api/v1/paymentMethod/RemovePaymentMethod?paymentMethodId={paymentMethodId}";
        var response = await _client.DeleteAsync(url);
        response.EnsureSuccessStatusCode();
    }

    private async Task<List<PaymentMethodDetailed>> GetPaymentMethods()
    {
        var url = "/api/v1/paymentMethod/GetPaymentMethodsList";
        var response = await _client.GetAsync(url);
        response.EnsureSuccessStatusCode();
        var paymentMethods =
            await response.Content.ReadFromJsonAsync<List<PaymentMethodDetailed>>(_camelCaseJsonSerializerOptions);
        return paymentMethods!;
    }

    // ===== Availability methods =====
    private async Task AddAvailability(NewAvailability newAvailability)
    {
        var content = JsonContent.Create(newAvailability);
        var url = "/api/v1/Availability/AddAvailability";
        var response = await _client.PostAsync(url, content);
        response.EnsureSuccessStatusCode();
    }

    private async Task RemoveAvailability(Guid availabilityId)
    {
        var url = $"/api/v1/Availability/RemoveAvailability?availabilityId={availabilityId}";
        var response = await _client.DeleteAsync(url);
        response.EnsureSuccessStatusCode();
    }

    private async Task<List<Availability>> GetAvailabilities()
    {
        var url = "/api/v1/Availability/GetAvailabilitiesList";
        var response = await _client.GetAsync(url);
        response.EnsureSuccessStatusCode();
        var availabilities =
            await response.Content.ReadFromJsonAsync<List<Availability>>(_camelCaseJsonSerializerOptions);
        return availabilities!;
    }
}