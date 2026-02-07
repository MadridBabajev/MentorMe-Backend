using Base.DAL.Contracts;
using Domain.Enums;

namespace BLL.DTO.Profiles;

public class BLLTutorBankingDetails: IDomainEntityId
{
    public Guid Id { get; set; }
    public EBankAccountType BankAccountType { get; set; }
    public string BankAccountNumber { get; set; } = default!;
    public string BankAccountName { get; set; } = default!;
}