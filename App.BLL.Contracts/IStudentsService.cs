using App.DAL.Contracts;
using Base.DAL.Contracts;
using BLL.DTO.Profiles;
using Public.DTO.v1.Profiles;

namespace App.BLL.Contracts;

public interface IStudentsService: IBaseRepository<BLLStudentProfile>, ITutorsRepositoryCustom<BLLStudentProfile>
{
    Task<BLLStudentProfile> GetStudentProfile(Guid authorizedUserId, Guid? visitedUserId);
    Task UpdateStudentProfile(Guid userId, UpdatedProfileData updatedProfileData);
    Task<BLLUpdatedProfileData> GetUserEditableData(Guid userId);
}