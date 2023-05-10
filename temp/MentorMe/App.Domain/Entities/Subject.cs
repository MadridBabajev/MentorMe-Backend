using System.ComponentModel.DataAnnotations;
using Base.DAL.Contracts;
using Base.DomainEntity;
using Microsoft.EntityFrameworkCore;

namespace Domain.Entities;

[Index(nameof(Name), IsUnique = true)]
public class Subject: BaseDomainEntity, IDomainEntityId
{
    [MinLength(2)]
    [MaxLength(32)]
    [Required]
    public string Name { get; set; } = default!;
    [MinLength(1)]
    [MaxLength(500)]
    public string Description { get; set; } = default!;
    public byte[]? SubjectPicture { get; set; }
    
    public virtual ICollection<TutorSubject>? TutorSubjects { get; set; }
    public virtual ICollection<StudentSubject>? StudentSubjects { get; set; }
}