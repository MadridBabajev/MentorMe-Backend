using App.DAL.Contracts;
using Base.DAL.Contracts;
using BLL.DTO.Profiles;
using Public.DTO.v1.Profiles.Secondary;

namespace App.BLL.Contracts;

public interface IAvailabilityService: IBaseRepository<BLLAvailability>, IAvailabilityRepositoryCustom<BLLAvailability>
{
    // custom methods
    Task<IEnumerable<BLLAvailability>> GetAllAvailabilities(Guid tutorId);
    Task DeleteAvailability(Guid availabilityId);
    Task AddAvailability(NewAvailability newAvailability, Guid tutorId);
}