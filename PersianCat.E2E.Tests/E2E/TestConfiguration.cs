namespace PersianCat.E2E.Tests.E2E;

public class TestConfiguration
{
    public string SutUrl { get; set; } = Environment.GetEnvironmentVariable("SUT_URL") ?? "http://localhost:8080";


    public static string GetSolutionDirectory()
    {
        var currentDirectory = new DirectoryInfo(AppContext.BaseDirectory);
        while (currentDirectory != null && !currentDirectory.GetFiles("*.sln").Any())
        {
            currentDirectory = currentDirectory.Parent;
        }

        return currentDirectory?.FullName ?? throw new Exception("Solution directory not found.");
    }
}