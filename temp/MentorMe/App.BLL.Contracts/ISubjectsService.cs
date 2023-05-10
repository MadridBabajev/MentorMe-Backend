using App.DAL.Contracts;
using Base.DAL.Contracts;
using BLL.DTO;

namespace App.BLL.Contracts;

public interface ISubjectsService: IBaseRepository<BLLSubjectDetails>, ISubjectsRepositoryCustom<BLLSubjectDetails>
{
    // add your custom service methods here
     public Task<IEnumerable<BLLSubjectListElement>> AllSubjects();
}