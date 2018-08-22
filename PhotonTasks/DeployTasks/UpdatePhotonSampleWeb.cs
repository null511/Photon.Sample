using Microsoft.Web.Administration;
using Photon.Framework.Agent;
using Photon.Framework.Tasks;
using Photon.Plugins.IIS;
using System;
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
            var appRev = await Context.Applications.GetApplicationRevision(
                projectId: Context.Project.Id,
                appName: Configuration.Apps.Web.AppName,
                deploymentNumber: Context.DeploymentNumber,
                token: token);

            if (appRev == null) throw new ApplicationException($"Application revision directory not found for app '{Configuration.Apps.Web.AppName}' revision '{Context.DeploymentNumber}'!");

            using (var iis = new IISTools(Context)) {
                await iis.ApplicationPool.ConfigureAsync(Configuration.AppPoolName, pool => {
                    // Configure AppPool
                    pool.AutoStart = true;
                    pool.ManagedPipelineMode = ManagedPipelineMode.Integrated;
                    pool.ManagedRuntimeVersion = "v4.0";

                    // Start Website
                    if (pool.State == ObjectState.Stopped)
                        pool.Start();
                }, token);

                await iis.WebSite.ConfigureAsync("Photon Web", 8086, site => {
                    // Configure Website
                    site.ApplicationDefaults.ApplicationPoolName = Configuration.AppPoolName;
                    site.ServerAutoStart = true;

                    // Set Bindings
                    site.Bindings.Clear();
                    site.Bindings.Add("*:8086:localhost", "http");

                    // Update Virtual Path
                    site.Applications[0]
                        .VirtualDirectories["/"]
                        .PhysicalPath = appRev.ApplicationPath;

                    // Start Website
                    if (site.State == ObjectState.Stopped)
                        site.Start();
                }, token);
            }
        }
    }
}
