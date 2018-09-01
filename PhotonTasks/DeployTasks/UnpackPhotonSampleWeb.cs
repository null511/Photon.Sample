using Photon.Framework.Agent;
using Photon.Framework.Packages;
using Photon.Framework.Tasks;
using Photon.Framework.Tools;
using PhotonTasks.Internal;
using System.Threading;
using System.Threading.Tasks;

namespace PhotonTasks.DeployTasks
{
    [Roles(Configuration.Roles.Deploy.Web)]
    internal class UnpackPhotonSampleWeb : IDeployTask
    {
        public IAgentDeployContext Context {get; set;}


        public async Task RunAsync(CancellationToken token)
        {
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
                    id: Configuration.Apps.Web.PackageId,
                    version: Context.ProjectPackageVersion);

                // Unpackage contents to application path
                await ApplicationPackageTools.UnpackAsync(packageFilename, appRev.ApplicationPath);
            }
            finally {
                try {
                    if (packageFilename != null)
                        PathEx.Delete(packageFilename);
                }
                catch {}
            }
        }
    }
}
