using Photon.Framework;
using Photon.Framework.Packages;
using Photon.Framework.Tasks;

namespace PhotonTasks
{
    [Roles(Roles.Deploy.Service)]
    class UnpackPhotonSampleService : ITask
    {
        public TaskResult Run(TaskContext context)
        {
            // Download package to working directory
            var packageFilename = context.DownloadPackage("photon.sample.svc", context.ReleaseVersion, context.WorkDirectory);

            // Get the versioned application path
            var applicationPath = context.GetApplicationDirectory(Apps.Service, context.ReleaseVersion);

            // Unpackage contents to application path
            PackageTools.Unpackage(packageFilename, applicationPath);

            return TaskResult.Ok();
        }
    }
}
