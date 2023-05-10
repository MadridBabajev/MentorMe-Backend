using Base.BLL.Contracts;

namespace App.BLL.Contracts;

// ReSharper disable once InconsistentNaming
public interface IAppBLL: IBaseBLL
{
    ITutorsService TutorsService { get; }
    ISubjectsService SubjectsService { get; }
}