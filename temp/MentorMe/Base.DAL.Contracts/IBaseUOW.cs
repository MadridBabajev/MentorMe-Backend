namespace Base.DAL.Contracts;

// ReSharper disable once InconsistentNaming
public interface IBaseUOW
{
    Task<int> SaveChangesAsync();
    // TODO contain and create repositories
}
