using System.ComponentModel.DataAnnotations;
using Base.DomainEntity;
using Domain.Enums;

namespace Domain.Entities;

public class Review: BaseDomainEntity
{
    [Range(typeof(decimal), "1", "5")]
    public int Rating { get; set; } = 5;
    [MinLength(1)]
    [MaxLength(500)]
    public string? Comment { get; set; }
    public EReviewType ReviewType { get; set; }
    
    // Nav
    public Guid StudentId { get; set; }
    public Student Student { get; set; } = default!;
    public Guid TutorId { get; set; }
    public Tutor Tutor { get; set; } = default!;
}
