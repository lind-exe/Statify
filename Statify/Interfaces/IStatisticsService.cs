using Statify.Models;

namespace Statify.Interfaces
{
    public interface IStatisticsService
    {
        public Task<Dictionary<string, int>> GetCalculatedGenreData();

        public Task<TrackData.LikedTracks> GetLikedSongs(int limit, int offset);

        public Task<Track> GetTrack(string trackId);

    }
}
