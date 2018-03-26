using Photon.Framework;
using Photon.Framework.Tasks;
using System;

namespace PhotonTasks
{
    [Roles(Roles.Deploy.Service)]
    class ServiceStopTask : ITask
    {
        public TaskResult Run(TaskContext context)
        {
            // TODO: Stop Service
            throw new NotImplementedException();

            return TaskResult.Ok();
        }
    }
}
