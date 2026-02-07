using App.DAL.Contracts;
using Base.DAL.EF;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Public.DTO.v1.Profiles;
using TutorAvailability = Domain.Entities.TutorAvailability;
using DomainTutorBankingDetails = Domain.Entities.TutorBankingDetails;
using TutorBankingDetailsPublic = Public.DTO.v1.Profiles.Secondary.TutorBankingDetails;

namespace App.DAL.EF.Repositories;

public class TutorsRepository: 
    EFBaseRepository<Tutor, ApplicationDbContext>, ITutorsRepository
{
    public TutorsRepository(ApplicationDbContext dataContext) : base(dataContext)
    {
    }
    
    public override async Task<IEnumerable<Tutor>> AllAsync() 
        => await RepositoryDbSet
            .OrderBy(e => e.CreatedAt)
            .ToListAsync();
    

    public async Task<Tutor> FindTutorById(Guid? userId)
    {
        return await RepositoryDbSet
            .Include(t => t.AppUser)
            .Include(t => t.TutorSubjects)!
                .ThenInclude(ts => ts.Subject)
            .Include(t => t.Availabilities)
            .Include(t => t.Reviews)
            .Include(t => t.Lessons)
            .FirstAsync(t => t.AppUserId == userId);
    }

    public async Task<List<TutorAvailability>> GetTutorAvailabilities(Guid? id)
    {
        if(id == null)
            throw new ArgumentNullException(nameof(id));

        var tutor = await RepositoryDbSet
            .Include(t => t.Availabilities)
            .FirstOrDefaultAsync(t => t.AppUserId == id);

        if(tutor == null)
            throw new Exception($"Tutor with id {id} not found.");

        return tutor.Availabilities!.ToList();
    }

    public async Task<DomainTutorBankingDetails> GetTutorBankingDetails(Guid tutorId) 
        => await RepositoryDbContext.TutorBankingDetails
            .Include(tbd => tbd.Tutor)
            .FirstAsync(tbd => tbd.TutorId == tutorId);
    

    public async Task UpdateBankingDetails(Guid tutorId, TutorBankingDetailsPublic updatedBankingDetails)
    {
        var tutor = await RepositoryDbSet
            .Include(t => t.BankingDetails)
            .FirstOrDefaultAsync(t => t.AppUserId == tutorId);
        
        if(tutor == null)
        {
            throw new Exception("Tutor not found");
        }

        tutor.BankingDetails ??= new TutorBankingDetails();

        tutor.BankingDetails.BankAccountNumber = updatedBankingDetails.BankAccountNumber;
        tutor.BankingDetails.BankAccountName = updatedBankingDetails.BankAccountName;
        tutor.BankingDetails.BankAccountType = updatedBankingDetails.BankAccountType;

        RepositoryDbSet.Update(tutor);
        await RepositoryDbContext.SaveChangesAsync();
    }

    public async Task<Tutor> GetTutorEditProfileData(Guid userId) 
        => (await RepositoryDbSet
            .Include(t => t.AppUser)
            .FirstOrDefaultAsync(t => t.AppUserId == userId))!;
    

    public async Task UpdateTutorProfileData(Guid tutorId, UpdatedProfileData updatedProfileData)
    {
        var tutor = await RepositoryDbSet
            .Include(t => t.AppUser)
            .FirstOrDefaultAsync(t => t.AppUserId == tutorId);

        if (tutor == null)
        {
            throw new Exception("Tutor not found");
        }
        
        tutor.AppUser!.FirstName = updatedProfileData.FirstName;
        tutor.AppUser.LastName = updatedProfileData.LastName;
        tutor.AppUser.MobilePhone = updatedProfileData.MobilePhone;
        tutor.AppUser.Title = updatedProfileData.Title;
        tutor.AppUser.Bio = updatedProfileData.Bio;
        tutor.AppUser.ProfilePicture = updatedProfileData.ProfilePicture;
        tutor.HourlyRate = updatedProfileData.HourlyRate ?? tutor.HourlyRate;
        
        RepositoryDbSet.Update(tutor);
        await RepositoryDbContext.SaveChangesAsync();
    }

    public async Task<IEnumerable<Tutor>> AllFilteredAsync(TutorSearchFilters filters)
    {
        var tutors = RepositoryDbSet.AsQueryable();

        if (!string.IsNullOrWhiteSpace(filters.FirstName))
        {
            tutors = tutors.Where(t => t.AppUser!.FirstName.Contains(filters.FirstName));
        }

        if (!string.IsNullOrWhiteSpace(filters.LastName))
        {
            tutors = tutors.Where(t => t.AppUser!.LastName.Contains(filters.LastName));
        }

        if (filters.MinHourlyRate.HasValue)
        {
            tutors = tutors.Where(t => t.HourlyRate >= filters.MinHourlyRate.Value);
        }

        if (filters.MaxHourlyRate.HasValue)
        {
            tutors = tutors.Where(t => t.HourlyRate <= filters.MaxHourlyRate.Value);
        }

        if (filters.MinAverageRating.HasValue)
        {
            tutors = tutors.Where(t => t.Reviews != null && t.Reviews.Any() && t.Reviews.Average(r => r.Rating) >= filters.MinAverageRating.Value);
        }

        if (filters.MaxAverageRating.HasValue)
        {
            tutors = tutors.Where(t => t.Reviews != null && t.Reviews.Any() && t.Reviews.Average(r => r.Rating) <= filters.MaxAverageRating.Value);
        }
        
        if (filters.MinClassesCount.HasValue)
        {
            tutors = tutors.Where(t => t.Lessons!.Count >= filters.MinClassesCount.Value);
        }

        if (filters.MaxClassesCount.HasValue)
        {
            tutors = tutors.Where(t => t.Lessons!.Count <= filters.MaxClassesCount.Value);
        }

        if (filters.SubjectIds!.Any())
        {
            tutors = tutors.Where(t => t.TutorSubjects!.Any(ts => filters.SubjectIds!.Contains(ts.SubjectId)));
        }

        return await tutors
            .Include(t => t.AppUser)
            .Include(t => t.Lessons)
            .Include(t => t.Reviews)
            .OrderBy(e => e.CreatedAt).ToListAsync();
    }
}
