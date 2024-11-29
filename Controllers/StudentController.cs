```csharp
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using StudentApi.Services;
using StudentApi.Models;

namespace StudentApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StudentController : ControllerBase
    {
        private readonly IStudentService _studentService;
        private readonly ILogger<StudentController> _logger;

        public StudentController(IStudentService studentService, ILogger<StudentController> logger)
        {
            _studentService = studentService ?? throw new ArgumentException(nameof(studentService), "IStudentService instance is required.");
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpPost]
        public async Task<IActionResult> CreateStudent([FromBody] StudentDTO studentDto)
        {
            try
            {
                if (studentDto == null)
                {
                    return BadRequest("Student DTO is required.");
                }

                var existingStudent = await _studentService.GetStudentAsync(studentDto.Email);
                if (existingStudent != null)
                {
                    return Conflict($"A student with email '{studentDto.Email}' already exists.");
                }

                var createdStudent = await _studentService.CreateStudentAsync(studentDto);
                if (createdStudent != null)
                {
                    _logger.LogInformation("Created new student with id {Id}", createdStudent.Id);
                    return CreatedAtAction(nameof(CreateStudent), new { id = createdStudent.Id }, createdStudent);
                }
                else
                {
                    throw new InvalidOperationException("Failed to create student.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating a new student");
                return StatusCode(500, "Internal Server Error");
            }
        }
    }
}
```