using Public.DTO.v1.Profiles.Secondary;
using Public.DTO.v1.Subjects;

namespace Public.DTO.v1.Lessons;

public class ReserveLessonData
{
    public List<StudentPaymentMethod> PaymentMethods { get; set; }
    public List<Availability> Availabilities { get; set; }
    public List<SubjectsFilterElement> Subjects { get; set; }
}