using Photon.Framework.Server;
using PhotonTasks.DeployTasks;
using System.Threading;
using System.Threading.Tasks;

namespace PhotonTasks
{
    public class DeployScript : IDeployScript
    {
        public IServerDeployContext Context {get; set;}


        public async Task RunAsync(CancellationToken token)
        {
            var agents = Context.RegisterAgents.All(
                Configuration.Roles.Deploy.Web,
                Configuration.Roles.Deploy.Service);

            try {
                await agents.InitializeAsync(token);

                // Unpack Applications
                await agents.RunTasksAsync(
                    nameof(UnpackPhotonSampleWeb),
                    nameof(UnpackPhotonSampleService));

                // Stop Applications
                //await agents.RunTasksAsync(
                //    nameof(ServiceStopTask),
                //    nameof(AppPoolStopTask));

                // Update Applications
                await agents.RunTasksAsync(
                    nameof(UpdatePhotonSampleWeb),
                    nameof(UpdatePhotonSampleService));

                // Start Applications
                //await agents.RunTasksAsync(
                //    nameof(ServiceStartTask),
                //    nameof(AppPoolStartTask));
            }
            finally {
                await agents.ReleaseAllAsync(token);
            }
        }
    }
}
