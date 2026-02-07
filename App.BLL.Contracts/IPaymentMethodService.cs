using App.DAL.Contracts;
using Base.DAL.Contracts;
using BLL.DTO.Profiles;
using Public.DTO.v1.Profiles.Secondary;

namespace App.BLL.Contracts;

public interface IPaymentMethodService: IBaseRepository<BLLPaymentMethodDetailed>, IPaymentMethodRepositoryCustom<BLLPaymentMethodDetailed>
{
    Task<IEnumerable<BLLPaymentMethodDetailed>> GetAllPaymentMethods(Guid userId);
    Task DeletePaymentMethod(Guid paymentMethodId);
    Task AddPaymentMethod(NewPaymentMethod newPaymentMethod, Guid studentId);
}

