using Statify.Models;

namespace Statify.Interfaces
{
    public interface IStatisticsService
    {
        public Task<Dictionary<string, int>> GetCalculatedGenreData();
        public Task<AudioFeature> GetCalculatedAudioFeatures();
        public Task<string> GetUserSoundProfileTitle();

    }
}
