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
            if (!Context.Applications.TryGetApplication(Context.Project.Id, Configuration.Apps.Service.AppName, out var app))
                throw new ApplicationException($"Application directory not found for app '{Configuration.Apps.Service.AppName}'!");

            if (!app.TryGetRevision(Context.DeploymentNumber, out var appRev))
                throw new ApplicationException($"Application revision directory not found for app '{Configuration.Apps.Service.AppName}' revision '{Context.DeploymentNumber}'!");

            // TODO
        }
    }
}
