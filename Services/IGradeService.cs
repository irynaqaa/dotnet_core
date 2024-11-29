```csharp
using System;
using System.ComponentModel.DataAnnotations;

namespace Services
{
    public interface IGradeService
    {
        bool AddGrade(GradeDTO grade);
    }

    public class GradeDTO
    {
        [Required(ErrorMessage = "Id is required and must be greater than 0")]
        [Range(1, int.MaxValue, ErrorMessage = "Id must be a positive integer")]
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [StringLength(50, ErrorMessage = "Name cannot exceed 50 characters")]
        [MinLength(1, ErrorMessage = "Name must be at least 1 character")]
        public string Name { get; set; }

        [Range(0.0, 100.0, ErrorMessage = "Grade value must be between 0 and 100")]
        public double GradeValue { get; set; }
    }
}
```