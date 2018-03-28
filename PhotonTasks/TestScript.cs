using Photon.Framework.Scripts;
using System.IO;
using System.Threading.Tasks;

namespace PhotonTasks
{
    public class TestScript : IScript
    {
        public async Task<ScriptResult> RunAsync(ScriptContext context)
        {
            var testFileName = Path.Combine(context.WorkDirectory, "test.txt");
            File.WriteAllText(testFileName, "Hello World! This message will self destruct in 10 seconds...");

            await Task.Delay(10_000);

            return ScriptResult.Cancel();
        }
    }
}
