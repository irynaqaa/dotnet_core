```csharp
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Controllers
{
    public class GetGradesForStudentController : ControllerBase
    {
        private readonly IGradesService _gradesService;

        public GetGradesForStudentController(IGradesService gradesService)
        {
            _gradesService = gradesService ?? throw new ArgumentNullException(nameof(gradesService));
        }

        [HttpGet]
        public async Task<IActionResult> GetGradesForStudent(int studentId)
        {
            var grades = await _gradesService.GetGradesForStudentAsync(studentId);
            return Ok(grades);
        }
    }
}
```