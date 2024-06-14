// MonoGame - Copyright (C) MonoGame Foundation, Inc
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using System;
using System.Diagnostics;
using System.Threading;

namespace Microsoft.Xna.Framework.Graphics
{
    /// <summary>
    /// Represents a texture resource
    /// </summary>
	public abstract partial class Texture : GraphicsResource
    {
        private static int _lastSortingKey;

        /// <summary>
        /// Gets a unique identifier of this texture for sorting purposes.
        /// </summary>
        /// <remarks>
        /// <para>For example, this value is used by <see cref="SpriteBatch"/> when drawing with <see cref="SpriteSortMode.Texture"/>.</para>
        /// <para>The value is an implementation detail and may change between application launches or MonoGame versions.
        /// It is only guaranteed to stay consistent during application lifetime.</para>
        /// </remarks>
        internal int SortingKey { get; } = Interlocked.Increment(ref _lastSortingKey);

        /// <summary>
        /// Gets the surface format used by this <b>Texture</b>.
        /// </summary>
        public SurfaceFormat Format { get; internal set; }

        /// <summary>
        /// Gets the number of mipmap levels in this <b>Texture</b>.
        /// </summary>
        public int LevelCount { get; internal set; }

        internal static int CalculateMipLevels(int width, int height = 0, int depth = 0)
        {
            int levels = 1;
            int size = Math.Max(Math.Max(width, height), depth);
            while (size > 1)
            {
                size = size / 2;
                levels++;
            }
            return levels;
        }

        internal static void GetSizeForLevel(int width, int height, int level, out int w, out int h)
        {
            w = width;
            h = height;
            while (level > 0)
            {
                --level;
                w /= 2;
                h /= 2;
            }
            if (w == 0)
                w = 1;
            if (h == 0)
                h = 1;
        }

        internal static void GetSizeForLevel(int width, int height, int depth, int level, out int w, out int h, out int d)
        {
            w = width;
            h = height;
            d = depth;
            while (level > 0)
            {
                --level;
                w /= 2;
                h /= 2;
                d /= 2;
            }
            if (w == 0)
                w = 1;
            if (h == 0)
                h = 1;
            if (d == 0)
                d = 1;
        }

        internal int GetPitch(int width)
        {
            Debug.Assert(width > 0, "The width is negative!");
            var pitch = Format switch
            {
                SurfaceFormat.Dxt1 or SurfaceFormat.Dxt1SRgb or SurfaceFormat.Dxt1a or SurfaceFormat.RgbPvrtc2Bpp or SurfaceFormat.RgbaPvrtc2Bpp or
                SurfaceFormat.RgbEtc1 or SurfaceFormat.Rgb8Etc2 or SurfaceFormat.Srgb8Etc2 or SurfaceFormat.Rgb8A1Etc2 or SurfaceFormat.Srgb8A1Etc2 or
                SurfaceFormat.Dxt3 or SurfaceFormat.Dxt3SRgb or SurfaceFormat.Dxt5 or SurfaceFormat.Dxt5SRgb or SurfaceFormat.RgbPvrtc4Bpp or
                SurfaceFormat.RgbaPvrtc4Bpp => ((width + 3) / 4) * Format.GetSize(),
                _ => width * Format.GetSize(),
            };

            return pitch;
        }

        /// <inheritdoc />
        internal protected override void GraphicsDeviceResetting()
        {
            PlatformGraphicsDeviceResetting();
        }
    }
}

