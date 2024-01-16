using System.ComponentModel.DataAnnotations;

namespace Demo.Models.Validations
{
    public class Shrit_EnsureCorrectSizingAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
           var shirt = validationContext.ObjectInstance as Shirt;
            if (shirt != null)
            {
                if(shirt.Gender.Equals("men", StringComparison.OrdinalIgnoreCase) && shirt.Size < 8)
                {
                    return new ValidationResult("For men's shirt, the size has to be greater or qual 8");
                }
                if (shirt.Gender.Equals("Women", StringComparison.OrdinalIgnoreCase) && shirt.Size < 6)
                {
                    return new ValidationResult("For women's shirt, the size has to be greater or qual 6");
                }
            }
            return ValidationResult.Success;
        }
    }
}
