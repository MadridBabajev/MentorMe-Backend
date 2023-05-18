using App.DAL.Contracts;
using Base.DAL.Contracts;
using BLL.DTO;
using BLL.DTO.Profiles;
using Public.DTO.v1.Profiles;

namespace App.BLL.Contracts;

public interface ITutorsService: IBaseRepository<BLLTutorProfile>, ITutorsRepositoryCustom<BLLTutorProfile>
{
    // add your custom service methods here
    Task<BLLTutorProfile> GetTutorProfile(Guid authorizedUserId, Guid? visitedUserId);
    Task<IEnumerable<BLLTutorSearch?>> GetTutorsWithFilters(TutorSearchFilters filters);
}