using System.Text.RegularExpressions;
using FileValidatorApi.Contracts;
using FileValidatorApi.Helpers;

namespace FileValidatorApi.Services;

public interface IValidationService
{
    Task<ValidationResult> ValidateAccountsFileAsync(IFormFile file);
}

public class ValidationService : IValidationService
{
    private readonly ITimeMeasurementService timeMeasurementService;

    public ValidationService(ITimeMeasurementService timeMeasurementService)
    {
        this.timeMeasurementService = timeMeasurementService;
    }

    public async Task<ValidationResult> ValidateAccountsFileAsync(IFormFile file)
    {
        var validationResult = new ValidationResult
        {
            FileValid = true,
            InvalidLines = new List<string>()
        };

        var lineNumber = 1;

        using var streamReader = new StreamReader(file.OpenReadStream());
        while (!streamReader.EndOfStream)
        {
            timeMeasurementService.Start();
            
            var line = await streamReader.ReadLineAsync();
            var parts = line.Split(' ');
            var invalidObjectNames = new List<string>();

            if (!IsNameValid(parts[0]))
            {
                invalidObjectNames.Add("account name");
            }
            
            if (!IsAccountNumberValid(parts[1]))
            {
                invalidObjectNames.Add("account number");
            }
            
            if (invalidObjectNames.Any())
            {
                var invalidObjects = StringFormatter.ListToFormattedString(invalidObjectNames, ',');
                validationResult.InvalidLines.Add($"{invalidObjects} - not valid for {lineNumber} line '{line}'");
            }

            lineNumber++;
            
            timeMeasurementService.Stop(line);
        }
        
        validationResult.FileValid = !validationResult.InvalidLines.Any();

        return validationResult;
    }
    
    private static bool IsNameValid(string name)
    {
        return Regex.IsMatch(name, @"^[A-Z][a-z]*$");
    }

    private static bool IsAccountNumberValid(string accountNumber)
    {
        return Regex.IsMatch(accountNumber, @"^(3|4)\d{6}(p)?$");
    }
}