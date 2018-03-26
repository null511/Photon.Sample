using Photon.Framework.Scripts;

namespace PhotonTasks
{
    class BuildScript : IScript
    {
        public ScriptResult Run(ScriptContext context)
        {
            var agents = context.RegisterAgents(
                Roles.Deploy.Web,
                Roles.Deploy.Service);

            try {
                agents.Initialize();

                agents.RunTasks(
                    "UnpackPhotonSampleWeb",
                    "UnpackPhotonSampleService");

                agents.RunTask("Stop");
            }
            finally {
                agents.Release();
            }

            return new ScriptResult {
                Successful = true,
                Message = "Ok",
            };
        }
    }
}
