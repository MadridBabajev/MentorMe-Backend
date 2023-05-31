using App.BLL.Contracts;
using App.DAL.Contracts;
using Base.BLL;
using Base.Mapper.Contracts;
using BLL.DTO.Profiles;
using Domain.Entities;
using Public.DTO.v1.Profiles;
using DomainTutorBankingDetails = Domain.Entities.TutorBankingDetails;
using UpdatedBankingDetails = Public.DTO.v1.Profiles.Secondary.TutorBankingDetails;

namespace App.BLL.Services;

public class TutorsService :
    BaseEntityService<BLLTutorProfile, Tutor, ITutorsRepository>, ITutorsService
{
    protected IAppUOW Uow;
    protected readonly IMapper<BLLTutorProfile, Tutor> TutorProfileDetailsMapper;
    protected readonly IMapper<BLLTutorSearch, Tutor> TutorSearchMapper;
    protected readonly IMapper<BLLTutorBankingDetails, DomainTutorBankingDetails> BankingDetailsMapper;

    public TutorsService(IAppUOW uow, IMapper<BLLTutorProfile, Tutor> detailsMapper,
        IMapper<BLLTutorSearch, Tutor> searchMapper, IMapper<BLLTutorBankingDetails, DomainTutorBankingDetails> bankingDetailsMapper)
        : base(uow.TutorsRepository, detailsMapper)
    {
        Uow = uow;
        TutorProfileDetailsMapper = detailsMapper;
        TutorSearchMapper = searchMapper;
        BankingDetailsMapper = bankingDetailsMapper;
    }
    
    public async Task<BLLTutorProfile> GetTutorProfile(Guid authorizedUserId, Guid? visitedUserId)
    {
        Tutor domainTutor = visitedUserId == null ? 
            await Uow.TutorsRepository.FindTutorById(authorizedUserId) :
            await Uow.TutorsRepository.FindTutorById(visitedUserId);
        BLLTutorProfile bllTutor = TutorProfileDetailsMapper.Map(domainTutor)!;
        
        bllTutor.IsPublic = visitedUserId != null;

        return bllTutor;
    }
    
    public async Task<IEnumerable<BLLTutorSearch?>> GetTutorsWithFilters(TutorSearchFilters filters)
    {
        var res = await Uow.TutorsRepository.AllFilteredAsync(filters);
        return res.Select(t => TutorSearchMapper.Map(t));
    }

    public async Task<BLLTutorBankingDetails> GetTutorBankingDetails(Guid tutorId)
        => BankingDetailsMapper.Map(await Uow.TutorsRepository.GetTutorBankingDetails(tutorId))!;
    

    public async Task EditTutorBankingDetails(Guid tutorId, UpdatedBankingDetails updatedBankingDetails)
        => await Uow.TutorsRepository.UpdateBankingDetails(tutorId, updatedBankingDetails);
    

    public async Task UpdateTutorProfile(Guid tutorId, UpdatedProfileData updatedProfileData)
        => await Uow.TutorsRepository.UpdateTutorProfileData(tutorId, updatedProfileData)!;
    
}
