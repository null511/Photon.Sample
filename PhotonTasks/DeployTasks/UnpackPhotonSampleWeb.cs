using System.Threading.Tasks;
using Photon.Framework.Packages;
using Photon.Framework.Tasks;

namespace PhotonTasks.DeployTasks
{
    [Roles(Configuration.Roles.Deploy.Web)]
    internal class UnpackPhotonSampleWeb : IDeployTask
    {
        public async Task<TaskResult> RunAsync(IAgentDeployContext context)
        {
            // Download package to working directory
            var packageFilename = await context.DownloadApplicationPackageAsync("photon.sample.web", context.ProjectPackageVersion, context.WorkDirectory);

            // Get the versioned application path
            var applicationPath = context.GetApplicationDirectory(Configuration.Apps.Web, context.ProjectPackageVersion);

            // Unpackage contents to application path
            PackageTools.UnpackApplication(packageFilename, applicationPath);

            return TaskResult.Ok();
        }
    }
}
