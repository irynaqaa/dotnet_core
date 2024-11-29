```csharp
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace StudentManagementSystem.Repositories
{
    public class StudentRepository : IStudentRepository
    {
        private readonly StudentContext _context;

        public StudentRepository(StudentContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public Student GetStudentById(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("Invalid student ID", nameof(id));
            }
            return _context.Students.Find(id);
        }

        public List<Grade> GetGradesByStudentId(int studentId)
        {
            if (studentId <= 0)
            {
                throw new ArgumentException("Invalid student ID", nameof(studentId));
            }
            var student = _context.Students.Find(studentId);

            if (student == null)
            {
                throw new InvalidOperationException($"A student with ID {studentId} does not exist");
            }

            return student.Grades?.ToList() ?? new List<Grade>();
        }

        public void AddStudent(Student student)
        {
            if (student is null)
            {
                throw new ArgumentNullException(nameof(student));
            }
            if (_context.Students.Any(s => s.Name == student.Name))
            {
                throw new InvalidOperationException($"A student with name {student.Name} already exists");
            }
            _context.Students.Add(student);
        }

        public void SaveChanges()
        {
            try
            {
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while saving changes: {ex.Message}");
                throw;
            }
        }
    }

    public interface IStudentRepository
    {
        Student GetStudentById(int id);
        List<Grade> GetGradesByStudentId(int studentId);
        void AddStudent(Student student);
        void SaveChanges();
    }

    public class StudentContext : DbContext
    {
        public DbSet<Student> Students { get; set; }
        public DbSet<Grade> Grades { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var connectionString = Environment.GetEnvironmentVariable("StudentManagementSystemDBConnectionString");
            optionsBuilder.UseSqlServer(connectionString);
        }
    }

    public class Student
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<Grade> Grades { get; set; }
    }

    public class Grade
    {
        public int Id { get; set; }
        public int StudentId { get; set; }
        public decimal Score { get; set; }
        public Student Student { get; set; }
    }
}
```