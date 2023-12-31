#nullable enable
using App.DAL.Contracts;
using App.DAL.EF.Repositories;
using Base.DAL.EF;

namespace App.DAL.EF;

// ReSharper disable InconsistentNaming
public class AppUOW : EFBaseUOW<ApplicationDbContext>, IAppUOW
{
    public AppUOW(ApplicationDbContext dataContext) : base(dataContext)
    {
    }

    private IStudentsRepository? _studentsRepository;
    private ITutorsRepository? _tutorsRepository;
    private ILessonsRepository? _lessonsRepository;
    private ISubjectsRepository? _subjectsRepository;
    private IAvailabilityRepository? _availabilityRepository;
    private IPaymentMethodRepository? _paymentMethodRepository;

    public IStudentsRepository StudentsRepository =>
        _studentsRepository ??= new StudentsRepository(UowDbContext);
    
    public ITutorsRepository TutorsRepository =>
        _tutorsRepository ??= new TutorsRepository(UowDbContext);
    
    public ILessonsRepository LessonsRepository =>
        _lessonsRepository ??= new LessonsRepository(UowDbContext);
    
    public ISubjectsRepository SubjectsRepository =>
        _subjectsRepository ??= new SubjectsRepository(UowDbContext);
    
    public IAvailabilityRepository AvailabilityRepository =>
        _availabilityRepository ??= new AvailabilityRepository(UowDbContext);
    
    public IPaymentMethodRepository PaymentMethodRepository =>
        _paymentMethodRepository ??= new PaymentMethodRepository(UowDbContext);
}
