using Base.BLL.Contracts;
using Base.DAL.Contracts;

namespace Base.BLL;

// ReSharper disable once InconsistentNaming
public abstract class BaseBLL<TUow> : IBaseBLL
    where TUow : IBaseUOW
    // where TUow : IBaseUOW
{
    protected readonly TUow Uow;

    protected BaseBLL(TUow uow)
    {
        Uow = uow;
    }

    public virtual async Task<int> SaveChangesAsync()
    {
        return await Uow.SaveChangesAsync();
    }
}