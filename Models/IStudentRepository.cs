using CapstoneProject.Models.Account_Models;

namespace CapstoneProject.Models
{
    public interface IStudentRepository
    {
        IQueryable<StudentInformation> GetAllStudents { get; }
        public void AddStudent(StudentInformation student);
    }
}
