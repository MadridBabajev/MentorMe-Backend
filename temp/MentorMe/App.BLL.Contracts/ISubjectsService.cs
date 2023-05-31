using App.DAL.Contracts;
using Base.DAL.Contracts;
using BLL.DTO.Subjects;
using Public.DTO.v1.Subjects;

namespace App.BLL.Contracts;

public interface ISubjectsService: IBaseRepository<BLLSubjectDetails>, ISubjectsRepositoryCustom<BLLSubjectDetails>
{
    // add your custom service methods here
     public Task<IEnumerable<BLLSubjectListElement>> AllSubjects();

     Task<BLLSubjectDetails?> FindSubjectAsync(Guid subjectId, Guid? userId);

     public Task<IEnumerable<BLLSubjectsFilterElement?>> AllSubjectFilters();
     Task AddRemoveUserSubject(UserSubjectAction userSubjectAction, Guid userId);
}