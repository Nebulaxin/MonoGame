// MonoGame - Copyright (C) MonoGame Foundation, Inc
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using System;
using System.IO;
using Windows.Storage;

namespace Microsoft.Xna.Framework.Media
{
    public sealed partial class Song
    {
        private Album album;
        private Artist artist;
        private Genre genre;
        
		private MusicProperties musicProperties;

        public StorageFile StorageFile => musicProperties.File;

        internal Song(Album album, Artist artist, Genre genre, MusicProperties musicProperties)
        {
            this.album = album;
            this.artist = artist;
            this.genre = genre;
            this.musicProperties = musicProperties;
		}

		private void PlatformInitialize(string fileName)
        {

        }

        private void PlatformDispose(bool disposing)
        {

        }

        private Album PlatformGetAlbum()
        {
            return album;
        }

        private void PlatformSetAlbum(Album album)
        {
            this.album = album;
        }

        private Artist PlatformGetArtist()
        {
            return artist;
        }

        private Genre PlatformGetGenre()
        {
            return genre;
        }

        private TimeSpan PlatformGetDuration()
        {
            if (musicProperties != null)
                return musicProperties.Duration;

            return _duration;
        }

        private bool PlatformIsProtected()
        {
            if (musicProperties != null)
                return musicProperties.IsProtected;

            return false;
        }

        private bool PlatformIsRated()
        {
            if (musicProperties != null)
                return musicProperties.Rating != 0;

            return false;
        }

        private string PlatformGetName()
        {
            if (musicProperties != null)
                return musicProperties.Title;

            return Path.GetFileNameWithoutExtension(_name);
        }

        private int PlatformGetPlayCount()
        {
            return _playCount;
        }

        private int PlatformGetRating()
        {
            if (musicProperties != null)
                return musicProperties.Rating;

            return 0;
        }

        private int PlatformGetTrackNumber()
        {
            if (musicProperties != null)
                return musicProperties.TrackNumber;

            return 0;
        }
    }
}