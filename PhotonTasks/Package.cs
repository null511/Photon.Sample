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
    public class Package : IBuildTask
    {
        private string packageVersion;
        private ApplicationPackageUtility appPackages;
        private ProjectPackageUtility projectPackages;

        public IAgentBuildContext Context {get; set;}


        public async Task RunAsync(CancellationToken token)
        {
            Context.Output.WriteLine("Building Solution...", ConsoleColor.White);

            Context.RunCommandLine(".\\bin\\msbuild.cmd", "/m", "/v:m",
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

            Context.Output
                .Write("Build Number: ", ConsoleColor.DarkBlue)
                .WriteLine(Context.BuildNumber, ConsoleColor.Blue);
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
