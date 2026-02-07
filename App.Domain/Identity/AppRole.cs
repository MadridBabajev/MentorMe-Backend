using Base.DAL.Contracts;
using Microsoft.AspNetCore.Identity;

namespace Domain.Identity;

public class AppRole: IdentityRole<Guid>, IDomainEntityId<Guid>
{
}
