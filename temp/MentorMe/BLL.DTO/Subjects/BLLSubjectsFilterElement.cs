using Base.DAL.Contracts;

namespace BLL.DTO.Subjects;

public class BLLSubjectsFilterElement: IDomainEntityId
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
}