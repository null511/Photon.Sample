using Photon.Framework.Agent;
using Photon.Framework.Tasks;
using PhotonTasks.Internal;
using System.Threading;
using System.Threading.Tasks;

namespace PhotonTasks.DeployTasks
{
    [Roles(Configuration.Roles.Deploy.Web)]
    internal class UpdatePhotonSampleService : IDeployTask
    {
        public IAgentDeployContext Context {get; set;}


        public async Task RunAsync(CancellationToken token)
        {
            // Get the versioned application path
            var applicationPath = Context.GetApplicationDirectory(Configuration.Apps.Service.AppName, Context.ProjectPackageVersion);

            //throw new NotImplementedException();
        }
    }
}
