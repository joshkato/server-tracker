using System.Collections.Generic;

namespace ServerTracker.Data.Models
{
    public class ValidationResult
    {
        public bool IsValid { get; set; }

        public List<string> ValidationErrors { get; set; } = new List<string>();

        public void AddError(string error)
        {
            ValidationErrors?.Add(error);
        }
    }
}
