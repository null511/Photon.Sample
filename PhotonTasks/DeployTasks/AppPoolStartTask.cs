using System.Threading.Tasks;
using Photon.Framework.Tasks;
using Photon.Plugins.IIS;

namespace PhotonTasks.DeployTasks
{
    [Roles(Configuration.Roles.Deploy.Web)]
    internal class AppPoolStartTask : IDeployTask
    {
        public async Task<TaskResult> RunAsync(IAgentDeployContext context)
        {
            using (var iis = new IISTools()) {
                iis.ConfigureAppPool(Configuration.AppPoolName, appPool => {
                    appPool.Start();
                });
            }

            return TaskResult.Ok();
        }
    }
}
