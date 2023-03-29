using System.ComponentModel.DataAnnotations.Schema;
using Domain.Identity;

namespace Domain;

public class Tutor: AppUser
{
    [Column(TypeName = "decimal(4, 2)")] 
    public double HourlyRate { get; set; } = 10.0;
    public int HoursTutored { get; set; }

    // Nav
    public Guid DialogFeatureUserId { get; set; }
    public DialogFeatureUser? DialogFeatureUser { get; set; }
    public TutorBankingDetails? BankingDetails { get; set; }
    public ICollection<TutorAvailability>? Availabilities { get; set; }
    public ICollection<TutorSubject>? TutorSubjects { get; set; }
    public ICollection<Lesson>? Lessons { get; set; }
    public ICollection<Review>? Reviews { get; set; }
    public ICollection<Tag>? Tags { get; set; }
    public ICollection<LessonPayment>? LessonPayments { get; set; }
}
