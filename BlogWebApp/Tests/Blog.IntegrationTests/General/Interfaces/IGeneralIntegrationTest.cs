using System.Threading.Tasks;

namespace Blog.IntegrationTests.General.Interfaces
{
    /// <summary>
    /// General integration test interface.
    /// </summary>
    public interface IGeneralIntegrationTest
    {
        /// <summary>
        /// Authenticates the asynchronous.
        /// </summary>
        /// <returns>Task.</returns>
        Task AuthenticateAsync();
    }
}