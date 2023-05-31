using App.DAL.Contracts;
using Base.DAL.EF;
using Microsoft.EntityFrameworkCore;
using Public.DTO.v1.Profiles.Secondary;
using TutorAvailability = Domain.Entities.TutorAvailability;

namespace App.DAL.EF.Repositories;

public class AvailabilityRepository: EFBaseRepository<TutorAvailability, ApplicationDbContext>, IAvailabilityRepository
{
    public AvailabilityRepository(ApplicationDbContext dataContext) : base(dataContext)
    {
    }

    public async Task<IEnumerable<TutorAvailability>> GetAllById(Guid tutorId) 
        => await RepositoryDbSet
            .Include(a => a.Tutor)
            .Where(a => a.Tutor!.AppUserId == tutorId)
            .ToListAsync();
    

    public async Task DeleteById(Guid availabilityId)
    {
        var availability = await RepositoryDbSet
            .FirstOrDefaultAsync(a => a.Id == availabilityId);

        if (availability != null)
        {
            RepositoryDbSet.Remove(availability);
            await RepositoryDbContext.SaveChangesAsync();
        }
    }

    public async Task AddNewAvailability(NewAvailability newAvailability, Guid tutorId)
{
    var tutor = await RepositoryDbContext.Tutors
        .Include(t => t.Availabilities)
        .FirstOrDefaultAsync(t => t.AppUserId == tutorId);
    
    if(tutor == null)
    {
        throw new Exception("Tutor not found");
    }

    var newFromHours = TimeSpan.Parse(newAvailability.FromHours);
    var newToHours = TimeSpan.Parse(newAvailability.ToHours);
    
    var overlappingAvailabilities = tutor.Availabilities?
        .Where(a => a.DayOfTheWeek == newAvailability.DayOfTheWeek && 
                    newFromHours <= a.ToHours && newToHours >= a.FromHours)
        .ToList();
    
    if (overlappingAvailabilities != null && overlappingAvailabilities.Count > 0)
    {
        var earliestFromHours = overlappingAvailabilities.Min(a => a.FromHours);
        var latestToHours = overlappingAvailabilities.Max(a => a.ToHours);

        earliestFromHours = new TimeSpan(Math.Min(newFromHours.Ticks, earliestFromHours.Ticks));
        latestToHours = new TimeSpan(Math.Max(newToHours.Ticks, latestToHours.Ticks));
        
        foreach (var overlappingAvailability in overlappingAvailabilities)
        {
            tutor.Availabilities!.Remove(overlappingAvailability);
            RepositoryDbSet.Remove(overlappingAvailability);
        }

        var availability = new TutorAvailability
        {
            FromHours = earliestFromHours,
            ToHours = latestToHours,
            DayOfTheWeek = newAvailability.DayOfTheWeek,
            TutorId = tutor.Id
        };

        tutor.Availabilities!.Add(availability);
        RepositoryDbSet.Add(availability);
    }
    else
    {
        var availability = new TutorAvailability
        {
            FromHours = newFromHours,
            ToHours = newToHours,
            DayOfTheWeek = newAvailability.DayOfTheWeek,
            TutorId = tutor.Id
        };

        tutor.Availabilities ??= new List<TutorAvailability>();
        tutor.Availabilities.Add(availability);
        RepositoryDbSet.Add(availability);
    }
    
    await RepositoryDbContext.SaveChangesAsync();
}
}