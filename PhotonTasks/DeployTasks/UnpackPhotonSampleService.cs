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
            var appRev = await Context.GetApplicationRevision(
                projectId: Context.Project.Id,
                appName: Configuration.Apps.Web.AppName,
                deploymentNumber: Context.DeploymentNumber);

            if (appRev == null) {
                var request = new DomainApplicationRevisionRequest {
                    ProjectId = Context.Project.Id,
                    ApplicationName = Configuration.Apps.Web.AppName,
                    DeploymentNumber = Context.DeploymentNumber,
                    PackageId = Configuration.Apps.Web.PackageId,
                    PackageVersion = Context.ProjectPackageVersion,
                };

                appRev = await Context.RegisterApplicationRevision(request);
            }

            string packageFilename = null;

            try {
                // Download Package to temp file
                packageFilename = await Context.PullApplicationPackageAsync(Configuration.Apps.Service.PackageId, Context.ProjectPackageVersion);

                // Unpackage contents to application path
                await ApplicationPackageTools.UnpackAsync(packageFilename, appRev.ApplicationPath);

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
