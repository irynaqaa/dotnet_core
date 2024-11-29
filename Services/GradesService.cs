```csharp
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace MySchool.Services
{
    public class GradesService : IGradesService
    {
        private readonly AppDbContext _context;

        public GradesService(AppDbContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            _context = context;
        }

        public async Task<List<Grade>> GetGradesForStudentAsync(int studentId)
        {
            try
            {
                return await _context.Grades
                    .Where(g => g.StudentId == studentId)
                    .ToListAsync();
            }
            catch (Exception ex) when (ex is DbUpdateException || ex is InvalidOperationException)
            {
                throw new ArgumentException("Invalid student ID or database error.", nameof(studentId), ex);
            }
        }
    }

    public interface IGradesService
    {
        Task<List<Grade>> GetGradesForStudentAsync(int studentId);
    }
}
```