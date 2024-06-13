// MonoGame - Copyright (C) MonoGame Foundation, Inc
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using System;
using System.Collections.Generic;

namespace Microsoft.Xna.Framework.Media
{
    /// <summary>
    /// Provides methods and properties to access and control the queue of playing songs.
    /// </summary>
    /// <remarks>
    /// <para>
    /// MediaQueue is a read-only queue of songs.
    /// With MediaQueue, you can control which song is playing in the queue,
    /// but you cannot add or remove songs from the queue.
    /// Either <see cref="MediaPlayer.Play(Song)">MediaPlayer.Play(Song)</see> or the songs already queued up
    /// when the game starts determine the songs that are in the queue of playing songs.
    /// </para>
    /// </remarks>
	public sealed class MediaQueue
	{
        List<Song> songs = new();
        private Random random = new();

        /// <summary>
        /// Creates a new instance of <see cref="MediaQueue"/>.
        /// </summary>
		public MediaQueue()
		{

		}

        /// <summary>
        /// Gets the current <see cref="Song"/> in the queue of playing songs.
        /// </summary>
		public Song ActiveSong
		{
			get
			{
                if (songs.Count == 0 || ActiveSongIndex < 0)
                    return null;

                return songs[ActiveSongIndex];
            }
		}

        /// <summary>
        /// Gets or sets the index of the current (active) song in the queue of playing songs.
        /// </summary>
        /// <remarks>
        /// Changing the active song index does not alter the current media state (playing, paused, or stopped).
        /// </remarks>
		public int ActiveSongIndex { get; set; } = -1;

        /// <summary>
        /// Gets the count of songs in the MediaQueue.
        /// </summary>
        internal int Count => songs.Count;

        /// <summary>
        /// Gets the <see cref="Song"/> at the specified index in the MediaQueue
        /// </summary>
        public Song this[int index] => songs[index];

        internal IEnumerable<Song> Songs => songs;

        internal Song GetNextSong(int direction, bool shuffle)
        {
            if (shuffle)
                ActiveSongIndex = random.Next(songs.Count);
            else
                ActiveSongIndex = (int)MathHelper.Clamp(ActiveSongIndex + direction, 0, songs.Count - 1);

            return songs[ActiveSongIndex];
        }

		internal void Clear()
		{
			Song song;
			for(; songs.Count > 0; )
			{
				song = songs[0];
#if !DIRECTX && !NATIVE
				song.Stop();
#endif
				songs.Remove(song);
			}
		}

#if !DIRECTX && !NATIVE
        internal void SetVolume(float volume)
        {
            int count = songs.Count;
            for (int i = 0; i < count; ++i)
                songs[i].Volume = volume;
        }
#endif

        internal void Add(Song song)
        {
            songs.Add(song);
        }

#if !DIRECTX && !NATIVE
        internal void Stop()
        {
            int count = songs.Count;
            for (int i = 0; i < count; ++i)
                songs[i].Stop();
        }
#endif
	}
}

