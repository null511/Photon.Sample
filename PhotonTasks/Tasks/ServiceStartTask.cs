using Photon.Framework;
using Photon.Framework.Tasks;
using System;

namespace PhotonTasks
{
    [Roles(Roles.Deploy.Service)]
    class ServiceStartTask : ITask
    {
        public TaskResult Run(TaskContext context)
        {
            // TODO: Start Service
            throw new NotImplementedException();

            return TaskResult.Ok();
        }
    }
}
