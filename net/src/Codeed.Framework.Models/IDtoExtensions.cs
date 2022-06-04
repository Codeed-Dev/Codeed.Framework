using Codeed.Framework.Models;
using System.ComponentModel.DataAnnotations;

namespace System
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
