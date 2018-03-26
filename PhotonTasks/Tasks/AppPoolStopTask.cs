using Photon.Framework;
using Photon.Framework.Tasks;
using System;

namespace PhotonTasks
{
    [Roles(Roles.Deploy.Web)]
    class AppPoolStopTask : ITask
    {
        public TaskResult Run(TaskContext context)
        {
            // TODO: Stop AppPool
            throw new NotImplementedException();

            return TaskResult.Ok();
        }
    }
}
