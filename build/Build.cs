using Nuke.Common;
using Nuke.Common.IO;
using Nuke.Common.ProjectModel;
using Nuke.Common.Tools.DotNet;
using Nuke.Common.Tools.Docker;
using static Nuke.Common.Tools.Docker.DockerTasks;
using static Nuke.Common.Tools.DotNet.DotNetTasks;
using static Nuke.Common.Tools.Git.GitTasks;

class Build : NukeBuild
{
    public static int Main() => Execute<Build>(x => x.Test);

    [Parameter("Configuration to build - Default is 'Debug' (local) or 'Release' (server)")]
    readonly Configuration Configuration = IsLocalBuild ? Configuration.Debug : Configuration.Release;

    [Solution] readonly Solution Solution;

    [Parameter("Commit message")] readonly string CommitMessage = "build: automatic commit";

    AbsolutePath SourceDirectory => RootDirectory;
    AbsolutePath TestsDirectory => RootDirectory / "tests";
    AbsolutePath OutputDirectory => RootDirectory / "output";


    Target Restore => _ => _
        .Executes(() =>
        {
            DotNetRestore(s => s
                .SetProjectFile(Solution));
        });

    Target Compile => _ => _
        .DependsOn(Restore)
        .Executes(() =>
        {
            DotNetBuild(s => s
                .SetProjectFile(Solution)
                .SetConfiguration(Configuration)
                .EnableNoRestore());
        });

    Target BuildDockerImage => _ => _
        .DependsOn(Compile)
        .Executes(() =>
        {
            // Clean docker cache
            // This is useful to ensure that the Docker image is built from scratch
            Docker("builder prune --force");

            DockerBuild(s => s
                .SetPath(".")
                .SetFile(Solution.GetProject("PersianCat.Resume")?.Directory / "Dockerfile")
                .SetTag("persiancat-resume:latest")
                .EnableForceRm()
                .EnableNoCache());
        });

    Target Test => _ => _
        .DependsOn(Compile)
        .Executes(() =>
        {
            DotNetTest(s => s
                .SetProjectFile(Solution)
                .SetConfiguration(Configuration)
                .EnableNoBuild()
                .EnableNoRestore());
        });

    Target Publish => _ => _
        .DependsOn(Test)
        .Executes(() =>
        {
            DotNetPublish(s => s
                .SetProject(Solution.GetProject("PersianCat.Resume"))
                .SetConfiguration(Configuration)
                .SetOutput(OutputDirectory));
        });

    Target Commit => _ => _
        .DependsOn(Publish)
        .Executes(() =>
        {
            Git("add .");
            Git($"commit -m \"{CommitMessage}\"");
        });

    Target Push => _ => _
        .DependsOn(Commit)
        .Executes(() =>
        {
            Git("push");
        });
}