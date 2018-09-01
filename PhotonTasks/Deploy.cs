using Photon.Framework.Server;
using PhotonTasks.DeployTasks;
using PhotonTasks.Internal;
using System.Threading;
using System.Threading.Tasks;

namespace PhotonTasks
{
    public class Deploy : IDeployScript
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

                // Update Applications
                await agents.RunTasksAsync(
                    nameof(InstallPhotonSampleWeb),
                    nameof(InstallPhotonSampleService));
            }
            finally {
                await agents.ReleaseAllAsync(token);
            }
        }
    }
}
