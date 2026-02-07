using Base.DAL.Contracts;

namespace BLL.DTO.Lessons;

public class BLLTag: IDomainEntityId
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
    public string Description { get; set; } = default!;
    public DateTime AddedAt { get; set; }
}