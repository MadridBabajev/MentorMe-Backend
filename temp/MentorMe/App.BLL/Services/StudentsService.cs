using App.BLL.Contracts;
using App.DAL.Contracts;
using Base.BLL;
using Base.Mapper.Contracts;
using BLL.DTO.Profiles;
using Domain.Entities;
using Public.DTO.v1.Profiles;

namespace App.BLL.Services;

public class StudentsService:
    BaseEntityService<BLLStudentProfile, Student, IStudentsRepository>, IStudentsService
{
    protected readonly IAppUOW Uow;
    protected readonly IMapper<BLLStudentProfile, Student> StudentProfileMapper;
    protected readonly IMapper<BLLUpdatedProfileData, Student> EditStudentMapper;
    protected readonly IMapper<BLLUpdatedProfileData, Tutor> EditTutorMapper;
    public StudentsService(IAppUOW uow, IMapper<BLLStudentProfile, Student> mapper,
        IMapper<BLLUpdatedProfileData, Student> editStudentMapper,
        IMapper<BLLUpdatedProfileData, Tutor> editTutorMapper)
        : base(uow.StudentsRepository, mapper)
    {
        Uow = uow;
        StudentProfileMapper = mapper;
        EditStudentMapper = editStudentMapper;
        EditTutorMapper = editTutorMapper;
    }
    
    public async Task<BLLStudentProfile> GetStudentProfile(Guid authorizedUserId, Guid? visitedUserId)
    {
        Student domainStudent = visitedUserId == null ? 
            await Uow.StudentsRepository.FindStudentById(authorizedUserId) :
            await Uow.StudentsRepository.FindStudentById(visitedUserId!);
        var bllStudent = StudentProfileMapper.Map(domainStudent);

        bllStudent!.IsPublic = visitedUserId != null;

        return bllStudent;
    }
    
    public async Task<BLLUpdatedProfileData> GetUserEditableData(Guid userId)
        => await Uow.StudentsRepository.UserIsStudent(userId) 
            ? EditStudentMapper.Map(await Uow.StudentsRepository.GetStudentEditProfileData(userId))!
            : EditTutorMapper.Map(await Uow.TutorsRepository.GetTutorEditProfileData(userId))!;
    
    public async Task UpdateStudentProfile(Guid studentId, UpdatedProfileData updatedProfileData)
        => await Uow.StudentsRepository.UpdateStudentProfileData(studentId, updatedProfileData)!;
}