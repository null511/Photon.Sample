using System;
using System.Threading.Tasks;
using Photon.Framework.Tasks;

namespace PhotonTasks.DeployTasks
{
    [Roles(Configuration.Roles.Deploy.Service)]
    internal class ServiceStopTask : IDeployTask
    {
        public async Task<TaskResult> RunAsync(IAgentDeployContext context)
        {
            // TODO: Stop Service
            throw new NotImplementedException();

            return TaskResult.Ok();
        }
    }
}
