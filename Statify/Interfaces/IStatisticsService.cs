using Statify.Models;

namespace Statify.Interfaces
{
    public interface IStatisticsService
    {
        public Task<Dictionary<string, int>> ToBeDecided();
    }
}
