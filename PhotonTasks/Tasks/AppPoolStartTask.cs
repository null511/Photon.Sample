using Photon.Framework;
using Photon.Framework.Tasks;
using System;

namespace PhotonTasks
{
    [Roles(Roles.Deploy.Web)]
    class AppPoolStartTask : ITask
    {
        public TaskResult Run(TaskContext context)
        {
            // TODO: Start AppPool
            throw new NotImplementedException();

            return TaskResult.Ok();
        }
    }
}
