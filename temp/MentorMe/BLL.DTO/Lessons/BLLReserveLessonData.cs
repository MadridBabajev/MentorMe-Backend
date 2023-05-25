using Base.DAL.Contracts;
using BLL.DTO.Profiles;
using BLL.DTO.Subjects;
using Domain.Entities;

namespace BLL.DTO.Lessons;

public class BLLReserveLessonData: IDomainEntityId
{
    public Guid Id { get; set; }
    public IEnumerable<BLLStudentPaymentMethod?> PaymentMethods { get; set; }
    public IEnumerable<BLLTutorAvailability?> Availabilities { get; set; }
    public IEnumerable<BLLSubjectsFilterElement?> Subjects { get; set; }
}