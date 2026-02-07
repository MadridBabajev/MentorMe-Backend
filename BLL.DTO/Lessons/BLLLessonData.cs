using Base.DAL.Contracts;
using BLL.DTO.Profiles;
using BLL.DTO.Subjects;
using Domain.Enums;

namespace BLL.DTO.Lessons;

public class BLLLessonData: IDomainEntityId
{
    public Guid Id { get; set; }
    public Guid PaymentId { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public ELessonState LessonState { get; set; }
    public double Price { get; set; }
    public bool ViewedByTutor { get; set; }
    public bool UserCanWriteReview { get; set; }
    public BLLStudentPaymentMethod StudentPaymentMethod { get; set; }
    public BLLSubjectListElement Subject { get; set; }
    public BLLProfileCardData LessonStudent { get; set; }
    public BLLProfileCardData LessonTutor { get; set; }
    public ICollection<BLLTag> Tags { get; set; }
}