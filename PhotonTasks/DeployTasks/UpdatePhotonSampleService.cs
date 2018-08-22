using Photon.Framework.Agent;
using Photon.Framework.Tasks;
using PhotonTasks.Internal;
using System;
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
            var appRev = await Context.Applications.GetApplicationRevision(
                projectId: Context.Project.Id,
                appName: Configuration.Apps.Web.AppName,
                deploymentNumber: Context.DeploymentNumber,
                token: token);

            if (appRev == null) throw new ApplicationException($"Application revision directory not found for app '{Configuration.Apps.Web.AppName}' revision '{Context.DeploymentNumber}'!");

            // TODO
        }
    }
}
