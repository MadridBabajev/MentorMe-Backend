using AutoMapper;
using Base.DAL;
using BLL.DTO.Profiles;
using DomainStudent = Domain.Entities.Student;

namespace App.BLL.Mappers;

public class StudentProfileMapper: BaseMapper<BLLStudentProfile, DomainStudent>
{
    public StudentProfileMapper(IMapper mapper) : base(mapper)
    {
    }
}