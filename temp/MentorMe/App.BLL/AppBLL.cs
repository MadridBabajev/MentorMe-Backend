using App.BLL.Contracts;
using App.BLL.Mappers;
using App.BLL.Services;
using App.DAL.Contracts;
using AutoMapper;
using Base.BLL;

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

    public AppBLL(IAppUOW uow, IMapper mapper) : base(uow)
    {
        Uow = uow;
        _mapper = mapper;
    }

    public ITutorsService TutorsService =>
        _tutors ??= new TutorsService(Uow, new TutorProfileMapper(_mapper), _mapper);
    
    public IStudentsService StudentsService =>
        _students ??= new StudentsService(Uow, new StudentProfileMapper(_mapper));

    public ISubjectsService SubjectsService =>
        _subjects ??= new SubjectsService(Uow, new SubjectsListMapper(_mapper),
            new SubjectDetailsMapper(_mapper),
            _mapper);
    
    public ILessonsService LessonsService =>
        _lessons ??= new LessonsService(Uow, new LessonDataMapper(_mapper), _mapper);
}
