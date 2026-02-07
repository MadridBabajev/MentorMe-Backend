using Base.DAL.Contracts;

namespace App.DAL.Contracts;

// ReSharper disable once InconsistentNaming
public interface IAppUOW : IBaseUOW
{
    // list all the repositories
    IStudentsRepository StudentsRepository { get; }
    ITutorsRepository TutorsRepository { get; }
    ILessonsRepository LessonsRepository { get; }
    ISubjectsRepository SubjectsRepository { get; }
    IAvailabilityRepository AvailabilityRepository { get; }
    IPaymentMethodRepository PaymentMethodRepository { get; }
}
