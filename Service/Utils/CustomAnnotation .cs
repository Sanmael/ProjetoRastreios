using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Service.Utils
{
    public class CustomAnnotation : ValidationAttribute
    {
        public CustomAnnotation(string error)
        {
            ErrorMessage = error;
        }
        public override bool IsValid(object value)
        {
            if (value == null)
                return false;

            string input = value.ToString();
            
            if (input.Length < 10)
                return false;

            return Regex.IsMatch(input, @"^(?=.*[a-zA-Z])(?=.*\d).{10,}$");
        }
    }
}
