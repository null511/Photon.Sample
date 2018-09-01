using Photon.Framework.Agent;
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
            var appRev = await Context.Applications.GetApplicationRevision(
                appName: Configuration.Apps.Web.AppName)
            ?? await Context.Applications.RegisterApplicationRevision(
                appName: Configuration.Apps.Web.AppName,
                packageId: Configuration.Apps.Web.PackageId,
                packageVersion: Context.ProjectPackageVersion);

            string packageFilename = null;

            try {
                // Download Package to temp file
                packageFilename = await Context.Packages.PullApplicationPackageAsync(
                    id: Configuration.Apps.Service.PackageId,
                    version: Context.ProjectPackageVersion);

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
                try {
                    if (packageFilename != null)
                        PathEx.Delete(packageFilename);
                }
                catch { }
            }
        }
    }
}
