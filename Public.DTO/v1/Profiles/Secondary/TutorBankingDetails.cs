using Domain.Enums;

namespace Public.DTO.v1.Profiles.Secondary;

/// <summary>
/// Represents the banking details of a tutor.
/// </summary>
public class TutorBankingDetails
{
    /// <summary>
    /// Gets or sets the bank account number of the tutor.
    /// </summary>
    public string BankAccountNumber { get; set; } = default!;

    /// <summary>
    /// Gets or sets the bank account name of the tutor.
    /// </summary>
    public string BankAccountName { get; set; } = default!;

    /// <summary>
    /// Gets or sets the type of the bank account.
    /// </summary>
    public EBankAccountType BankAccountType { get; set; }
}