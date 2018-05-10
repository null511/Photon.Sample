using Photon.Framework.Agent;
using Photon.Framework.Extensions;
using Photon.Framework.Packages;
using Photon.Framework.Tasks;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace PhotonTasks
{
    public class BuildTask : IBuildTask
    {
        private string packageVersion;

        public IAgentBuildContext Context {get; set;}


        public async Task RunAsync(CancellationToken token)
        {
            Context.Output.AppendLine("Building Solution...", ConsoleColor.White);

            Context.RunCommandLine("bin\\msbuild.cmd", "/m", "/v:m",
                "\"Photon.Sample.sln\"",
                "/t:Rebuild",
                "/p:Configuration=\"Debug\"",
                "/p:Platform=\"Any CPU\"",
                "/p:DeployOnBuild=true",
                "/p:publishUrl=\"Publish\"",
                "/p:DeployDefaultTarget=WebPublish",
                "/p:DeleteExistingFiles=True",
                "/p:WebPublishMethod=FileSystem");

            packageVersion = Context.BuildNumber.ToString();

            await Task.WhenAll(
                CreateProjectPackage(),
                CreateWebApplicationPackage(),
                CreateServiceApplicationPackage());

            Context.Output
                .Append("Build Number: ", ConsoleColor.DarkBlue)
                .AppendLine(Context.BuildNumber, ConsoleColor.Blue);
        }

        private async Task CreateProjectPackage()
        {
            Context.Output.AppendLine("Creating Project Package...", ConsoleColor.White);

            try {
                var packageDefinition = Path.Combine(Context.ContentDirectory, "PhotonTasks", "PhotonTasks.json");
                var projectPackageFilename = Path.Combine(Context.BinDirectory, $"photon.sample.tasks.{packageVersion}.zip");

                await ProjectPackageTools.CreatePackage(packageDefinition, packageVersion, projectPackageFilename);
                await Context.PushProjectPackageAsync(projectPackageFilename);

                Context.Output.AppendLine("Created Project Package successfully.", ConsoleColor.DarkGreen);
            }
            catch (Exception error) {
                Context.Output.AppendLine($"Failed to create Project Package! {error.UnfoldMessages()}", ConsoleColor.DarkYellow);
                throw;
            }
        }

        private async Task CreateWebApplicationPackage()
        {
            Context.Output.AppendLine("Creating Web Application Package...", ConsoleColor.White);

            try {
                //Context.RunCommandLine("bin\\msbuild.cmd \"Photon.Sample.sln\" /t:Rebuild /p:Configuration=\"Release\" /p:Platform=\"Any CPU\" /m");

                var packageDefinition = Path.Combine(Context.ContentDirectory, "WebApplication", "WebApplication.json");
                var webAppPackageFilename = Path.Combine(Context.BinDirectory, $"photon.sample.web.{packageVersion}.zip");

                await ApplicationPackageTools.CreatePackage(packageDefinition, packageVersion, webAppPackageFilename);
                await Context.PushApplicationPackageAsync(webAppPackageFilename);

                Context.Output.AppendLine("Created Web Application Package successfully.", ConsoleColor.DarkGreen);
            }
            catch (Exception error) {
                Context.Output.AppendLine($"Failed to create Web Application Package! {error.UnfoldMessages()}", ConsoleColor.DarkYellow);
                throw;
            }
        }

        private async Task CreateServiceApplicationPackage()
        {
            Context.Output.AppendLine("Creating Service Application Package...", ConsoleColor.White);

            try {
                var packageDefinition = Path.Combine(Context.ContentDirectory, "WindowsService", "WindowsService.json");
                var svcAppPackageFilename = Path.Combine(Context.BinDirectory, $"photon.sample.svc.{packageVersion}.zip");

                await ApplicationPackageTools.CreatePackage(packageDefinition, packageVersion, svcAppPackageFilename);
                await Context.PushApplicationPackageAsync(svcAppPackageFilename);

                Context.Output.AppendLine("Created Service Application Package successfully.", ConsoleColor.DarkGreen);
            }
            catch (Exception error) {
                Context.Output.AppendLine($"Failed to create Service Application Package! {error.UnfoldMessages()}", ConsoleColor.DarkYellow);
                throw;
            }
        }
    }
}
