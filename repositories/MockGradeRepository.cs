```csharp
using System;
using System.Collections.Generic;

namespace repositories
{
    public class MockGradeRepository : IGradeRepository
    {
        private readonly List<Grade> grades = new List<Grade>
        {
            new Grade { Id = 1, Name = "Math", Score = 90 },
            new Grade { Id = 2, Name = "Science", Score = 85 },
            new Grade { Id = 3, Name = "English", Score = 95 }
        };

        public List<Grade> GetGrades()
        {
            return grades;
        }
    }

    public class Grade
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Score { get; set; }
    }

    public interface IGradeRepository
    {
        List<Grade> GetGrades();
    }
}
```