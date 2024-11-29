```csharp
using System;
using System.Collections.Generic;

namespace Repositories
{
    public interface IGradeRepository
    {
        IEnumerable<Grade> GetGrades();
        void AddGrade(Grade grade);
        void UpdateGrade(Grade grade);
    }

    public class Grade
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Value { get; set; }

        public Grade(int id, string name, double value)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Name cannot be null or empty", nameof(name));

            if (value < 0 || value > 100)
                throw new ArgumentOutOfRangeException(nameof(value), "Value must be between 0 and 100");

            Id = id;
            Name = name;
            Value = value;
        }
    }

    public class GradeRepository : IGradeRepository
    {
        private readonly List<Grade> _grades;

        public GradeRepository()
        {
            _grades = new List<Grade>();
        }

        public IEnumerable<Grade> GetGrades()
        {
            return _grades ?? Enumerable.Empty<Grade>();
        }

        public void AddGrade(Grade grade)
        {
            if (grade == null)
                throw new ArgumentNullException(nameof(grade), "Grade cannot be null");

            _grades.Add(grade);
        }

        public void UpdateGrade(Grade grade)
        {
            if (grade == null)
                throw new ArgumentNullException(nameof(grade), "Grade cannot be null");

            var existingGrade = _grades.Find(g => g.Id == grade.Id);

            if (existingGrade != null)
                _grades.Remove(existingGrade);

            _grades.Add(grade);
        }
    }
}
```