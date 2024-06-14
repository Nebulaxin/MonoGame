// MonoGame - Copyright (C) MonoGame Foundation, Inc
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using System;
using System.Runtime.InteropServices;
using MonoGame.OpenGL;
using GLPixelFormat = MonoGame.OpenGL.PixelFormat;
using MonoGame.Framework.Utilities;

namespace Microsoft.Xna.Framework.Graphics
{
    public partial class TextureCube
    {
        private void PlatformConstruct(GraphicsDevice graphicsDevice, int size, bool mipMap, SurfaceFormat format, bool renderTarget)
        {
            glTarget = TextureTarget.TextureCubeMap;

            Threading.BlockOnUIThread(() =>
            {
                GL.GenTextures(1, out glTexture);
                GraphicsExtensions.CheckGLError();

                GL.BindTexture(TextureTarget.TextureCubeMap, glTexture);
                GraphicsExtensions.CheckGLError();

                GL.TexParameter(
                    TextureTarget.TextureCubeMap, TextureParameterName.TextureMinFilter,
                    mipMap ? (int)TextureMinFilter.LinearMipmapLinear : (int)TextureMinFilter.Linear);
                GraphicsExtensions.CheckGLError();

                GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
                GraphicsExtensions.CheckGLError();

                GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureWrapS, (int)TextureWrapMode.ClampToEdge);
                GraphicsExtensions.CheckGLError();

                GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureWrapT, (int)TextureWrapMode.ClampToEdge);
                GraphicsExtensions.CheckGLError();

                format.GetGLFormat(GraphicsDevice, out glInternalFormat, out glFormat, out glType);

                for (var i = 0; i < 6; i++)
                {
                    var target = GetGLCubeFace((CubeMapFace)i);

                    if (glFormat == GLPixelFormat.CompressedTextureFormats)
                    {
                        var imageSize = 0;
                        imageSize = format switch
                        {
                            SurfaceFormat.RgbPvrtc2Bpp or SurfaceFormat.RgbaPvrtc2Bpp => (Math.Max(size, 16) * Math.Max(size, 8) * 2 + 7) / 8,
                            SurfaceFormat.RgbPvrtc4Bpp or SurfaceFormat.RgbaPvrtc4Bpp => (Math.Max(size, 8) * Math.Max(size, 8) * 4 + 7) / 8,
                            SurfaceFormat.Dxt1 or SurfaceFormat.Dxt1a or SurfaceFormat.Dxt1SRgb or SurfaceFormat.Dxt3 or SurfaceFormat.Dxt3SRgb or SurfaceFormat.Dxt5 or SurfaceFormat.Dxt5SRgb or SurfaceFormat.RgbEtc1 or SurfaceFormat.Rgb8Etc2 or SurfaceFormat.Srgb8Etc2 or SurfaceFormat.Rgb8A1Etc2 or SurfaceFormat.Srgb8A1Etc2 or SurfaceFormat.Rgba8Etc2 or SurfaceFormat.SRgb8A8Etc2 or SurfaceFormat.RgbaAtcExplicitAlpha or SurfaceFormat.RgbaAtcInterpolatedAlpha => (size + 3) / 4 * ((size + 3) / 4) * format.GetSize(),
                            _ => throw new NotSupportedException(),
                        };
                        GL.CompressedTexImage2D(target, 0, glInternalFormat, size, size, 0, imageSize, IntPtr.Zero);
                        GraphicsExtensions.CheckGLError();
                    }
                    else
                    {
                        GL.TexImage2D(target, 0, glInternalFormat, size, size, 0, glFormat, glType, IntPtr.Zero);
                        GraphicsExtensions.CheckGLError();
                    }
                }

                if (mipMap)
                {
#if IOS || ANDROID
                    GL.GenerateMipmap(GenerateMipmapTarget.TextureCubeMap);
#else
                    GraphicsDevice.FramebufferHelper.Get().GenerateMipmap((int)glTarget);
                    // This updates the mipmaps after a change in the base texture
                    GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.GenerateMipmap, (int)Bool.True);
#endif
                    GraphicsExtensions.CheckGLError();
                }
            });
        }

        private void PlatformGetData<T>(
            CubeMapFace cubeMapFace, int level, Rectangle rect, T[] data, int startIndex, int elementCount)
            where T : struct
        {
            Threading.EnsureUIThread();

#if OPENGL && DESKTOPGL
            var target = GetGLCubeFace(cubeMapFace);
            var tSizeInByte = ReflectionHelpers.SizeOf<T>.Get();
            GL.BindTexture(TextureTarget.TextureCubeMap, glTexture);

            if (glFormat == GLPixelFormat.CompressedTextureFormats)
            {
                // Note: for compressed format Format.GetSize() returns the size of a 4x4 block
                var pixelToT = Format.GetSize() / tSizeInByte;
                var tFullWidth = Math.Max(size >> level, 1) / 4 * pixelToT;
                var temp = new T[Math.Max(size >> level, 1) / 4 * tFullWidth];
                GL.GetCompressedTexImage(target, level, temp);
                GraphicsExtensions.CheckGLError();

                var rowCount = rect.Height / 4;
                var tRectWidth = rect.Width / 4 * Format.GetSize() / tSizeInByte;
                for (var r = 0; r < rowCount; r++)
                {
                    var tempStart = rect.X / 4 * pixelToT + (rect.Top / 4 + r) * tFullWidth;
                    var dataStart = startIndex + r * tRectWidth;
                    Array.Copy(temp, tempStart, data, dataStart, tRectWidth);
                }
            }
            else
            {
                // we need to convert from our format size to the size of T here
                var tFullWidth = Math.Max(size >> level, 1) * Format.GetSize() / tSizeInByte;
                var temp = new T[Math.Max(size >> level, 1) * tFullWidth];
                GL.GetTexImage(target, level, glFormat, glType, temp);
                GraphicsExtensions.CheckGLError();

                var pixelToT = Format.GetSize() / tSizeInByte;
                var rowCount = rect.Height;
                var tRectWidth = rect.Width * pixelToT;
                for (var r = 0; r < rowCount; r++)
                {
                    var tempStart = rect.X * pixelToT + (r + rect.Top) * tFullWidth;
                    var dataStart = startIndex + r * tRectWidth;
                    Array.Copy(temp, tempStart, data, dataStart, tRectWidth);
                }
            }
#else
            throw new NotImplementedException();
#endif
        }

        private void PlatformSetData<T>(CubeMapFace face, int level, Rectangle rect, T[] data, int startIndex, int elementCount)
        {
            Threading.BlockOnUIThread(() =>
            {
                var elementSizeInByte = ReflectionHelpers.SizeOf<T>.Get();
                var dataHandle = GCHandle.Alloc(data, GCHandleType.Pinned);
                // Use try..finally to make sure dataHandle is freed in case of an error
                try
                {
                    var startBytes = startIndex * elementSizeInByte;
                    var dataPtr = new IntPtr(dataHandle.AddrOfPinnedObject().ToInt64() + startBytes);

                    GL.BindTexture(TextureTarget.TextureCubeMap, glTexture);
                    GraphicsExtensions.CheckGLError();

                    var target = GetGLCubeFace(face);
                    if (glFormat == GLPixelFormat.CompressedTextureFormats)
                    {
                        GL.CompressedTexSubImage2D(
                            target, level, rect.X, rect.Y, rect.Width, rect.Height,
                            glInternalFormat, elementCount * elementSizeInByte, dataPtr);
                        GraphicsExtensions.CheckGLError();
                    }
                    else
                    {
                        GL.TexSubImage2D(
                            target, level, rect.X, rect.Y, rect.Width, rect.Height, glFormat, glType, dataPtr);
                        GraphicsExtensions.CheckGLError();
                    }
                }
                finally
                {
                    dataHandle.Free();
                }
            });
        }

        private TextureTarget GetGLCubeFace(CubeMapFace face)
        {
            return face switch
            {
                CubeMapFace.PositiveX => TextureTarget.TextureCubeMapPositiveX,
                CubeMapFace.NegativeX => TextureTarget.TextureCubeMapNegativeX,
                CubeMapFace.PositiveY => TextureTarget.TextureCubeMapPositiveY,
                CubeMapFace.NegativeY => TextureTarget.TextureCubeMapNegativeY,
                CubeMapFace.PositiveZ => TextureTarget.TextureCubeMapPositiveZ,
                CubeMapFace.NegativeZ => TextureTarget.TextureCubeMapNegativeZ,
                _ => throw new ArgumentException(),
            };
        }
    }
}

