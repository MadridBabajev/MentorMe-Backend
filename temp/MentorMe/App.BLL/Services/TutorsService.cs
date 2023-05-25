using App.BLL.Contracts;
using App.BLL.Mappers;
using App.DAL.Contracts;
using AutoMapper;
using Base.BLL;
using Base.Mapper.Contracts;
using BLL.DTO.Profiles;
using Domain.Entities;
using Public.DTO.v1.Profiles;

namespace App.BLL.Services;

public class TutorsService :
    BaseEntityService<BLLTutorProfile, Tutor, ITutorsRepository>, ITutorsService
{
    protected IAppUOW Uow;
    protected readonly IMapper<BLLTutorProfile, Tutor> TutorProfileMapper;
    protected readonly IMapper<BLLTutorSearch, Tutor> TutorSearchMapper;

    public TutorsService(IAppUOW uow, IMapper<BLLTutorProfile, Tutor> mapper, IMapper autoMapper)
        : base(uow.TutorsRepository, mapper)
    {
        Uow = uow;
        TutorProfileMapper = mapper;
        TutorSearchMapper = new TutorsSearchMapper(autoMapper);
    }
    
    public async Task<BLLTutorProfile> GetTutorProfile(Guid authorizedUserId, Guid? visitedUserId)
    {
        Tutor domainTutor = visitedUserId == null ? 
            await Uow.TutorsRepository.FindTutorById(authorizedUserId) :
            await Uow.TutorsRepository.FindTutorById(visitedUserId);
        BLLTutorProfile bllTutor = TutorProfileMapper.Map(domainTutor)!;
        
        bllTutor.IsPublic = visitedUserId != null;

        return bllTutor;
    }
    
    public async Task<IEnumerable<BLLTutorSearch?>> GetTutorsWithFilters(TutorSearchFilters filters)
    {
        var res = await Uow.TutorsRepository.AllFilteredAsync(filters);
        return res.Select(t => TutorSearchMapper.Map(t));
    }
    
}