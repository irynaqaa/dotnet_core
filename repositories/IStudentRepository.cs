```csharp
using System;
using System.Collections.Generic;
using StudentManagement.Models;

namespace StudentManagement.Repositories
{
    public interface IStudentRepository
    {
        IEnumerable<Student> GetAllStudents();
    }
}
```