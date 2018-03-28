using Photon.Framework.Scripts;
using System;
using System.Threading.Tasks;

namespace PhotonTasks
{
    public class BuildScript : IScript
    {
        public async Task<ScriptResult> RunAsync(ScriptContext context)
        {
            var agents = context.RegisterAgents(
                Configuration.Roles.Build);

            try {
                await agents.InitializeAsync();

                // Build Solution
                await agents.RunTasksAsync(
                    nameof(BuildSolutionTask));

                throw new NotImplementedException();

                // TODO: Create Web Package
                // TODO: Create Service Package

                // TODO: Create Release
                // TODO: Publish Release Package
                // TODO: Publish Web and Sevice Project Packages
            }
            finally {
                await agents.ReleaseAllAsync();
            }

            return ScriptResult.Ok();
        }
    }
}
