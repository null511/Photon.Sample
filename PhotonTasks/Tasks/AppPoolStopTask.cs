using Microsoft.Web.Administration;
using Photon.Framework;
using Photon.Framework.Tasks;
using Photon.Plugins.IIS;

namespace PhotonTasks
{
    [Roles(Configuration.Roles.Deploy.Web)]
    class AppPoolStopTask : ITask
    {
        public TaskResult Run(TaskContext context)
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
