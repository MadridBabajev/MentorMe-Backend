using App.DAL.Contracts;
using Base.DAL.EF;
using Microsoft.EntityFrameworkCore;
using Public.DTO.v1.Profiles.Secondary;
using StudentPaymentMethod = Domain.Entities.StudentPaymentMethod;

namespace App.DAL.EF.Repositories;

public class PaymentMethodRepository: EFBaseRepository<StudentPaymentMethod, ApplicationDbContext>, IPaymentMethodRepository
{
    public PaymentMethodRepository(ApplicationDbContext dataContext) : base(dataContext)
    {
    }
    
    public async Task<IEnumerable<StudentPaymentMethod>> GetAllById(Guid userId) 
        => await RepositoryDbSet
            .Include(stm => stm.Student)
                .ThenInclude(s => s!.AppUser)
            .Where(spm => spm.Student!.AppUserId == userId)
            .ToListAsync();
    

    public async Task DeleteById(Guid paymentMethodId)
    {
        var paymentMethod = await RepositoryDbSet
            .FirstOrDefaultAsync(spm => spm.Id == paymentMethodId);

        if (paymentMethod != null)
        {
            RepositoryDbSet.Remove(paymentMethod);
            await RepositoryDbContext.SaveChangesAsync();
        }
    }

    public async Task AddNewPaymentMethod(NewPaymentMethod newPaymentMethod, Guid studentId)
    {
        var student = await RepositoryDbContext.Students
            .Include(s => s.PaymentMethods)
            .FirstOrDefaultAsync(s => s.AppUserId == studentId);
        if(student == null)
        {
            throw new Exception("Student not found");
        }

        var paymentMethod = new StudentPaymentMethod
        {
            Details = newPaymentMethod.Details,
            CardCvv = newPaymentMethod.CardCvv,
            CardExpirationDate = newPaymentMethod.CardExpirationDate,
            CardNumber = newPaymentMethod.CardNumber,
            StudentId = student.Id
        };

        student.PaymentMethods ??= new List<StudentPaymentMethod>();
        student.PaymentMethods.Add(paymentMethod);
        RepositoryDbSet.Add(paymentMethod);
        
        await RepositoryDbContext.SaveChangesAsync();
    }
}