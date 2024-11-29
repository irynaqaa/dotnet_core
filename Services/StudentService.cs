```csharp
using System;
using System.Collections.Generic;

namespace University.Services
{
    public class StudentService : IStudentService
    {
        private readonly IStudentRepository _studentRepository;
        private readonly IRepository _repository;

        public StudentService(IStudentRepository studentRepository, IRepository repository)
        {
            _studentRepository = studentRepository ?? throw new ArgumentNullException(nameof(studentRepository));
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public List<Student> GetAllStudents()
        {
            return _studentRepository.GetAll().ToList();
        }

        public void CreateStudent(StudentDTO studentDto)
        {
            if (studentDto == null)
            {
                throw new ArgumentNullException(nameof(studentDto));
            }

            var existingStudent = _studentRepository.GetStudentByEmail(studentDto.Email);

            if (existingStudent != null)
            {
                throw new InvalidOperationException("A student with the same email already exists.");
            }

            var student = new Student
            {
                FirstName = studentDto.FirstName,
                LastName = studentDto.LastName,
                Email = studentDto.Email
            };

            _repository.Create(student);
        }

        public List<StudentDTO> GetAllStudentsWithGrades()
        {
            var students = _studentRepository.GetAll();

            var result = new List<StudentDTO>();

            foreach (var student in students)
            {
                var grades = _studentRepository.GetGradesByStudentId(student.Id);

                if (grades != null)
                {
                    result.Add(new StudentDTO
                    {
                        Id = student.Id,
                        FirstName = student.FirstName,
                        LastName = student.LastName,
                        Email = student.Email,
                        Grades = grades
                    });
                }
            }

            return result;
        }

        public void CreateStudentWithGrades(StudentDTO studentDto)
        {
            if (studentDto == null)
            {
                throw new ArgumentNullException(nameof(studentDto));
            }

            var existingStudent = _studentRepository.GetStudentByEmail(studentDto.Email);

            if (existingStudent != null)
            {
                throw new InvalidOperationException("A student with the same email already exists.");
            }

            var student = new Student
            {
                FirstName = studentDto.FirstName,
                LastName = studentDto.LastName,
                Email = studentDto.Email
            };

            _repository.Create(student);

            foreach (var grade in studentDto.Grades)
            {
                _studentRepository.CreateGrade(grade);
            }
        }

        public StudentDTO GetStudentWithGrades(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(id));
            }

            var student = _studentRepository.GetStudentById(id);

            if (student == null)
            {
                throw new InvalidOperationException($"Student with ID {id} not found.");
            }

            var grades = _studentRepository.GetGradesByStudentId(id);

            if (grades == null)
            {
                throw new InvalidOperationException($"Grades for student with ID {id} not found.");
            }

            return new StudentDTO
            {
                Id = student.Id,
                FirstName = student.FirstName,
                LastName = student.LastName,
                Email = student.Email,
                Grades = grades
            };
        }
    }
}
```