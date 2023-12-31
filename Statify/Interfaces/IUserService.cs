﻿using Statify.Models;

namespace Statify.Interfaces
{
    public interface IUserService // All api calls spotify receives originates from this interface
    {
        /// <summary>
        /// Returns a authorized user from Spotify
        /// </summary>
        /// <returns>A deserialized User from json</returns>
        public Task<User> GetUser();
        /// <summary>
        /// User selects # of playlists to receive from API
        /// User must be authorized
        /// </summary>
        /// <param name="amount">number of playlists to return, default 20, max 50</param>
        /// <returns>A deserialized PlayListCollection from json</returns>
        public Task<PlaylistResponse> GetPlaylists(int amount = 20);
        /// <summary>
        /// Send API request to spotify to retrieve top {count} items, either TrackCollection or ArtistCollection. Term = short_term, medium_term or long_term. Default count 20
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="itemType"></param>
        /// <param name="term"></param>
        /// <param name="count"></param>
        /// <returns>top {count} artists or tracks for the specified term.</returns>
        public Task<T> GetTopItems<T>(string itemType, string term, int count = 20, int offset = 0);
        /// <summary>
        /// Retrieves track or tracks from spotify based on endpoint
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="endpoint"></param>
        /// <returns>Singular Track object or TrackCollection</returns>
        public Task<T> GetTracks<T>(string endpoint);
        /// <summary>
        /// Retrieves multiple artists from spotify
        /// </summary>
        /// <param name="ids"></param>
        /// <returns>list of artists based on how many artist-ids are provided</returns>
        Task<ArtistData.ArtistArtists> GetArtists(string ids);
        /// <summary>
        /// Retrieves audio features from spotify
        /// </summary>
        /// <param name="ids"></param>
        /// <returns>array of audio features based on how many song-ids are provided</returns>
        Task <AudioFeatureCollection>GetAudioFeatures(string ids);
    }
}
