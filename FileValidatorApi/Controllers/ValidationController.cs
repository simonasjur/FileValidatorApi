using FileValidatorApi.Contracts;
using FileValidatorApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace FileValidatorApi.Controllers;

[ApiController]
[Route("api/validation")]
public class ValidationController : ControllerBase
{
    private readonly IValidationService validationService;

    public ValidationController(IValidationService validationService)
    {
        this.validationService = validationService;
    }

    [HttpPost("upload")]
    public async Task<ValidationResult> UploadAndValidateFile(IFormFile file)
    {
        return await validationService.ValidateAccountsFileAsync(file);
    }
}