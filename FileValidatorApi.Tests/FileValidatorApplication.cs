using Microsoft.AspNetCore.Mvc.Testing;

namespace FileValidatorApi.Tests;

internal class FileValidatorApplication : WebApplicationFactory<Program>, IDisposable
{
}