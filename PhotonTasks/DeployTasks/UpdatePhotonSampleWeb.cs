using Microsoft.Web.Administration;
using Photon.Framework.Agent;
using Photon.Framework.Tasks;
using Photon.Plugins.IIS;
using System.Threading;
using System.Threading.Tasks;
using Configuration = PhotonTasks.Internal.Configuration;

namespace PhotonTasks.DeployTasks
{
    [Roles(Configuration.Roles.Deploy.Web)]
    internal class UpdatePhotonSampleWeb : IDeployTask
    {
        public IAgentDeployContext Context {get; set;}


        public async Task RunAsync(CancellationToken token)
        {
            // Get the versioned application path
            var applicationPath = Context.GetApplicationDirectory(Configuration.Apps.Web.AppName, Context.ProjectPackageVersion);

            using (var iis = new IISTools(Context)) {
                iis.ApplicationPool.Configure(Configuration.AppPoolName, pool => {
                    // Configure AppPool
                    pool.AutoStart = true;
                    pool.ManagedPipelineMode = ManagedPipelineMode.Integrated;
                    pool.ManagedRuntimeVersion = "v4.0";

                    // Start Website
                    if (pool.State == ObjectState.Stopped)
                        pool.Start();
                });

                iis.WebSite.Configure("Photon Web", 8086, site => {
                    // Configure Website
                    site.ApplicationDefaults.ApplicationPoolName = Configuration.AppPoolName;
                    site.ServerAutoStart = true;

                    // Set Bindings
                    site.Bindings.Clear();
                    site.Bindings.Add("*:8086:localhost", "http");

                    // Update Virtual Path
                    site.Applications[0]
                        .VirtualDirectories["/"]
                        .PhysicalPath = applicationPath;

                    // Start Website
                    if (site.State == ObjectState.Stopped)
                        site.Start();
                });

                //iis.WebApplication.Configure();
            }
        }
    }
}
