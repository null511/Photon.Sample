using Photon.Framework.Agent;
using Photon.Framework.Tasks;
using PhotonTasks.Internal;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace PhotonTasks.DeployTasks
{
    [Roles(Configuration.Roles.Deploy.Web)]
    internal class InstallPhotonSampleService : IDeployTask
    {
        public IAgentDeployContext Context {get; set;}


        public async Task RunAsync(CancellationToken token)
        {
            // Get the versioned application path
            var appRev = await Context.Applications.GetApplicationRevision(Configuration.Apps.Service.AppName);

            if (appRev == null) throw new ApplicationException($"Application revision directory not found for app '{Configuration.Apps.Service.AppName}' revision '{Context.DeploymentNumber}'!");

            // TODO
        }
    }
}
