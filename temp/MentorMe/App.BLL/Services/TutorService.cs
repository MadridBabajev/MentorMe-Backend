using App.BLL.Contracts;
using App.DAL.Contracts;
using Base.BLL;
using Base.Mapper.Contracts;
using BLL.DTO;
using DomainTutor = Domain.Entities.Tutor;

namespace App.BLL.Services;

public class TutorsService :
    BaseEntityService<BLLTutorSearch, DomainTutor, ITutorsRepository>, ITutorsService
{
    protected IAppUOW Uow;

    public TutorsService(IAppUOW uow, IMapper<BLLTutorSearch, DomainTutor> mapper)
        : base(uow.TutorsRepository, mapper)
    {
        Uow = uow;
    }
    
    public async Task<IEnumerable<BLLTutorSearch>> AllAsync(Guid userId)
    {
        return (await Uow.TutorsRepository.AllAsync())
            .Select(e => Mapper.Map(e))!;
    }

    // public async Task<DomainTutor?> FindAsync(Guid id, Guid userId)
    // {
    //     return Mapper.Map(await Uow.TutorsRepository.FindAsync(id, userId));
    // }

    // public async Task<DomainTutor?> RemoveAsync(Guid id, Guid userId)
    // {
    //     throw new NotImplementedException();
    // }

    // public async Task<bool> IsOwnedByUserAsync(Guid id, Guid userId)
    // {
    //     throw new NotImplementedException();
    // }
    //
    // public async Task<IEnumerable<TrainingPlanWithEventCount>> AllWithPlanCountAsync(Guid userId)
    // {
    //     throw new NotImplementedException();
    // }
}