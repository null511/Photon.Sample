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
            if (!Context.Applications.TryGetApplication(Context.Project.Id, Configuration.Apps.Web.AppName, out var app))
                app = Context.Applications.RegisterApplication(Context.Project.Id, Configuration.Apps.Web.AppName);

            if (!app.TryGetRevision(Context.DeploymentNumber, out var appRev)) {
                var rev = new ApplicationRevision {
                    DeploymentNumber = Context.DeploymentNumber,
                    PackageId = Configuration.Apps.Web.PackageId,
                    PackageVersion = Context.ProjectPackageVersion,
                };

                appRev = app.RegisterRevision(rev);
            }

            string packageFilename = null;
            try {
                // Download Package to temp file
                packageFilename = await Context.PullApplicationPackageAsync(Configuration.Apps.Web.PackageId, Context.ProjectPackageVersion);

                // Unpackage contents to application path
                await ApplicationPackageTools.UnpackAsync(packageFilename, appRev.Location);
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
