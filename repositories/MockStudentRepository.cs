```csharp
using System;
using System.Collections.Generic;
using System.Linq;

namespace StudentManagementSystem.Repositories
{
    public class MockStudentRepository : IStudentRepository
    {
        private List<Student> _students = new List<Student>();

        public MockStudentRepository()
        {
            // Initialize mock data for testing purposes.
            _students.Add(new Student { Id = 1, Name = "John Doe", Email = "john.doe@example.com" });
            _students.Add(new Student { Id = 2, Name = "Jane Doe", Email = "jane.doe@example.com" });
        }

        public IEnumerable<Student> GetAllStudents()
        {
            return _students;
        }

        public Student GetStudentById(int id)
        {
            if (_students.Count == 0)
                throw new InvalidOperationException("No students found.");

            var student = _students.FirstOrDefault(s => s.Id == id);
            if (student == null)
                throw new KeyNotFoundException($"Student with ID {id} not found.");

            return student;
        }

        public void AddStudent(Student student)
        {
            if (student == null)
                throw new ArgumentNullException(nameof(student));

            if (string.IsNullOrWhiteSpace(student.Name))
                throw new ArgumentException("Name cannot be empty or whitespace.", nameof(student.Name));

            if (string.IsNullOrWhiteSpace(student.Email))
                throw new ArgumentException("Email cannot be empty or whitespace.", nameof(student.Email));

            var existingStudent = _students.FirstOrDefault(s => s.Id == student.Id);
            if (existingStudent != null)
                throw new InvalidOperationException($"A student with ID {student.Id} already exists.");

            _students.Add(student);
        }

        public void UpdateStudent(Student student)
        {
            if (student == null)
                throw new ArgumentNullException(nameof(student));

            if (string.IsNullOrWhiteSpace(student.Name))
                throw new ArgumentException("Name cannot be empty or whitespace.", nameof(student.Name));

            if (string.IsNullOrWhiteSpace(student.Email))
                throw new ArgumentException("Email cannot be empty or whitespace.", nameof(student.Email));

            var existingStudent = _students.FirstOrDefault(s => s.Id == student.Id);
            if (existingStudent == null)
                throw new KeyNotFoundException($"Student with ID {student.Id} not found.");

            if (student.Id <= 0)
                throw new ArgumentException("ID must be a positive integer.", nameof(student.Id));

            existingStudent.Name = student.Name;
            existingStudent.Email = student.Email;
        }

        public void DeleteStudent(int id)
        {
            var studentToDelete = _students.FirstOrDefault(s => s.Id == id);
            if (studentToDelete != null)
                _students.Remove(studentToDelete);
            else
                throw new KeyNotFoundException($"Student with ID {id} not found.");
        }
    }
}
```