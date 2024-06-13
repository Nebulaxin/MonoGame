// MonoGame - Copyright (C) MonoGame Foundation, Inc
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using System;
using System.Collections.Generic;
#if IOS
using UIKit;
#endif

namespace Microsoft.Xna.Framework.Media
{
    /// <summary>
    /// Provides methods and properties to access the source or sources from which the media will be read.
    /// </summary>
    /// <remarks>
    /// <para>
    /// MediaSource provides access to the source or sources from which the media will be read.
    /// A source can be either the local device, or a device connected through Windows Media Connect.
    /// </para>
    /// </remarks>
	public sealed class MediaSource
    {
        internal MediaSource (string name, MediaSourceType type)
		{
            Name = name;
            MediaSourceType = type;
        }

        /// <summary>
        /// Gets the <see cref="MediaSourceType"/> of this media source.
        /// </summary>
        public Microsoft.Xna.Framework.Media.MediaSourceType MediaSourceType { get; }

        /// <summary>
        /// Gets the name of this media source.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets the available media sources with which a media library can be constructed.
        /// </summary>
        /// <returns>This method will always return a single media source: the local device.</returns>
		public static IList<MediaSource> GetAvailableMediaSources()
        {
#if IOS
			MediaSource[] result = { new MediaSource(UIDevice.CurrentDevice.SystemName, MediaSourceType.LocalDevice) };
#else
            MediaSource[] result = { new("DummpMediaSource", MediaSourceType.LocalDevice) };
#endif
            return result;
        }
    }
}
