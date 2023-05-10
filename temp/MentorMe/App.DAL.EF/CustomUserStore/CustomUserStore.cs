// using Domain.Identity;
// using Microsoft.AspNetCore.Identity;
// using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
// using Microsoft.EntityFrameworkCore;

// namespace App.DAL.EF.CustomUserStore;
//
// public class CustomUserStore : UserStore<AppUser, AppRole, ApplicationDbContext, Guid>
// {
//     public CustomUserStore(ApplicationDbContext context, IdentityErrorDescriber describer = null) 
//         : base(context, describer)
//     {
//     }
//
//     protected Task<bool> UserAlreadyHasUserNameAsync(AppUser user, string userName, CancellationToken cancellationToken = default)
//     {
//         return Users.AnyAsync(u => u.UserName == userName && u.AppUserType == user.AppUserType, cancellationToken);
//     }
//
//     protected Task<bool> UserAlreadyHasEmailAsync(AppUser user, string email, CancellationToken cancellationToken = default)
//     {
//         return Users.AnyAsync(u => u.Email == email && u.AppUserType == user.AppUserType, cancellationToken);
//     }
// }