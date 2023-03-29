using Domain.Identity;

namespace Domain;

public class Student: AppUser
{
    
    // Nav
    public Guid DialogFeatureUserId { get; set; }
    public DialogFeatureUser? DialogFeatureUser { get; set; }
    public ICollection<Lesson>? Lessons { get; set; }
    public ICollection<StudentSubject>? StudentSubjects { get; set; }
    public ICollection<Review>? Reviews { get; set; }
    public ICollection<Cancellation>? Cancellations { get; set; }
    public ICollection<LessonPayment>? LessonPayments { get; set; }
    public ICollection<StudentPaymentMethod>? PaymentMethods { get; set; }
}
