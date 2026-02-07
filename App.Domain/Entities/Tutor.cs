using System.ComponentModel.DataAnnotations.Schema;
using Base.DAL.Contracts;
using Base.DomainEntity;
using Domain.Identity;

namespace Domain.Entities;

public class Tutor: BaseDomainEntity, IDomainEntityId
{
    [Column(TypeName = "decimal(4, 2)")] 
    public double HourlyRate { get; set; } = 15.0;

    // Nav
    public Guid AppUserId { get; set; }
    public AppUser? AppUser { get; set; }
    
    public TutorBankingDetails? BankingDetails { get; set; }
    public ICollection<TutorAvailability>? Availabilities { get; set; }
    public ICollection<TutorSubject>? TutorSubjects { get; set; }
    public ICollection<Lesson>? Lessons { get; set; }
    public ICollection<Review>? Reviews { get; set; }
    
    public ICollection<Tag>? Tags { get; set; }
    
    public ICollection<LessonPayment>? LessonPayments { get; set; }
}
