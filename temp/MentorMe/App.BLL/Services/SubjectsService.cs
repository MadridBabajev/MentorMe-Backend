using App.BLL.Contracts;
using App.BLL.Mappers;
using App.DAL.Contracts;
using AutoMapper;
using Base.BLL;
using Base.DAL.Contracts;
using Base.Mapper.Contracts;
using BLL.DTO.Subjects;
using DomainSubject = Domain.Entities.Subject;

namespace App.BLL.Services;

public class SubjectsService :
    BaseEntityService<BLLSubjectListElement, DomainSubject, ISubjectsRepository>, ISubjectsService
{
    protected readonly IAppUOW Uow;
    protected readonly IMapper<BLLSubjectDetails, DomainSubject> SubjectDetailsMapper;
    protected readonly IMapper<BLLSubjectsFilterElement, DomainSubject> SubjectsFilterMapper;

    public SubjectsService(IAppUOW uow, IMapper<BLLSubjectListElement, DomainSubject> subjectListMapper, IMapper<BLLSubjectDetails, DomainSubject> subjectDetailsMapper, 
        IMapper autoMapper)
        : base(uow.SubjectsRepository, subjectListMapper)
    {
        Uow = uow;
        SubjectDetailsMapper = subjectDetailsMapper;
        SubjectsFilterMapper = new SubjectsFilterMapper(autoMapper);
    }
    
    public async Task<IEnumerable<BLLSubjectListElement>> AllSubjects()
    {
        return await Uow.SubjectsRepository.AllSubjectsAsync();
    }

    Task<IEnumerable<BLLSubjectDetails>> IBaseRepository<BLLSubjectDetails, Guid>.AllAsync()
    {
        throw new NotImplementedException();
    }

    Task<BLLSubjectDetails?> IBaseRepository<BLLSubjectDetails, Guid>.FindAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<BLLSubjectsFilterElement?>> AllSubjectFilters()
    {
        var res = await Uow.SubjectsRepository.AllAsync();
        return res.Select(s => SubjectsFilterMapper.Map(s));
    }

    public new async Task<BLLSubjectDetails?> FindAsync(Guid id)
    {
        var subject = await Uow.SubjectsRepository.FindAsyncWithDetails(id);

        var x = SubjectDetailsMapper.Map(subject);
        return x;
        //
        // var x = SubjectDetailsMapper.Map(await Uow.SubjectsRepository.FindAsyncWithDetails(id));
        // return x;
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
