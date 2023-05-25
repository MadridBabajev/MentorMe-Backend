using Public.DTO.v1.Profiles;
using Public.DTO.v1.Subjects;

namespace Public.DTO.v1.Lessons;

public class ReserveLessonData
{
    public List<StudentPaymentMethod> PaymentMethods { get; set; }
    public List<TutorAvailability> Availabilities { get; set; }
    public List<SubjectsFilterElement> Subjects { get; set; }
}