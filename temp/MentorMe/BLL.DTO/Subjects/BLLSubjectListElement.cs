using Base.DAL.Contracts;

namespace BLL.DTO.Subjects;

// ReSharper disable once InconsistentNaming
public class BLLSubjectListElement: IDomainEntityId
{
    public Guid Id { get; set; }

    public string Name { get; set; } = default!;
    
    public byte[]? SubjectPicture { get; set; }
}