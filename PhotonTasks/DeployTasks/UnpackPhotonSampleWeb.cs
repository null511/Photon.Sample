using Photon.Framework.Agent;
using Photon.Framework.Applications;
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

                appRev = await Context.Applications.RegisterApplicationRevision(request);
            }

            string packageFilename = null;
            try {
                // Download Package to temp file
                packageFilename = await Context.Packages.PullApplicationPackageAsync(Configuration.Apps.Web.PackageId, Context.ProjectPackageVersion);

                // Unpackage contents to application path
                await ApplicationPackageTools.UnpackAsync(packageFilename, appRev.ApplicationPath);
            }
            finally {
                if (packageFilename != null) {
                    try {
                        PathEx.Delete(packageFilename);
                    }
                    catch {}
                }
            }
        }
    }
}
