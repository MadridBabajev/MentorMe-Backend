using Base.DAL.Contracts;
using Base.DomainEntity;
using Domain.Identity;

namespace Domain.Entities;

public class Student: BaseDomainEntity, IDomainEntityId
{

    // Nav
    public Guid AppUserId { get; set; }
    public AppUser? AppUser { get; set; }
    public ICollection<Lesson>? Lessons { get; set; }
    public ICollection<StudentSubject>? StudentSubjects { get; set; }
    public ICollection<Review>? Reviews { get; set; }
    public ICollection<Cancellation>? Cancellations { get; set; }
    public ICollection<LessonPayment>? LessonPayments { get; set; }
    public ICollection<StudentPaymentMethod>? PaymentMethods { get; set; }
}
