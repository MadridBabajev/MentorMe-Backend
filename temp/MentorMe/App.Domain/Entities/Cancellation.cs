using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Base.DomainEntity;
using Domain.Enums;

namespace Domain.Entities;

public class Cancellation: BaseDomainEntity
{
    [MinLength(2)]
    [MaxLength(32)]
    public string Reason { get; set; } = default!;
    [Column(TypeName = "decimal(4, 2)")]
    public double? Penalty { get; set; }
    public ECancellationType CancellationType { get; set; }
    
    // Nav
    public Guid LessonId { get; set; }
    public Lesson? Lesson { get; set; }
    public Guid StudentId { get; set; }
    public Student? Student { get; set; }
}
