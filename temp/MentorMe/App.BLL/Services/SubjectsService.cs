using App.BLL.Contracts;
using App.DAL.Contracts;
using Base.BLL;
using Base.DAL.Contracts;
using Base.Mapper.Contracts;
using BLL.DTO;
using DomainSubject = Domain.Entities.Subject;

namespace App.BLL.Services;

public class SubjectsService :
    BaseEntityService<BLLSubjectListElement, DomainSubject, ISubjectsRepository>, ISubjectsService
{
    protected readonly IAppUOW Uow;

    public SubjectsService(IAppUOW uow, IMapper<BLLSubjectListElement, DomainSubject> mapper)
        : base(uow.SubjectsRepository, mapper)
    {
        Uow = uow;
    }
    
    public async Task<IEnumerable<BLLSubjectListElement>> AllSubjects()
    {
        return await Uow.SubjectsRepository.AllSubjectsAsync();
    }

    Task<IEnumerable<BLLSubjectDetails>> IBaseRepository<BLLSubjectDetails, Guid>.AllAsync()
    {
        throw new NotImplementedException();
    }

    public new async Task<BLLSubjectDetails?> FindAsync(Guid id)
    {
        return await Uow.SubjectsRepository.FindAsyncWithDetails(id);
    }

    public BLLSubjectDetails Add(BLLSubjectDetails entity)
    {
        // Uow.SubjectsRepository.Add(Mapper.Map(entity));
        throw new NotImplementedException();
    }

    public BLLSubjectDetails Update(BLLSubjectDetails entity)
    {
        throw new NotImplementedException();
    }

    public BLLSubjectDetails Remove(BLLSubjectDetails entity)
    {
        throw new NotImplementedException();
    }

    public new Task<BLLSubjectDetails?> RemoveAsync(Guid id)
    {
        throw new NotImplementedException();
    }
}
