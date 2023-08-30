namespace FileValidatorApi.Contracts;

public class ValidationResult
{
    public bool FileValid { get; set; }
    public List<string> InvalidLines { get; set; }
}