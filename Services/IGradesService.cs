```csharp
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services
{
    public interface IGradesService
    {
        Task<IEnumerable<Grade>> GetGradesForStudentAsync(int studentId);
    }

    public class Grade
    {
        public int Id { get; set; }
        public string Subject { get; set; }
        public double Score { get; set; }
    }

    public sealed class GradesService : IGradesService
    {
        private readonly Dictionary<int, List<Grade>> _grades = new Dictionary<int, List<Grade>>();

        public async Task<IEnumerable<Grade>> GetGradesForStudentAsync(int studentId)
        {
            if (studentId <= 0)
                return Enumerable.Empty<Grade>();

            if (_grades.TryGetValue(studentId, out var grades))
                return await Task.Run(() => grades);

            return Enumerable.Empty<Grade>();
        }
    }
}
```