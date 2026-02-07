namespace App.DAL.EF.Seeding;

public class DataGuids
{
    public readonly Guid StudentId = Guid.NewGuid();
    
    public readonly Guid Tutor1Id = Guid.NewGuid();
    public readonly Guid Tutor2Id = Guid.NewGuid(); 
    public readonly Guid Tutor3Id = Guid.NewGuid(); 
    public readonly Guid Tutor4Id = Guid.NewGuid(); 
    public readonly Guid Tutor5Id = Guid.NewGuid(); 
    public readonly Guid StudentTutorAccountId = Guid.NewGuid();

    public readonly Guid SubjectMathId = Guid.NewGuid(); 
    public readonly Guid SubjectFrontEndId = Guid.NewGuid(); 
    public readonly Guid SubjectBackendId = Guid.NewGuid(); 
    public readonly Guid SubjectEnglishId = Guid.NewGuid(); 
    public readonly Guid SubjectPhysicsId = Guid.NewGuid();
    
    public readonly Guid Lesson1Id = Guid.NewGuid(); 
    public readonly Guid Lesson2Id = Guid.NewGuid();
    public readonly Guid Lesson3Id = Guid.NewGuid();
    public readonly Guid Lesson4Id = Guid.NewGuid(); 
    public readonly Guid Lesson5Id = Guid.NewGuid(); 
    public readonly Guid Lesson6Id = Guid.NewGuid(); 
    
    public readonly Guid StudentPaymentMethod1Id = Guid.NewGuid(); 
    public readonly Guid StudentPaymentMethod2Id = Guid.NewGuid(); 
}