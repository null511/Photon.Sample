using Photon.Framework.Agent;
using Photon.Framework.Packages;
using Photon.Framework.Tasks;
using System.IO;
using System.Threading.Tasks;

namespace PhotonTasks.DeployTasks
{
    [Roles(Configuration.Roles.Deploy.Service)]
    internal class UnpackPhotonSampleService : IDeployTask
    {
        public IAgentDeployContext Context {get; set;}


        public async Task<TaskResult> RunAsync()
        {
            // Download package to working directory
            var packageFilename = Path.Combine(Context.ContentDirectory, "photon.sample.svc.zip");

            await Context.PullApplicationPackageAsync(Configuration.Apps.Service.PackageId, Context.ProjectPackageVersion, packageFilename);

            // Get the versioned application path
            var applicationPath = Context.GetApplicationDirectory(Configuration.Apps.Service.AppName, Context.ProjectPackageVersion);

            // Unpackage contents to application path
            await ApplicationPackageTools.UnpackAsync(packageFilename, applicationPath);

            return TaskResult.Ok();
        }
    }
}
