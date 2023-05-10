using App.DAL.Contracts;
using Base.DAL.Contracts;
using BLL.DTO;

namespace App.BLL.Contracts;

public interface ITutorsService: IBaseRepository<BLLTutorSearch>, ITutorsRepositoryCustom<BLLTutorSearch>
{
    // add your custom service methods here
}