#nullable enable
using Base.DAL.Contracts;
using Domain.Entities;

namespace App.DAL.Contracts;

public interface ILessonsRepository: IBaseRepository<Lesson>
{
    // public Task<IEnumerable<Lessons>> AllAsync(Guid userId);
    Task<List<Lesson>> AllAsync(Guid getUserId);
    public Task<Lesson?> FindAsync(Guid id, Guid userId);
}