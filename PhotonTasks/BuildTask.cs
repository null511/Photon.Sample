using Photon.Framework.Packages;
using Photon.Framework.Tasks;
using System;
using System.IO;
using System.Threading.Tasks;

namespace PhotonTasks
{
    public class BuildTask : IBuildTask
    {
        public async Task<TaskResult> RunAsync(IAgentBuildContext context)
        {
            context.Output.AppendLine("Building Solution...", ConsoleColor.White);

            context.RunCommandLine("bin\\msbuild.cmd \"Photon.Sample.sln\" /t:Rebuild /p:Configuration=\"Release\" /p:Platform=\"Any CPU\" /m");

            context.Output.AppendLine("Creating Package...", ConsoleColor.White);

            var now = DateTime.Now;
            var packageVersion = $"{now.Year}.{now.Month:00}.{now.Day:00}.{context.BuildNumber}";

            var projectDefinition = Path.Combine(context.ContentDirectory, "Project.json");
            var packageFilename = Path.Combine(context.ContentDirectory, $"Photon.Sample.{packageVersion}.zip");
            await ProjectPackageTools.CreatePackage(projectDefinition, packageVersion, packageFilename);

            context.Output.AppendLine("Publishing Package...", ConsoleColor.White);

            context.PushProjectPackage(packageFilename);

            context.Output
                .Append("Project Package: ", ConsoleColor.DarkCyan)
                .AppendLine(packageVersion, ConsoleColor.Cyan);

            return TaskResult.Ok();
        }
    }
}
