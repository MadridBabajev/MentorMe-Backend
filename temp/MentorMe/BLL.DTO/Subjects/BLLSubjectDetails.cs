using Base.DAL.Contracts;

namespace BLL.DTO.Subjects;

// ReSharper disable once InconsistentNaming
public class BLLSubjectDetails: IDomainEntityId
{
    public Guid Id { get; set; }

    public string Name { get; set; } = default!;
    
    public string Description { get; set; } = default!;
    
    public int TaughtBy { get; set; }
    
    public int LearnedBy { get; set; }
    
    public byte[]? SubjectPicture { get; set; }
    
    public bool? IsAdded { get; set; }
}