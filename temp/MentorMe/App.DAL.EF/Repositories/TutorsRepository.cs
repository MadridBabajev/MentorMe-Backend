using App.DAL.Contracts;
using Base.DAL.EF;
using Domain;
using Domain.Entities;

namespace App.DAL.EF.Repositories;

public class TutorsRepository: 
    EFBaseRepository<Tutor, ApplicationDbContext>, ITutorsRepository
{
    public TutorsRepository(ApplicationDbContext dataContext) : base(dataContext)
    {
    }
    // exp.
    // public async Task<IEnumerable<Tutor>> AllAsync(Guid userId)
    // {
    //     return await 
    // }
}