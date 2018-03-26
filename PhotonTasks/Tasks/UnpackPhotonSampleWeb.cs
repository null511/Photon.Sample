using Photon.Framework;
using Photon.Framework.Packages;
using Photon.Framework.Tasks;
using System.IO;

namespace PhotonTasks
{
    [Roles(Roles.Deploy.Web)]
    class UnpackPhotonSampleWeb : ITask
    {
        public TaskResult Run(TaskContext context)
        {
            var packageFilename = context.DownloadPackage("photon.sample.web", context.ReleaseVersion, context.WorkDirectory);

            var applicationPath = context.GetApplicationDirectory("Photon.Sample.Web", context.ReleaseVersion);

            //if (!Directory.Exists(applicationPath))
            //    Directory.CreateDirectory(applicationPath);

            PackageTools.Unpackage(packageFilename, applicationPath);

            return new TaskResult {
                Successful = true,
                Message = "Ok",
            };
        }
    }
}
