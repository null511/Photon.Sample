﻿using Photon.Framework.Scripts;
using System.Threading.Tasks;
using PhotonTasks.DeployTasks;

namespace PhotonTasks
{
    public class DeployScript : IDeployScript
    {
        public async Task<ScriptResult> RunAsync(IServerDeployContext context)
        {
            var agents = context.RegisterAgents(
                Configuration.Roles.Deploy.Web,
                Configuration.Roles.Deploy.Service);

            try {
                await agents.InitializeAsync("", "");

                // Unpack Applications
                await agents.RunTasksAsync(
                    nameof(UnpackPhotonSampleWeb),
                    nameof(UnpackPhotonSampleService));

                // Stop Applications
                await agents.RunTasksAsync(
                    nameof(ServiceStopTask),
                    nameof(AppPoolStopTask));

                // Update Applications
                await agents.RunTasksAsync(
                    nameof(UpdatePhotonSampleWeb),
                    nameof(UpdatePhotonSampleService));

                // Start Applications
                await agents.RunTasksAsync(
                    nameof(ServiceStartTask),
                    nameof(AppPoolStartTask));
            }
            finally {
                await agents.ReleaseAllAsync();
            }

            return ScriptResult.Ok();
        }
    }
}
