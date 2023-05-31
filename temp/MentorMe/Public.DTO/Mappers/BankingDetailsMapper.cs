using AutoMapper;
using Base.DAL;
using BLL.DTO.Profiles;
using Public.DTO.v1.Profiles.Secondary;

namespace Public.DTO.Mappers;

public class BankingDetailsMapper: BaseMapper<BLLTutorBankingDetails, TutorBankingDetails>
{
    public BankingDetailsMapper(IMapper mapper) : base(mapper)
    {
    }
}