```csharp
using Microsoft.AspNetCore.Mvc;
using System;

namespace Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CalculateAverageScoreController : ControllerBase
    {
        private readonly GradeService _gradeService;

        public CalculateAverageScoreController(GradeService gradeService)
        {
            _gradeService = gradeService ?? throw new ArgumentNullException(nameof(gradeService));
        }

        [HttpGet]
        public IActionResult GetAverageScore()
        {
            try
            {
                var averageScore = _gradeService.GetAverageScore();
                return Json(new { AverageScore = averageScore });
            }
            catch (ArgumentException ex)
            {
                return StatusCode(400, new { Error = "Invalid input", Message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Error = "An error occurred while calculating the average score", Message = ex.Message });
            }
        }
    }

    public class GradeService
    {
        private readonly double[] _scores;

        public GradeService(double[] scores)
        {
            _scores = scores ?? throw new ArgumentNullException(nameof(scores));
        }

        public double GetAverageScore()
        {
            if (_scores == null || _scores.Length == 0)
            {
                throw new ArgumentException("Cannot calculate average score for an empty set of scores");
            }

            var sum = 0.0;
            foreach (var score in _scores)
            {
                sum += score;
            }

            return sum / _scores.Length;
        }
    }
}
```