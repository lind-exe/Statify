using Statify.Models;

namespace Statify.Interfaces
{
    public interface IStatisticsService
    {   
        /// <summary>
        /// Fetches neccessary data and calculates genre data based on multiple API calls to spotify
        /// </summary>
        /// <returns>dictionary with genre as key and score as value</returns>
        public Task<Dictionary<string, int>?> GetCalculatedGenreData();
        /// <summary>
        /// Fetches neccessary data and calculates audio features based on the users top 50 songs
        /// </summary>
        /// <returns>an object containing each audiofeature as a property</returns>
        public Task<AudioFeature> GetCalculatedAudioFeatures();
        /// <summary>
        /// Determines the users sound profile title based on top 50 songs audio features
        /// </summary>
        /// <returns>title as a string</returns>
        public Task<string> GetUserSoundProfileTitle();
        /// <summary>
        /// Summarizes duration of every song from the users top 50 songs
        /// </summary>
        /// <returns>HH:MM:SS timespan object of total duration</returns>
        public TimeSpan GetTotalDurationOfTopSongs();
    }
}
