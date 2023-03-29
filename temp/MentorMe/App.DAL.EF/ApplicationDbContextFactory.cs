using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace App.DAL.EF;

// public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
// {
//     public ApplicationDbContext CreateDbContext(string[] args)
//     {
//         var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
//         // does not actually connect to db
//         optionsBuilder.UseNpgsql("");
//         return new ApplicationDbContext(optionsBuilder.Options);
//     }
// }
