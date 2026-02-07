using System.ComponentModel.DataAnnotations;
using Base.DAL.Contracts;

namespace Base.DomainEntity;

public class BaseRefreshToken : BaseRefreshToken<Guid>
{
}

// ReSharper disable InconsistentNaming
public class BaseRefreshToken<TKey>: IDomainEntityId<TKey>
    where TKey : struct, IEquatable<TKey>
{
    public TKey Id { get; set; }
    
    [MaxLength(64)]
    public string RefreshToken { get; set; } = Guid.NewGuid().ToString();
    public DateTime ExpirationDT { get; set; } = DateTime.UtcNow.AddDays(7);

    [MaxLength(64)]
    public string? PreviousRefreshToken { get; set; } 
    // UTC
    public DateTime? PreviousExpirationDT { get; set; } = DateTime.UtcNow.AddDays(7);

}