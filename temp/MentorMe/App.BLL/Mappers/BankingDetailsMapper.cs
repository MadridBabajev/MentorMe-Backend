using AutoMapper;
using Base.DAL;
using BLL.DTO.Profiles;
using DomainTutorBankingDetails = Domain.Entities.TutorBankingDetails;

namespace App.BLL.Mappers;

public class BankingDetailsMapper: BaseMapper<BLLTutorBankingDetails, DomainTutorBankingDetails>
{
    public BankingDetailsMapper(IMapper mapper) : base(mapper)
    {
    }
}