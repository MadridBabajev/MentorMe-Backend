using App.DAL.Contracts;
using Base.DAL.EF;
using Domain;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

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
    public override async Task<IEnumerable<Tutor>> AllAsync()
    {
        return await RepositoryDbSet
            .OrderBy(e => e.CreatedAt)
            .ToListAsync();
    }
}