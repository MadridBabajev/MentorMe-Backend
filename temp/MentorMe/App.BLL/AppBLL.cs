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
    private ISubjectsService? _subjects;

    public AppBLL(IAppUOW uow, IMapper mapper) : base(uow)
    {
        Uow = uow;
        _mapper = mapper;
    }

    public ITutorsService TutorsService =>
        _tutors ??= new TutorsService(Uow, new TutorMapper(_mapper));

    public ISubjectsService SubjectsService =>
        _subjects ??= new SubjectsService(Uow, new SubjectsMapper(_mapper));
}
