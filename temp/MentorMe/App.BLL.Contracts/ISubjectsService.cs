using App.DAL.Contracts;
using Base.DAL.Contracts;
using BLL.DTO;
using BLL.DTO.Subjects;
using Domain.Entities;

namespace App.BLL.Contracts;

public interface ISubjectsService: IBaseRepository<BLLSubjectDetails>, ISubjectsRepositoryCustom<BLLSubjectDetails>
{
    // add your custom service methods here
     public Task<IEnumerable<BLLSubjectListElement>> AllSubjects();

     new Task<BLLSubjectDetails?> FindAsync(Guid id);

     public Task<IEnumerable<BLLSubjectsFilterElement?>> AllSubjectFilters();
}