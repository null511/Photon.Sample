using Photon.Framework;
using Photon.Framework.Packages;
using Photon.Framework.Tasks;

namespace PhotonTasks
{
    [Roles(Configuration.Roles.Deploy.Web)]
    class UnpackPhotonSampleWeb : ITask
    {
        public TaskResult Run(TaskContext context)
        {
            // Download package to working directory
            var packageFilename = context.DownloadPackage("photon.sample.web", context.ReleaseVersion, context.WorkDirectory);

            // Get the versioned application path
            var applicationPath = context.GetApplicationDirectory(Configuration.Apps.Web, context.ReleaseVersion);

            // Unpackage contents to application path
            PackageTools.Unpackage(packageFilename, applicationPath);

            return TaskResult.Ok();
        }
    }
}
