using Photon.Framework.Agent;
using Photon.Framework.Tasks;
using Photon.WindowsServices;
using System.Threading.Tasks;

namespace PhotonTasks.DeployTasks
{
    [Roles(Configuration.Roles.Deploy.Service)]
    internal class ServiceStartTask : IDeployTask
    {
        public IAgentDeployContext Context {get; set;}

        public async Task<TaskResult> RunAsync()
        {
            var tools = new WindowsServiceTools(Context);

            await tools.StartAsync(Configuration.Apps.Service.AppName);

            return TaskResult.Ok();
        }
    }
}
