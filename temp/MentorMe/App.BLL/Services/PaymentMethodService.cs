using App.BLL.Contracts;
using App.DAL.Contracts;
using Base.BLL;
using Base.Mapper.Contracts;
using BLL.DTO.Profiles;
using Public.DTO.v1.Profiles.Secondary;
using StudentPaymentMethod = Domain.Entities.StudentPaymentMethod;

namespace App.BLL.Services;

public class PaymentMethodService: BaseEntityService<BLLPaymentMethodDetailed, StudentPaymentMethod, IPaymentMethodRepository>, IPaymentMethodService
{
    protected readonly IAppUOW Uow;
    public PaymentMethodService(IAppUOW uow, IMapper<BLLPaymentMethodDetailed, StudentPaymentMethod> mapper) 
        : base(uow.PaymentMethodRepository, mapper)
    {
        Uow = uow;
    }

    public async Task<IEnumerable<BLLPaymentMethodDetailed>> GetAllPaymentMethods(Guid userId)
    {
        var paymentMethods = await Uow.PaymentMethodRepository.GetAllById(userId);
        return paymentMethods.Select(pm => Mapper.Map(pm))!;
    }

    public async Task DeletePaymentMethod(Guid paymentMethodId) 
        => await Uow.PaymentMethodRepository.DeleteById(paymentMethodId);
    
    public async Task AddPaymentMethod(NewPaymentMethod newPaymentMethod, Guid studentId) 
        => await Uow.PaymentMethodRepository.AddNewPaymentMethod(newPaymentMethod, studentId);
}