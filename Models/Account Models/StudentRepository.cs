namespace CapstoneProject.Models.Account_Models
{
    public class StudentRepository
    {

        private static List<StudentInformation> _students = new List<StudentInformation>();

        public static void addStudent(StudentInformation si)
        {
            _students.Add(si);
        }

        public static IEnumerable<StudentInformation> GetStudents => _students;
    }
}
