```csharp
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GradeController : ControllerBase
    {
        private readonly IGradeService _gradeService;

        public GradeController(IGradeService gradeService)
        {
            _gradeService = gradeService;
        }

        [HttpPost]
        public async Task<IActionResult> AddGrade([FromBody]AddGradeRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var validationResults = ValidateRequest(request);

            if (validationResults.Any())
            {
                return BadRequest(validationResults);
            }

            try
            {
                var result = await _gradeService.AddGradeAsync(request);

                if (result == null)
                {
                    return NotFound();
                }

                return CreatedAtAction(nameof(AddGrade), new { id = result.Id }, result);
            }
            catch (Exception ex) when (ex is ArgumentException || ex is InvalidOperationException)
            {
                return BadRequest(ex.Message);
            }
        }

        private IActionResult ValidateRequest(AddGradeRequest request)
        {
            if (request.StudentId <= 0)
            {
                ModelState.AddModelError(nameof(request.StudentId), "Student ID must be greater than zero.");
            }

            if (string.IsNullOrWhiteSpace(request.SubjectName))
            {
                ModelState.AddModelError(nameof(request.SubjectName), "Subject name is required.");
            }

            if (request.GradeValue < 0.00m || request.GradeValue > 100.00m)
            {
                ModelState.AddModelError(nameof(request.GradeValue), "Grade value must be between 0 and 100.");
            }

            return ModelState.IsValid ? null : BadRequest(ModelState);
        }
    }

    public class AddGradeRequest
    {
        [Required]
        public int StudentId { get; set; }

        [Required]
        public string SubjectName { get; set; }

        [Range(0.00, 100.00)]
        public decimal GradeValue { get; set; }
    }

    public interface IGradeService
    {
        Task<AddGradeResponse> AddGradeAsync(AddGradeRequest request);
    }

    public class AddGradeResponse
    {
        public int Id { get; set; }
        public string Message { get; set; }
    }
}
```