```csharp
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/[controller]/students")]
    public class GetAllStudentsController : ControllerBase
    {
        private readonly IStudentRepository _studentRepository;

        public GetAllStudentsController(IStudentRepository studentRepository)
        {
            _studentRepository = studentRepository;
        }

        [HttpGet]
        public IActionResult GetAllStudents()
        {
            if (_studentRepository == null)
            {
                return StatusCode(500);
            }

            var students = _studentRepository.GetAllStudents();

            if (students == null || !students.Any())
            {
                return NotFound();
            }

            return Json(students);
        }
    }
}
```