using Photon.Framework;
using Photon.Framework.Tasks;
using System;

namespace PhotonTasks
{
    [Roles(Roles.Deploy.Web)]
    class UpdatePhotonSampleWeb : ITask
    {
        public TaskResult Run(TaskContext context)
        {
            // Get the versioned application path
            var applicationPath = context.GetApplicationDirectory(Apps.Web, context.ReleaseVersion);

            // TODO: Update IIS Application path
            throw new NotImplementedException();

            return TaskResult.Ok();
        }
    }
}
