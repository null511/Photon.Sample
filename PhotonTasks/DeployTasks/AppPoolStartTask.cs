using Photon.Framework.Agent;
using Photon.Framework.Tasks;
using Photon.Plugins.IIS;
using System;
using System.Threading.Tasks;

namespace PhotonTasks.DeployTasks
{
    [Roles(Configuration.Roles.Deploy.Web)]
    internal class AppPoolStartTask : IDeployTask
    {
        public IAgentDeployContext Context {get; set;}


        public async Task<TaskResult> RunAsync()
        {
            using (var iis = new IISTools(Context)) {
                await iis.ApplicationPool.StartAsync(Configuration.AppPoolName, TimeSpan.FromMinutes(1));
            }

            return TaskResult.Ok();
        }
    }
}
