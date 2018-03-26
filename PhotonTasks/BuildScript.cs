using Photon.Framework.Scripts;

namespace PhotonTasks
{
    public class BuildScript : IScript
    {
        public ScriptResult Run(ScriptContext context)
        {
            var agents = context.RegisterAgents(
                Configuration.Roles.Deploy.Web,
                Configuration.Roles.Deploy.Service);

            try {
                agents.Initialize();

                // Unpack Applications
                agents.RunTasks(
                    nameof(UnpackPhotonSampleWeb),
                    nameof(UnpackPhotonSampleService));

                // Stop Applications
                agents.RunTasks(
                    nameof(ServiceStopTask),
                    nameof(AppPoolStopTask));

                // Update Applications
                agents.RunTasks(
                    nameof(UpdatePhotonSampleWeb),
                    nameof(UpdatePhotonSampleService));

                // Start Applications
                agents.RunTasks(
                    nameof(ServiceStartTask),
                    nameof(AppPoolStartTask));
            }
            finally {
                agents.Release();
            }

            return ScriptResult.Ok();
        }
    }
}
