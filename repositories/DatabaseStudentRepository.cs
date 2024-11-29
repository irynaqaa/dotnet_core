```csharp
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace StudentManagementSystem.Repositories
{
    public class DatabaseStudentRepository : IStudentRepository
    {
        private readonly DbContext _context;

        public DatabaseStudentRepository(DbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public IEnumerable<Student> GetAllStudents()
        {
            try
            {
                var students = _context.Set<Student>().ToList();
                return students;
            }
            catch (Exception ex) when (!(ex is DbUpdateException || ex is DbUpdateConcurrencyException))
            {
                throw new Exception("Failed to retrieve students from database", ex);
            }
        }
    }

    public interface IStudentRepository
    {
        IEnumerable<Student> GetAllStudents();
    }

    public class Student
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public DateTime DateOfBirth { get; set; }
    }
}
```