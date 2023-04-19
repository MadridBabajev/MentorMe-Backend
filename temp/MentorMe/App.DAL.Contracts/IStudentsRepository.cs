using Base.DAL.Contracts;
using Domain;
using Domain.Entities;

namespace App.DAL.Contracts;

public interface IStudentsRepository: IBaseRepository<Student>
{
    // public Task<IEnumerable<Student>> AllAsync(Guid userId);
}