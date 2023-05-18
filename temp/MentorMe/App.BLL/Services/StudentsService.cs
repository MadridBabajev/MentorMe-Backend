using App.BLL.Contracts;
using App.DAL.Contracts;
using Base.BLL;
using Base.Mapper.Contracts;
using BLL.DTO.Profiles;
using Domain.Entities;

namespace App.BLL.Services;

public class StudentsService:
    BaseEntityService<BLLStudentProfile, Student, IStudentsRepository>, IStudentsService
{
    protected readonly IAppUOW Uow;
    protected readonly IMapper<BLLStudentProfile, Student> StudentProfileMapper;
    public StudentsService(IAppUOW uow, IMapper<BLLStudentProfile, Student> mapper)
        : base(uow.StudentsRepository, mapper)
    {
        Uow = uow;
        StudentProfileMapper = mapper;
    }
    
    public async Task<BLLStudentProfile> GetStudentProfile(Guid authorizedUserId, Guid? visitedUserId)
    {
        var domainStudent = await Uow.StudentsRepository.FindStudentById(authorizedUserId);
        var bllStudent = StudentProfileMapper.Map(domainStudent);

        bllStudent!.IsPublic = visitedUserId != null;;

        return bllStudent;
    }
}