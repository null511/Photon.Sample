using Photon.Framework.Agent;
using Photon.Framework.Tasks;
using Photon.WindowsServices;
using System.Threading.Tasks;

namespace PhotonTasks.DeployTasks
{
    [Roles(Configuration.Roles.Deploy.Service)]
    internal class ServiceStopTask : IDeployTask
    {
        public IAgentDeployContext Context {get; set;}


        public async Task<TaskResult> RunAsync()
        {
            var tools = new WindowsServiceTools(Context);

            await tools.StopAsync(Configuration.Apps.Service.AppName);

            return TaskResult.Ok();
        }
    }
}
