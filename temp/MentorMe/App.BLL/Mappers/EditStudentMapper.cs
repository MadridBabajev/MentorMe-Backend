using AutoMapper;
using Base.DAL;
using BLL.DTO.Profiles;
using Domain.Entities;

namespace App.BLL.Mappers;

public class EditStudentMapper: BaseMapper<BLLUpdatedProfileData, Student>
{
    public EditStudentMapper(IMapper mapper) : base(mapper)
    {
    }
}