namespace Base.DAL.Contracts;

// Can accept any ID types (exp. string, int, Guid)
public interface IDomainEntityId<TKey>
    where TKey : struct, IEquatable<TKey>
{
    TKey Id { get; set; }
}

public interface IDomainEntityId : IDomainEntityId<Guid>
{
}
