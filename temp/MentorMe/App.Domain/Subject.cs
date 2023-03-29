using System.ComponentModel.DataAnnotations;
using Base.DomainEntity;
using Microsoft.EntityFrameworkCore;

namespace Domain;

[Index(nameof(Name), IsUnique = true)]
public class Subject: BaseDomainEntity
{
    [MinLength(2)]
    [MaxLength(32)]
    [Required]
    public string Name { get; set; } = default!;
    [MinLength(1)]
    [MaxLength(500)]
    public string Description { get; set; } = default!;
    public virtual ICollection<TutorSubject>? TutorSubjects { get; set; }
    public virtual ICollection<StudentSubject>? StudentSubjects { get; set; }
}