using System.Threading.Tasks;
using Microsoft.Web.Administration;
using Photon.Framework.Tasks;
using Photon.Plugins.IIS;

namespace PhotonTasks.DeployTasks
{
    [Roles(Configuration.Roles.Deploy.Web)]
    internal class AppPoolStopTask : IDeployTask
    {
        public async Task<TaskResult> RunAsync(IAgentDeployContext context)
        {
            using (var iis = new IISTools()) {
                iis.ConfigureAppPool(Configuration.AppPoolName, appPool => {
                    appPool.Stop();

                    appPool.AutoStart = true;
                    appPool.ManagedPipelineMode = ManagedPipelineMode.Integrated;
                });
            }

            return TaskResult.Ok();
        }
    }
}
