using System;
using System.Threading.Tasks;
using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Containers;
using DotNet.Testcontainers.Images;
using Microsoft.Extensions.Logging;

namespace PersianCat.E2E.Tests.E2E;

public class ResumeAppContainer : IAsyncDisposable
{
    private readonly IContainer _container;
    private readonly int _hostPort;

    private ResumeAppContainer(IContainer container, int hostPort)
    {
        _container = container;
        _hostPort = hostPort;
    }

    public string GetBaseUrl() => $"http://localhost:{_hostPort}";

    public static async Task<ResumeAppContainer> StartAsync(ILogger logger)
    {
        var dockerFileDirectoryPath = Path.Combine(PathHelper.FindSlnDirectoryPath(), "PersianCat.Resume");
        if (!Directory.Exists(dockerFileDirectoryPath))
        {
            throw new DirectoryNotFoundException($"Dockerfile directory not found: {dockerFileDirectoryPath}");
        }

        logger.LogInformation("Building Resume app image from Dockerfile in {Directory}", dockerFileDirectoryPath);

        const string imageName = "persiancat-resume:latest";

        // Build the image if it doesn't exist
        var image = new ImageFromDockerfileBuilder()
            .WithDockerfileDirectory(dockerFileDirectoryPath)
            .WithDockerfile("Dockerfile")
            .WithName(imageName)
            .WithDeleteIfExists(true)
            .Build();

        await image.CreateAsync();

        logger.LogInformation("Resume app image built successfully");

        var container = new ContainerBuilder()
            .WithImage(imageName)
            .WithPortBinding(80, true)
            .WithWaitStrategy(Wait.ForUnixContainer().UntilPortIsAvailable(80))
            .WithAutoRemove(true)
            .WithCleanUp(true)
            .Build();

        await container.StartAsync();
        var hostPort = container.GetMappedPublicPort(80);
        logger.LogInformation("Starting Resume app container on port {Port}", hostPort);

        logger.LogInformation("Resume app container started successfully");

        // Remove docker images using docker image prune -f command
        var pruneCommand = "docker image prune -f";
        var pruneProcess = new System.Diagnostics.Process
        {
            // Check if OS is Windows, otherwise use "sh" for Unix-like systems
            StartInfo = new System.Diagnostics.ProcessStartInfo
            {
                FileName = Environment.OSVersion.Platform == PlatformID.Win32NT ? "cmd.exe" : "sh",
                Arguments = Environment.OSVersion.Platform == PlatformID.Win32NT ? $"/c {pruneCommand}" : $"-c \"{pruneCommand}\"",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            }
        };

        pruneProcess.Start();

        string output = await pruneProcess.StandardOutput.ReadToEndAsync();
        string error = await pruneProcess.StandardError.ReadToEndAsync();
        await pruneProcess.WaitForExitAsync();

        logger.LogInformation("Docker image prune output: {Output}", output);

        if (pruneProcess.ExitCode != 0)
        {
            logger.LogError("Failed to prune Docker images: {Error}", error);
        }
        else
        {
            logger.LogInformation("Docker images pruned successfully: {Output}", output);
        }

        return new ResumeAppContainer(container, hostPort);
    }

    public async ValueTask DisposeAsync()
    {
        await _container.DisposeAsync();
        GC.SuppressFinalize(this);
    }
}
