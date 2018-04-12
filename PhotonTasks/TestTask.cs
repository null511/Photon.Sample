using Photon.Framework.Agent;
using Photon.Framework.Tasks;
using System;
using System.Threading.Tasks;

namespace PhotonTasks
{
    public class TestTask : IBuildTask
    {
        public IAgentBuildContext Context {get; set;}


        public async Task<TaskResult> RunAsync()
        {
            Context.Output.AppendLine("Hello World!", ConsoleColor.White);

            await Task.Delay(2_000);
            Context.Output.AppendLine("3...", ConsoleColor.DarkRed);
            await Task.Delay(1_000);
            Context.Output.AppendLine("2...", ConsoleColor.DarkGreen);
            await Task.Delay(1_000);
            Context.Output.AppendLine("1...", ConsoleColor.DarkBlue);
            await Task.Delay(1_000);

            Context.Output.AppendLine("Complete!", ConsoleColor.Green);

            return TaskResult.Ok("Alright Alright Alright");
        }
    }
}
