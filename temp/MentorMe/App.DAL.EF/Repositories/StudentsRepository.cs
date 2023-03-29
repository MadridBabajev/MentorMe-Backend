using App.DAL.Contracts;
using Base.DAL.EF;
using Domain;

namespace App.DAL.EF.Repositories;

public class StudentsRepository: EFBaseRepository<Student, ApplicationDbContext>, IStudentsRepository
{
    public StudentsRepository(ApplicationDbContext dataContext) : base(dataContext)
    {
    }

}