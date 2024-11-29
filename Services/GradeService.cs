```csharp
using System;

namespace Services
{
    public class GradeService : IGradeService
    {
        private readonly IGradeRepository _gradeRepository;

        public GradeService(IGradeRepository gradeRepository)
        {
            if (gradeRepository == null) throw new ArgumentNullException(nameof(gradeRepository));

            _gradeRepository = gradeRepository;
        }

        public bool AddGrade(Grade grade)
        {
            return _gradeRepository.AddGrade(grade);
        }
    }

    public interface IGradeService
    {
        bool AddGrade(Grade grade);
    }
}
```