using System.Threading.Tasks;

namespace FountainBlue.Scripting
{
    public interface IScript
    {
        /// <summary>
        ///     Executes this instance asynchronously.
        /// </summary>
        Task ExecuteAsync();
    }
}