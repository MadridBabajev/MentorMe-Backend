using System.ComponentModel.DataAnnotations;
using Base.DAL.Contracts;
using Domain.Identity;

namespace BLL.DTO;

// ReSharper disable once InconsistentNaming
public class BLLTutorSearch : IDomainEntityId
{
    public Guid Id { get; set; }

    public string FirstName { get; set; } = default!;

    public string LastName { get; set; } = default!;
    
    public string Title { get; set; } = "";
    
    public byte[]? ProfilePicture { get; set; }

    public Guid AppUserId { get; set; }

    public AppUser? AppUser { get; set; }
}
