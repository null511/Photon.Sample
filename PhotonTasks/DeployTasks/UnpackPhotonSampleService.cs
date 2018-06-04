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
            Context.Output.Write("Unpackaging ", ConsoleColor.DarkCyan)
                .Write(Configuration.Apps.Service.AppName, ConsoleColor.Cyan)
                .WriteLine("...", ConsoleColor.DarkCyan);

            // Get the versioned application path
            var applicationPath = Context.GetApplicationDirectory(Configuration.Apps.Service.AppName, Context.ProjectPackageVersion);

            string packageFilename = null;

            try {
                // Download Package to temp file
                packageFilename = await Context.PullApplicationPackageAsync(Configuration.Apps.Service.PackageId, Context.ProjectPackageVersion);

                // Unpackage contents to application path
                await ApplicationPackageTools.UnpackAsync(packageFilename, applicationPath);

                Context.Output.Write("Unpackaged ", ConsoleColor.DarkGreen)
                    .Write(Configuration.Apps.Service.AppName, ConsoleColor.Green)
                    .WriteLine("successfully.", ConsoleColor.DarkGreen);
            }
            catch (Exception error) {
                Context.Output.Write("Failed to unpackage ", ConsoleColor.DarkCyan)
                    .Write(Configuration.Apps.Service.AppName, ConsoleColor.Cyan)
                    .WriteLine("! ", ConsoleColor.DarkCyan)
                    .WriteLine(error.UnfoldMessages(), ConsoleColor.DarkYellow);

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
