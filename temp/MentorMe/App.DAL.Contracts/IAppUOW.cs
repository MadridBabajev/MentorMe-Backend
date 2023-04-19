namespace App.DAL.Contracts;

// ReSharper disable once InconsistentNaming
public interface IAppUOW
{
    // list all the repositories
    IStudentsRepository StudentsRepository { get; }
    ITutorsRepository TutorsRepository { get; }

    ILessonsRepository LessonsRepository { get; }
}

