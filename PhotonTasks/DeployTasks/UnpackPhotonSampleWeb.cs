using Photon.Framework.Packages;
using Photon.Framework.Tasks;
using System.Threading.Tasks;

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
            ApplicationPackageTools.Unpack(packageFilename, applicationPath);

            return TaskResult.Ok();
        }
    }
}
