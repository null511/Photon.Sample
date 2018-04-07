using System.Threading.Tasks;
using Microsoft.Web.Administration;
using Photon.Framework.Tasks;
using Photon.Plugins.IIS;

namespace PhotonTasks.DeployTasks
{
    [Roles(Configuration.Roles.Deploy.Web)]
    internal class UpdatePhotonSampleWeb : IDeployTask
    {
        public async Task<TaskResult> RunAsync(IAgentDeployContext context)
        {
            // Get the versioned application path
            var applicationPath = context.GetApplicationDirectory(Configuration.Apps.Web, context.ProjectPackageVersion);

            var iis = new IISTools();

            iis.ConfigureAppPool(Configuration.AppPoolName, pool => {
                // Configure AppPool
                pool.AutoStart = true;
                pool.ManagedPipelineMode = ManagedPipelineMode.Integrated;
                pool.ManagedRuntimeVersion = "v4.0";

                // Start AppPool
                if (pool.State == ObjectState.Stopped)
                    pool.Start();
            });

            iis.ConfigureWebSite("Photon Web", site => {
                // Configure Website
                site.ApplicationDefaults.ApplicationPoolName = Configuration.AppPoolName;
                site.ServerAutoStart = true;

                // Set Bindings
                site.Bindings.Clear();
                site.Bindings.Add("*:80:localhost", "http");

                // Update Virtual Path
                site.Applications[0]
                    .VirtualDirectories["/"]
                    .PhysicalPath = applicationPath;

                // Start Website
                if (site.State == ObjectState.Stopped)
                    site.Start();
            });

            return TaskResult.Ok();
        }
    }
}
