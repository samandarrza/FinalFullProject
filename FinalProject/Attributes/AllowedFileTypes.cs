using System.ComponentModel.DataAnnotations;

namespace FinalProject.Attributes
{
    public class AllowedFileTypes : ValidationAttribute
    {
        string[] _fileTypes;
        public AllowedFileTypes(params string[] fileTypes)
        {
            _fileTypes = fileTypes;
        }
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            List<IFormFile> files = new List<IFormFile>();
            if (value is IFormFile)
            {
                var file = value as IFormFile;
                files.Add(file);
            }
            else if (value is List<IFormFile>)
            {
                files = value as List<IFormFile>;
            }

            if (files != null)
            {
                foreach (var file in files)
                {
                    if (!_fileTypes.Contains(file.ContentType))
                        return new ValidationResult("File ContentType " + string.Join(", ", _fileTypes) + " olmalıdır");
                }
            }
            return ValidationResult.Success;
        }
    }

}
