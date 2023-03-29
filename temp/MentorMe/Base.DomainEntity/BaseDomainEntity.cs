using Base.DAL.Contracts;

namespace Base.DomainEntity;

// Base entity with Guid implementation
public abstract class BaseDomainEntity: DomainEntity<Guid>
{
}

public abstract class DomainEntity<TKey> : IDomainEntityId<TKey>
    where TKey : struct, IEquatable<TKey>
{
    public TKey Id { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public string? EntityComment { get; set; }
}
