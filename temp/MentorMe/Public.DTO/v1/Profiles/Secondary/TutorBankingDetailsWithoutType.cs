namespace Public.DTO.v1.Profiles.Secondary;

public class TutorBankingDetailsWithoutType
{
    public string BankAccountNumber { get; set; } = default!;
    public string BankAccountName { get; set; } = default!;
    public int BankAccountType { get; set; }
}