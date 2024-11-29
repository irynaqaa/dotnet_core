```csharp
using Microsoft.AspNetCore.Mvc;
using System;

namespace StudentApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GetStudentWithGradesController : ControllerBase
    {
        private readonly IStudentService _studentService;

        public GetStudentWithGradesController(IStudentService studentService)
        {
            _studentService = studentService;
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            try
            {
                var result = _studentService.GetStudentWithGrades(id);

                if (result == null)
                {
                    return NotFound();
                }

                return Json(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
```