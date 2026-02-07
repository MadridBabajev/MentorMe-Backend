namespace Public.DTO.v1.Profiles.Secondary;

/// <summary>
/// Represents the banking details of a tutor without specifying the account type.
/// </summary>
public class TutorBankingDetailsWithoutType
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
    /// Gets or sets the bank account type of the tutor (as an integer).
    /// </summary>
    public int BankAccountType { get; set; }
}