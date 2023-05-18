using App.DAL.Contracts;
using Base.DAL.Contracts;
using BLL.DTO.Profiles;

namespace App.BLL.Contracts;

public interface IStudentsService: IBaseRepository<BLLStudentProfile>, ITutorsRepositoryCustom<BLLStudentProfile>
{
    Task<BLLStudentProfile> GetStudentProfile(Guid authorizedUserId, Guid? visitedUserId);
}