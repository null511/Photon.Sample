using Photon.Framework.Tasks;
using System;
using System.Threading.Tasks;

namespace PhotonTasks
{
    public class TestTask : IBuildTask
    {
        public async Task<TaskResult> RunAsync(IAgentBuildContext context)
        {
            context.Output.AppendLine("Hello World!", ConsoleColor.White);

            await Task.Delay(2_000);
            context.Output.AppendLine("3...", ConsoleColor.DarkRed);
            await Task.Delay(1_000);
            context.Output.AppendLine("2...", ConsoleColor.DarkGreen);
            await Task.Delay(1_000);
            context.Output.AppendLine("1...", ConsoleColor.DarkBlue);
            await Task.Delay(1_000);

            context.Output.AppendLine("Complete!", ConsoleColor.Green);

            //var testFileName = Path.Combine(context.WorkDirectory, "test.txt");
            //File.WriteAllText(testFileName, "Hello World! This message will self destruct in 10 seconds...");

            return TaskResult.Ok("Alright Alright Alright");
        }
    }
}
