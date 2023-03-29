#nullable enable
using App.DAL.Contracts;
using App.DAL.EF.Repositories;
using Base.DAL.EF;

namespace App.DAL.EF;

// ReSharper disable InconsistentNaming
public class AppUOW : EFBaseUOW<ApplicationDbContext>, IAppUOW
{
    public AppUOW(ApplicationDbContext dataContext) : base(dataContext)
    {
    }

    private IStudentsRepository? _studentsRepository;
    private ITutorsRepository? _tutorsRepository;

    public IStudentsRepository StudentsRepository =>
        _studentsRepository ??= new StudentsRepository(UowDbContext);
    
    public ITutorsRepository TutorsRepository =>
        _tutorsRepository ??= new TutorsRepository(UowDbContext);
}
