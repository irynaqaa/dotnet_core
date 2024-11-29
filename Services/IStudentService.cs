```csharp
using System.Threading.Tasks;

namespace Services
{
    public interface IStudentService
    {
        Task<Student> CreateStudentAsync(Student student);
    }

    public class Student
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public int Age { get; set; }
    }
}
```