using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using Public.DTO.v1.Identity;
using Xunit.Abstractions;

namespace ProjectTests.Integration;

// ReSharper disable InconsistentNaming
public class IdentityIntegrationTests : IClassFixture<CustomWebAppFactory<Program>>
{
    private readonly HttpClient _client;
    private readonly CustomWebAppFactory<Program> _factory;
    private readonly ITestOutputHelper _testOutputHelper;

    private readonly JsonSerializerOptions _camelCaseJsonSerializerOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };

    public IdentityIntegrationTests(CustomWebAppFactory<Program> factory, ITestOutputHelper testOutputHelper)
    {
        _factory = factory;
        _testOutputHelper = testOutputHelper;
        _client = _factory.CreateClient(new WebApplicationFactoryClientOptions
        {
            AllowAutoRedirect = false
        });
    }

    [Fact(DisplayName = "POST - Register a valid user")]
    public async Task Register_ReturnsJwtResponse_ForValidModel()
    {
        // Arrange
        var registerData = new
        {
            Email = "test@test.com",
            MobilePhone = "1234567890",
            Password = "Test1234!",
            Firstname = "Test",
            Lastname = "User",
            IsTutor = true,
            ConfirmPassword = "Test1234!",
            Country = "Estonia"
        };

        var content = JsonContent.Create(registerData);
        var url = "/api/v1/identity/account/register?expiresInSeconds=3600";

        // Act
        var response = await _client.PostAsync(url, content);
        var responseContent = await response.Content.ReadAsStringAsync();

        // Assert
        response.EnsureSuccessStatusCode();
        var returnedToken = await response.Content.ReadFromJsonAsync<JWTResponse>();
        Assert.NotNull(returnedToken?.JWT);
        Assert.NotNull(returnedToken.RefreshToken);
        VerifyJwtContent(responseContent, registerData.Email, 
            registerData.Firstname, registerData.Lastname,
            registerData.IsTutor ? "Tutor" : "Student",
            DateTime.Now.AddSeconds(3600 + 1).ToUniversalTime());
    }

    [Fact(DisplayName = "POST - Registering an invalid user")]
    public async Task Register_ReturnsBadRequest_ForInvalidModel()
    {
        // Arrange
        var registerData = new
        {
            Email = "test@test.com",
            MobilePhone = "1234567890",
            Password = "Test1234!",
            Firstname = "Test",
            Lastname = "User",
            IsTutor = true,
            ConfirmPassword = "WrongPassword",
            Country = "Estonia"
        };

        var content = JsonContent.Create(registerData);
        var url = "/api/v1/identity/account/register?expiresInSeconds=3600";

        // Act
        var response = await _client.PostAsync(url, content);
        await response.Content.ReadAsStringAsync();

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact(DisplayName = "POST - login user")]
    public async Task LoginUserTest()
    {
        const string email = "login@test.ee";
        const string mobile = "+37253983030";
        const string firstname = "TestFirst";
        const string lastname = "TestLast";
        const string password = "Foo.bar1";
        const string isTutor = "Tutor";
        const string country = "Estonia";
        const int expiresInSeconds = 1;

        // Arrange
        await RegisterNewUser(email, mobile, password,
            firstname, lastname, isTutor, country, expiresInSeconds);

        var URL = "/api/v1/identity/account/login?expiresInSeconds=1";

        var loginData = new
        {
            Email = email,
            Password = password,
            IsTutor = isTutor.Equals("Tutor")
        };

        var data = JsonContent.Create(loginData);

        // Act
        var response = await _client.PostAsync(URL, data);

        var responseContent = await response.Content.ReadAsStringAsync();

        // Assert
        if (!response.IsSuccessStatusCode)
        {
            _testOutputHelper.WriteLine($"Response content: {responseContent}");
        }
        Assert.True(response.IsSuccessStatusCode);
    
        VerifyJwtContent(responseContent, email, firstname, lastname, isTutor,
            DateTime.Now.AddSeconds(expiresInSeconds + 1).ToUniversalTime());
    }

    [Fact(DisplayName = "POST - JWT expired")]
    public async Task JWTExpired()
    {
        const string email = "expired@test.ee";
        const string mobile = "+37253983030";
        const string firstname = "TestFirst";
        const string lastname = "TestLast";
        const string password = "Foo.bar1";
        const string isTutor = "Tutor";
        const string country = "Estonia";
        const int expiresInSeconds = 6; // normally jwt expiry time is 180, changed it to 6 for tests to run faster
        const string URL = "/api/v1/lessons/GetLessonsList";

        // Arrange
        var jwt = await RegisterNewUser(email, mobile, password, firstname, lastname, isTutor, country, expiresInSeconds);
        var jwtResponse = JsonSerializer.Deserialize<JWTResponse>(jwt, _camelCaseJsonSerializerOptions);


        var request = new HttpRequestMessage(HttpMethod.Get, URL);
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", jwtResponse!.JWT);

        // Act
        var response = await _client.SendAsync(request);

        // Assert
        Assert.True(response.IsSuccessStatusCode); 

        // Arrange
        await Task.Delay((expiresInSeconds + 4) * 1000);

        var request2 = new HttpRequestMessage(HttpMethod.Get, URL);
        request2.Headers.Authorization = new AuthenticationHeaderValue("Bearer", jwtResponse.JWT);

        // Act
        var response2 = await _client.SendAsync(request2);

        // Assert
        Assert.False(response2.IsSuccessStatusCode);
    }

    [Fact(DisplayName = "POST - JWT renewal")]
    public async Task JWTRenewal()
    {
        const string email = "expired@test.ee";
        const string mobile = "+37253983030";
        const string firstname = "TestFirst";
        const string lastname = "TestLast";
        const string password = "Foo.bar1";
        const string isTutor = "Tutor";
        const string country = "Estonia";
        const int expiresInSeconds = 6; // normally jwt expiry time is 180, changed it to 6 for tests to run faster
        const string URL = "/api/v1/lessons/GetLessonsList";

        // Arrange
        var jwt = await RegisterNewUser(email, mobile, password, firstname, lastname, isTutor, country, expiresInSeconds);
        var jwtResponse = JsonSerializer.Deserialize<JWTResponse>(jwt, _camelCaseJsonSerializerOptions);
        
        // let the jwt expire
        await Task.Delay((expiresInSeconds + 4) * 1000);

        var request = new HttpRequestMessage(HttpMethod.Get, URL);
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", jwtResponse!.JWT);

        // Act
        var response = await _client.SendAsync(request);

        // Assert
        Assert.False(response.IsSuccessStatusCode);

        // Arrange
        var REFRESH_URL = $"/api/v1/identity/account/refreshtoken?expiresInSeconds={expiresInSeconds}";
        var refreshData = new
        {
            jwtResponse.JWT, jwtResponse.RefreshToken
        };

        var data =  JsonContent.Create(refreshData);
        
        var response2 = await _client.PostAsync(REFRESH_URL, data);
        var responseContent2 = await response2.Content.ReadAsStringAsync();
        
        Assert.True(response2.IsSuccessStatusCode);
        
        jwtResponse = JsonSerializer.Deserialize<JWTResponse>(responseContent2, _camelCaseJsonSerializerOptions);

        var request3 = new HttpRequestMessage(HttpMethod.Get, URL);
        request3.Headers.Authorization = new AuthenticationHeaderValue("Bearer", jwtResponse!.JWT);

        // Act
        var response3 = await _client.SendAsync(request3);
        // Assert
        Assert.True(response3.IsSuccessStatusCode);
    }
    
    private void VerifyJwtContent(string jwt, string email,
        string firstname, string lastname,
        string userType, DateTime validToIsSmallerThan)
    {
        var jwtResponse = JsonSerializer.Deserialize<JWTResponse>(jwt, _camelCaseJsonSerializerOptions);

        Assert.NotNull(jwtResponse);
        Assert.NotNull(jwtResponse.RefreshToken);
        Assert.NotNull(jwtResponse.JWT);

        // verify the actual JWT
        var jwtToken = new JwtSecurityTokenHandler().ReadJwtToken(jwtResponse.JWT);
        Assert.Equal(email, jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value);
        Assert.Equal(firstname, jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.GivenName)?.Value);
        Assert.Equal(lastname, jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Surname)?.Value);
        Assert.Equal(userType, jwtToken.Claims.FirstOrDefault(c => c.Type == "UserType")?.Value);
        Assert.True(jwtToken.ValidTo < validToIsSmallerThan);
    }

    private async Task<string> RegisterNewUser(string email, string mobile, string password, string firstname, string lastname, string isTutor, string country, 
        int expiresInSeconds)
    {
        var URL = $"/api/v1/identity/account/register?expiresInSeconds={expiresInSeconds}";

        var registerData = new
        {
            Email = email,
            MobilePhone = mobile,
            Password = password,
            ConfirmPassword = password,
            Firstname = firstname,
            Lastname = lastname,
            IsTutor = isTutor.Equals("Tutor"),
            Country = country
        };

        var data = JsonContent.Create(registerData);
        // Act
        var response = await _client.PostAsync(URL, data);

        var responseContent = await response.Content.ReadAsStringAsync();
        // Assert
        Assert.True(response.IsSuccessStatusCode);

        VerifyJwtContent(responseContent, email, firstname, lastname, isTutor,
            DateTime.Now.AddSeconds(expiresInSeconds + 1).ToUniversalTime());

        return responseContent;
    }
}