using System.Net.Http.Headers;

namespace FileValidatorApi.Tests;

[UsesVerify]
public class FileValidatorTests
{
    private readonly FileValidatorApplication app;
    private readonly HttpClient httpClient;

    public FileValidatorTests()
    {
        app = new FileValidatorApplication();
        httpClient = app.CreateClient();
    }
    
    [Fact]
    public async Task Should_Have_Invalid_Lines()
    {
        const string content = "Thomas 32999921\n" +
                               "Richard 3293982\n" +
                               "XAEA-12 8293982\n" +
                               "Rose 329a982\n" +
                               "Bob 329398.\n" +
                               "michael 3113902\n" +
                               "Rob 3113902p\n";
        
        const string fileName = "file.txt";
        var tempFilePath = Path.Combine(Path.GetTempPath(), fileName);

        await File.WriteAllTextAsync(tempFilePath, content);

        await using var fileStream = new FileStream(tempFilePath, FileMode.Open);
        var fileContent = new StreamContent(fileStream);
        fileContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
        {
            Name = "file",
            FileName = fileName,
        };

        var multipartContent = new MultipartFormDataContent();
        multipartContent.Add(fileContent);
        

        var response = await httpClient.PostAsync("/api/validation/upload", multipartContent);
        var responseContent = await response.Content.ReadAsStringAsync();
        
        await VerifyJson(responseContent);
    }
    
    [Fact]
    public async Task Should_Have_No_Invalid_Lines()
    {
        const string content = "Richard 3293982\n" +
                               "Rob 3113902\n";
        
        const string fileName = "file.txt";
        var tempFilePath = Path.Combine(Path.GetTempPath(), fileName);

        await File.WriteAllTextAsync(tempFilePath, content);

        await using var fileStream = new FileStream(tempFilePath, FileMode.Open);
        var fileContent = new StreamContent(fileStream);
        fileContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
        {
            Name = "file",
            FileName = fileName,
        };

        var multipartContent = new MultipartFormDataContent();
        multipartContent.Add(fileContent);

        var response = await httpClient.PostAsync("/api/validation/upload", multipartContent);
        var responseContent = await response.Content.ReadAsStringAsync();
        
        await VerifyJson(responseContent);
    }
}
