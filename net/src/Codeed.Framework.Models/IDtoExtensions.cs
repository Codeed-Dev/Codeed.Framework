using System.ComponentModel.DataAnnotations;

namespace Codeed.Framework.Models
{
    public static class IDtoExtensions
    {
        public static void Validate(this IDto dto)
        {
            var context = new ValidationContext(dto, null, null);
            Validator.ValidateObject(dto, context);
        }
    }
}
