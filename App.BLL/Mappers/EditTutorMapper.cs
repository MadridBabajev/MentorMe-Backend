using AutoMapper;
using Base.DAL;
using BLL.DTO.Profiles;
using Domain.Entities;

namespace App.BLL.Mappers;

public class EditTutorMapper: BaseMapper<BLLUpdatedProfileData, Tutor>
{
    public EditTutorMapper(IMapper mapper) : base(mapper)
    {
    }
}