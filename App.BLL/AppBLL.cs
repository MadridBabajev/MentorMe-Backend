using App.BLL.Contracts;
using App.BLL.Mappers;
using App.BLL.Services;
using App.BLL.Services.ML;
using App.DAL.Contracts;
using AutoMapper;
using Base.BLL;
using AvailabilityMapper = App.BLL.Mappers.AvailabilityMapper;
using BankingDetailsMapper = App.BLL.Mappers.BankingDetailsMapper;
using LessonDataMapper = App.BLL.Mappers.LessonDataMapper;
using LessonListMapper = App.BLL.Mappers.LessonListMapper;
using PaymentMapper = App.BLL.Mappers.PaymentMapper;
using PaymentMethodDetailedMapper = App.BLL.Mappers.PaymentMethodDetailedMapper;
using StudentProfileMapper = App.BLL.Mappers.StudentProfileMapper;
using SubjectDetailsMapper = App.BLL.Mappers.SubjectDetailsMapper;
using SubjectsFilterMapper = App.BLL.Mappers.SubjectsFilterMapper;
using TutorProfileMapper = App.BLL.Mappers.TutorProfileMapper;
using TutorsSearchMapper = App.BLL.Mappers.TutorsSearchMapper;

namespace App.BLL;

// ReSharper disable InconsistentNaming
public class AppBLL : BaseBLL<IAppUOW>, IAppBLL
{
    protected new readonly IAppUOW Uow;
    private readonly IMapper _mapper;

    // services
    private ITutorsService? _tutors;
    private IStudentsService? _students;
    private ISubjectsService? _subjects;
    private ILessonsService? _lessons;
    private IAvailabilityService? _availabilities;
    private IPaymentMethodService? _paymentMethods;
    private IOcrInferenceService? _ocrInference;
    private ISummarizationInferenceService? _summarizationInference;

    public AppBLL(IAppUOW uow, IMapper mapper) : base(uow)
    {
        Uow = uow;
        _mapper = mapper;
    }

    public ITutorsService TutorsService =>
        _tutors ??= new TutorsService(Uow,
            new TutorProfileMapper(_mapper), 
            new TutorsSearchMapper(_mapper),
            new BankingDetailsMapper(_mapper));
    
    public IStudentsService StudentsService =>
        _students ??= new StudentsService(Uow, new StudentProfileMapper(_mapper),
            new EditStudentMapper(_mapper),
            new EditTutorMapper(_mapper));

    public ISubjectsService SubjectsService =>
        _subjects ??= new SubjectsService(Uow, new SubjectsListMapper(_mapper),
            new SubjectDetailsMapper(_mapper),
            new SubjectsFilterMapper(_mapper));
    
    public ILessonsService LessonsService =>
        _lessons ??= new LessonsService(Uow, new LessonDataMapper(_mapper),
            new LessonListMapper(_mapper),
            new PaymentMethodMapper(_mapper),
            new AvailabilityMapper(_mapper),
            new SubjectsFilterMapper(_mapper),
            new PaymentMapper(_mapper));
    
    public IAvailabilityService AvailabilityService =>
        _availabilities ??= new AvailabilityService(Uow, new AvailabilityMapper(_mapper));
    
    public IPaymentMethodService PaymentMethodService =>
        _paymentMethods ??= new PaymentMethodService(Uow, new PaymentMethodDetailedMapper(_mapper));
    
    public IOcrInferenceService OcrInferenceService =>
        _ocrInference ??= new OcrInferenceService();
    
    public ISummarizationInferenceService SummarizationInferenceService =>
        _summarizationInference ??= new SummarizationInferenceService();
}
