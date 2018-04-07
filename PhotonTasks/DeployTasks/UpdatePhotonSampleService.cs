using System;
using System.Threading.Tasks;
using Photon.Framework.Tasks;

namespace PhotonTasks.DeployTasks
{
    [Roles(Configuration.Roles.Deploy.Web)]
    internal class UpdatePhotonSampleService : IDeployTask
    {
        public async Task<TaskResult> RunAsync(IAgentDeployContext context)
        {
            // Get the versioned application path
            var applicationPath = context.GetApplicationDirectory(Configuration.Apps.Service, context.ProjectPackageVersion);

            // TODO: Update IIS Application path
            throw new NotImplementedException();

            return TaskResult.Ok();
        }
    }
}
