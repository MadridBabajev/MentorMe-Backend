namespace Public.DTO.v1.Profiles;

public class TutorSearchFilters
{
    
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public int? MinClassesCount { get; set; }
    public int? MaxClassesCount { get; set; }
    public double? MinHourlyRate { get; set; }
    public double? MaxHourlyRate { get; set; }
    public double? MinAverageRating { get; set; }
    public double? MaxAverageRating { get; set; }
    public List<Guid> SubjectIds { get; set; }
}