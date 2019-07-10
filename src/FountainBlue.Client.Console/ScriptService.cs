using System.Threading.Tasks;
using CSScriptLibrary;
using FountainBlue.Scripting;

namespace FountainBlue.Client.Console
{
    internal class ScriptService
    {
        /// <summary>
        ///     Executes the specified script asynchronously.
        /// </summary>
        /// <param name="script">The script.</param>
        public async Task ExecuteAsync(string script)
        {
            CSScript.EvaluatorConfig.Engine = EvaluatorEngine.CodeDom;
            CSScript.GlobalSettings.InMemoryAssembly = false;

            var codeScript = await CSScript.MonoEvaluator.LoadCodeAsync<IScript>(script);
            await codeScript.ExecuteAsync();
        }
    }
}