using Photon.Framework.Agent;
using Photon.Framework.Applications;
using Photon.Framework.Extensions;
using Photon.Framework.Packages;
using Photon.Framework.Tasks;
using Photon.Framework.Tools;
using PhotonTasks.Internal;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace PhotonTasks.DeployTasks
{
    [Roles(Configuration.Roles.Deploy.Service)]
    internal class UnpackPhotonSampleService : IDeployTask
    {
        public IAgentDeployContext Context {get; set;}


        public async Task RunAsync(CancellationToken token)
        {
            using (var block = Context.Output.WriteBlock()) {
                block.Write("Unpackaging ", ConsoleColor.DarkCyan);
                block.Write(Configuration.Apps.Service.AppName, ConsoleColor.Cyan);
                block.WriteLine("...", ConsoleColor.DarkCyan);
            }

            // Get the versioned application path
            if (!Context.Applications.TryGetApplication(Context.Project.Id, Configuration.Apps.Service.AppName, out var app))
                app = Context.Applications.RegisterApplication(Context.Project.Id, Configuration.Apps.Service.AppName);

            if (!app.TryGetRevision(Context.DeploymentNumber, out var appRev)) {
                var rev = new ApplicationRevision {
                    DeploymentNumber = Context.DeploymentNumber,
                    PackageId = Configuration.Apps.Service.PackageId,
                    PackageVersion = Context.ProjectPackageVersion,
                };

                appRev = app.RegisterRevision(rev);
            }

            string packageFilename = null;

            try {
                // Download Package to temp file
                packageFilename = await Context.PullApplicationPackageAsync(Configuration.Apps.Service.PackageId, Context.ProjectPackageVersion);

                // Unpackage contents to application path
                await ApplicationPackageTools.UnpackAsync(packageFilename, appRev.Location);

                using (var block = Context.Output.WriteBlock()) {
                    block.Write("Unpackaged ", ConsoleColor.DarkGreen);
                    block.Write(Configuration.Apps.Service.AppName, ConsoleColor.Green);
                    block.WriteLine("successfully.", ConsoleColor.DarkGreen);
                }
            }
            catch (Exception error) {
                using (var block = Context.Output.WriteBlock()) {
                    block.Write("Failed to unpackage ", ConsoleColor.DarkCyan);
                    block.Write(Configuration.Apps.Service.AppName, ConsoleColor.Cyan);
                    block.WriteLine("! ", ConsoleColor.DarkCyan);
                    block.WriteLine(error.UnfoldMessages(), ConsoleColor.DarkYellow);
                }

                throw;
            }
            finally {
                if (packageFilename != null) {
                    try {
                        PathEx.Delete(packageFilename);
                    }
                    catch { }
                }
            }
        }
    }
}
