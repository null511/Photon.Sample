using Photon.Framework.Agent;
using Photon.Framework.Extensions;
using Photon.Framework.Packages;
using Photon.Framework.Tasks;
using PhotonTasks.Internal;
using System;
using System.IO;
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
            var applicationPath = Context.GetApplicationDirectory(Configuration.Apps.Service.AppName, Context.ProjectPackageVersion);

            string packageFilename = null;

            try {
                // Download Package to temp file
                packageFilename = await Context.PullApplicationPackageAsync(Configuration.Apps.Service.PackageId, Context.ProjectPackageVersion);

                // Unpackage contents to application path
                await ApplicationPackageTools.UnpackAsync(packageFilename, applicationPath);

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
                    try {File.Delete(packageFilename);}
                    catch {}
                }
            }
        }
    }
}
