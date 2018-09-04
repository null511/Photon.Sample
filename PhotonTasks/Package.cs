using Photon.Framework.Agent;
using Photon.Framework.Extensions;
using Photon.Framework.Packages;
using Photon.Framework.Tasks;
using Photon.MSBuild;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace PhotonTasks
{
    public class Package : IBuildTask
    {
        private string packageVersion;
        private ApplicationPackageUtility appPackages;
        private ProjectPackageUtility projectPackages;

        public IAgentBuildContext Context {get; set;}


        public async Task RunAsync(CancellationToken token)
        {
            Context.Output.WriteLine("Building Solution...", ConsoleColor.White);

            var msbuild = new MSBuildCommand(Context) {
                Exe = ".\\bin\\msbuild.cmd",
                WorkingDirectory = Context.ContentDirectory,
            };

            var buildArgs = new MSBuildArguments {
                ProjectFile = "Photon.Sample.sln",
                Targets = {"Rebuild"},
                Properties = {
                    ["Configuration"] = "Debug",
                    ["Platform"] = "Any CPU",
                    ["DeployOnBuild"] = "True",
                    ["PublishUrl"] = "Publish",
                    ["DeployDefaultTarget"] = "WebPublish",
                    ["DeleteExistingFiles"] = "True",
                    ["WebPublishMethod"] = "FileSystem",
                },
                Logger = {
                    ConsoleLoggerParameters = MSBuildConsoleLoggerParameters.ErrorsOnly
                        | MSBuildConsoleLoggerParameters.Summary,
                },
                Verbosity = MSBuildVerbosityLevels.Minimal,
                MaxCpuCount = 0,
            };

            await msbuild.RunAsync(buildArgs, token);

            packageVersion = Context.BuildNumber.ToString();
            var packagePath = Path.Combine(Context.BinDirectory, "Packages");
            appPackages = new ApplicationPackageUtility(Context) {
                PackageDirectory = packagePath,
            };

            projectPackages = new ProjectPackageUtility(Context) {
                PackageDirectory = packagePath,
            };

            await Task.WhenAll(
                CreateProjectPackage(token),
                CreateWebApplicationPackage(token),
                CreateServiceApplicationPackage(token));

            using (var block = Context.Output.WriteBlock()) {
                block.Write("Build Number: ", ConsoleColor.DarkBlue);
                block.WriteLine(Context.BuildNumber, ConsoleColor.Blue);
            }
        }

        private async Task CreateProjectPackage(CancellationToken token)
        {
            Context.Output.WriteLine("Creating Project Package...", ConsoleColor.White);

            try {
                var packageDefinition = Path.Combine(Context.ContentDirectory, "PhotonTasks", "PhotonTasks.json");

                await projectPackages.Publish(packageDefinition, packageVersion, token);
            }
            catch (Exception error) {
                Context.Output.WriteLine($"Failed to create Project Package! {error.UnfoldMessages()}", ConsoleColor.DarkYellow);
                throw;
            }
        }

        private async Task CreateWebApplicationPackage(CancellationToken token)
        {
            var packageDefinition = Path.Combine(Context.ContentDirectory, "WebApplication", "WebApplication.json");

            await appPackages.Publish(packageDefinition, packageVersion, token);
        }

        private async Task CreateServiceApplicationPackage(CancellationToken token)
        {
            var packageDefinition = Path.Combine(Context.ContentDirectory, "WindowsService", "WindowsService.json");

            await appPackages.Publish(packageDefinition, packageVersion, token);
        }
    }
}
