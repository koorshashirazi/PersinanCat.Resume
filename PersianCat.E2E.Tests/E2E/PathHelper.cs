public sealed class PathHelper
{

    public static string FindSlnDirectoryPath()
    {
        var slnDirectoryPath = string.Empty;
        var currentPath = Directory.GetCurrentDirectory();

        while (string.IsNullOrEmpty(slnDirectoryPath))
        {
            var directoryInfo = Directory.GetParent(currentPath);
            currentPath = directoryInfo?.FullName;
            slnDirectoryPath = directoryInfo?.GetFiles("PersianCat.sln", SearchOption.TopDirectoryOnly).FirstOrDefault()
                ?.DirectoryName;
        }

        return slnDirectoryPath;
    }
    public static string GetPathToSlnFile()
    {
        var slnDirectoryPath = FindSlnDirectoryPath();
        if (string.IsNullOrEmpty(slnDirectoryPath))
        {
            throw new InvalidOperationException("Could not find the solution directory.");
        }
        return Path.Combine(slnDirectoryPath, "PersianCat.sln");
    }
}