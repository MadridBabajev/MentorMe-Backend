using Base.DAL.Contracts;
using Domain;
using Domain.Entities;

namespace App.DAL.Contracts;

public interface ITutorsRepository: IBaseRepository<Tutor>
{
    // public Task<IEnumerable<Tutor>> AllAsync(Guid userId);
}
