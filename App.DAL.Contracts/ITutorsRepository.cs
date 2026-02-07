using Base.DAL.Contracts;
using BLL.DTO.Profiles;
using Domain.Entities;
using Public.DTO.v1.Profiles;
using TutorAvailability = Domain.Entities.TutorAvailability;
using DomainTutorBankingDetails = Domain.Entities.TutorBankingDetails;
using TutorBankingDetails = Public.DTO.v1.Profiles.Secondary.TutorBankingDetails;

namespace App.DAL.Contracts;

public interface ITutorsRepository : IBaseRepository<Tutor>, ILessonsRepositoryCustom<Tutor>
{
    // add here custom methods for repo only
    Task<IEnumerable<Tutor>> AllFilteredAsync(TutorSearchFilters filters);

    Task<Tutor> FindTutorById(Guid? userId);

    Task<List<TutorAvailability>> GetTutorAvailabilities(Guid? id);
    Task<DomainTutorBankingDetails> GetTutorBankingDetails(Guid tutorId);
    Task UpdateBankingDetails(Guid tutorId, TutorBankingDetails updatedBankingDetails);
    Task<Tutor> GetTutorEditProfileData(Guid userId);
    Task UpdateTutorProfileData(Guid tutorId, UpdatedProfileData updatedProfileData);
}

public interface ITutorsRepositoryCustom<TEntity>
{
    // add here shared methods between repo and service
    
    // Task<IEnumerable<TEntity>> AllAsync();
    
    // Task<TEntity?> FindAsync(Guid id, Guid userId);
    
    // Task<TEntity?> RemoveAsync(Guid id, Guid userId);
    
    // Task<bool> IsOwnedByUserAsync(Guid id, Guid userId);
}