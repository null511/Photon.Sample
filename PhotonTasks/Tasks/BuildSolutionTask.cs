using Photon.Framework.Tasks;

namespace PhotonTasks
{
    [Roles(Configuration.Roles.Build)]
    class BuildSolutionTask : ITask
    {
        public TaskResult Run(TaskContext context)
        {
            //...

            return TaskResult.Ok();
        }
    }
}
