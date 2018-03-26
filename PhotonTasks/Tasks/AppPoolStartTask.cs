using Photon.Framework;
using Photon.Framework.Tasks;
using Photon.IIS;

namespace PhotonTasks
{
    [Roles(Configuration.Roles.Deploy.Web)]
    class AppPoolStartTask : ITask
    {
        public TaskResult Run(TaskContext context)
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
