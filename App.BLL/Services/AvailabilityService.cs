using App.BLL.Contracts;
using App.DAL.Contracts;
using Base.BLL;
using Base.Mapper.Contracts;
using BLL.DTO.Profiles;
using Public.DTO.v1.Profiles.Secondary;
using TutorAvailability = Domain.Entities.TutorAvailability;

namespace App.BLL.Services;

public class AvailabilityService: BaseEntityService<BLLAvailability, TutorAvailability, IAvailabilityRepository>, IAvailabilityService
{
    
    protected readonly IAppUOW Uow;
    
    public AvailabilityService(IAppUOW uow, IMapper<BLLAvailability, TutorAvailability> mapper)
        : base(uow.AvailabilityRepository, mapper)
    {
        Uow = uow;
        
    }

    public async Task<IEnumerable<BLLAvailability>> GetAllAvailabilities(Guid tutorId)
    {
        var availabilities = await Uow.AvailabilityRepository.GetAllById(tutorId);
        return availabilities.Select(pm => Mapper.Map(pm))!;
    }

    public async Task DeleteAvailability(Guid availabilityId) 
        => await Uow.AvailabilityRepository.DeleteById(availabilityId);
    

    public async Task AddAvailability(NewAvailability newAvailability, Guid tutorId) 
        => await Uow.AvailabilityRepository.AddNewAvailability(newAvailability, tutorId);
}