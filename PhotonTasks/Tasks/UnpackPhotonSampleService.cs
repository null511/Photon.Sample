using Photon.Framework;
using Photon.Framework.Tasks;

namespace PhotonTasks
{
    [Roles(Roles.Deploy.Service)]
    class UnpackPhotonSampleService : ITask
    {
        public TaskResult Run(TaskContext context)
        {
            context.DownloadPackage("photon.sample.web", context.ReleaseVersion);

            return new TaskResult {
                Successful = true,
                Message = "Ok",
            };
        }
    }
}
