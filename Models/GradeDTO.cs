```csharp
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Models
{
    public class GradeDTO : IValidatableObject
    {
        [Required, Range(1, int.MaxValue)]
        public int? StudentId { get; set; }

        [Required, Range(1, int.MaxValue)]
        public int? CourseId { get; set; }

        [Range(0, 100), Required]
        public decimal? Grade { get; set; }

        public bool IsValid => Validate(null).Count() == 0;

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var results = new List<ValidationResult>();

            Validator.TryValidateObject(this, validationContext, results);

            if (StudentId <= 0)
                yield return new ValidationResult("Student ID must be greater than zero", new[] { nameof(StudentId) });

            if (CourseId <= 0)
                yield return new ValidationResult("Course ID must be greater than zero", new[] { nameof(CourseId) });

            if (Grade < 0 || Grade > 100)
                yield return new ValidationResult("Grade must be between 0 and 100", new[] { nameof(Grade) });
        }
    }
}
```