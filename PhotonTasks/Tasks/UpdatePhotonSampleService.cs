using Photon.Framework;
using Photon.Framework.Tasks;
using System;

namespace PhotonTasks
{
    [Roles(Roles.Deploy.Web)]
    class UpdatePhotonSampleService : ITask
    {
        public TaskResult Run(TaskContext context)
        {
            // Get the versioned application path
            var applicationPath = context.GetApplicationDirectory(Apps.Service, context.ReleaseVersion);

            // TODO: Update IIS Application path
            throw new NotImplementedException();

            return TaskResult.Ok();
        }
    }
}
