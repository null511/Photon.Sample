using Photon.Framework.Agent;
using Photon.Framework.Packages;
using Photon.Framework.Tasks;
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
            // Get the versioned application path
            var applicationPath = Context.GetApplicationDirectory(Configuration.Apps.Service.AppName, Context.ProjectPackageVersion);

            string packageFilename = null;
            try {
                // Download Package to temp file
                packageFilename = await Context.PullApplicationPackageAsync(Configuration.Apps.Service.PackageId, Context.ProjectPackageVersion);

                // Unpackage contents to application path
                await ApplicationPackageTools.UnpackAsync(packageFilename, applicationPath);
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
