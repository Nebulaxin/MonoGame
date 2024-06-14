// MonoGame - Copyright (C) MonoGame Foundation, Inc
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using System;
using System.Diagnostics;

#if OPENGL
#if DESKTOPGL || GLES
using MonoGame.OpenGL;
using GLPixelFormat = MonoGame.OpenGL.PixelFormat;
using PixelFormat = MonoGame.OpenGL.PixelFormat;
#elif ANGLE
using OpenTK.Graphics;
#endif
#endif

namespace Microsoft.Xna.Framework.Graphics
{
    static class GraphicsExtensions
    {
#if OPENGL
        public static int OpenGLNumberOfElements(this VertexElementFormat elementFormat)
        {
            return elementFormat switch
            {
                VertexElementFormat.Single => 1,
                VertexElementFormat.Vector2 => 2,
                VertexElementFormat.Vector3 => 3,
                VertexElementFormat.Vector4 => 4,
                VertexElementFormat.Color => 4,
                VertexElementFormat.Byte4 => 4,
                VertexElementFormat.Short2 => 2,
                VertexElementFormat.Short4 => 4,
                VertexElementFormat.NormalizedShort2 => 2,
                VertexElementFormat.NormalizedShort4 => 4,
                VertexElementFormat.HalfVector2 => 2,
                VertexElementFormat.HalfVector4 => 4,
                _ => throw new ArgumentException(),
            };
        }

        public static VertexPointerType OpenGLVertexPointerType(this VertexElementFormat elementFormat)
        {
            return elementFormat switch
            {
                VertexElementFormat.Single => VertexPointerType.Float,
                VertexElementFormat.Vector2 => VertexPointerType.Float,
                VertexElementFormat.Vector3 => VertexPointerType.Float,
                VertexElementFormat.Vector4 => VertexPointerType.Float,
                VertexElementFormat.Color => VertexPointerType.Short,
                VertexElementFormat.Byte4 => VertexPointerType.Short,
                VertexElementFormat.Short2 => VertexPointerType.Short,
                VertexElementFormat.Short4 => VertexPointerType.Short,
                VertexElementFormat.NormalizedShort2 => VertexPointerType.Short,
                VertexElementFormat.NormalizedShort4 => VertexPointerType.Short,
                VertexElementFormat.HalfVector2 => VertexPointerType.Float,
                VertexElementFormat.HalfVector4 => VertexPointerType.Float,
                _ => throw new ArgumentException(),
            };
        }

		public static VertexAttribPointerType OpenGLVertexAttribPointerType(this VertexElementFormat elementFormat)
        {
            return elementFormat switch
            {
                VertexElementFormat.Single => VertexAttribPointerType.Float,

                VertexElementFormat.Vector2 => VertexAttribPointerType.Float,

                VertexElementFormat.Vector3 => VertexAttribPointerType.Float,

                VertexElementFormat.Vector4 => VertexAttribPointerType.Float,

                VertexElementFormat.Color => VertexAttribPointerType.UnsignedByte,

                VertexElementFormat.Byte4 => VertexAttribPointerType.UnsignedByte,

                VertexElementFormat.Short2 => VertexAttribPointerType.Short,

                VertexElementFormat.Short4 => VertexAttribPointerType.Short,

                VertexElementFormat.NormalizedShort2 => VertexAttribPointerType.Short,

                VertexElementFormat.NormalizedShort4 => VertexAttribPointerType.Short,

#if WINDOWS || DESKTOPGL
                VertexElementFormat.HalfVector2 => VertexAttribPointerType.HalfFloat,

                VertexElementFormat.HalfVector4 => VertexAttribPointerType.HalfFloat,
#endif
                _ => throw new ArgumentException()
            };
        }

        public static bool OpenGLVertexAttribNormalized(this VertexElement element)
        {
            // TODO: This may or may not be the right behavor.  
            //
            // For instance the VertexElementFormat.Byte4 format is not supposed
            // to be normalized, but this line makes it so.
            //
            // The question is in MS XNA are types normalized based on usage or
            // normalized based to their format?
            //
            if (element.VertexElementUsage == VertexElementUsage.Color)
                return true;

            return element.VertexElementFormat switch
            {
                VertexElementFormat.NormalizedShort2 or VertexElementFormat.NormalizedShort4 => true,
                _ => false,
            };
        }

        public static ColorPointerType OpenGLColorPointerType(this VertexElementFormat elementFormat)
        {
            return elementFormat switch
            {
                VertexElementFormat.Single => ColorPointerType.Float,

                VertexElementFormat.Vector2 => ColorPointerType.Float,

                VertexElementFormat.Vector3 => ColorPointerType.Float,

                VertexElementFormat.Vector4 => ColorPointerType.Float,

                VertexElementFormat.Color => ColorPointerType.UnsignedByte,

                VertexElementFormat.Byte4 => ColorPointerType.UnsignedByte,

                VertexElementFormat.Short2 => ColorPointerType.Short,

                VertexElementFormat.Short4 => ColorPointerType.Short,

                VertexElementFormat.NormalizedShort2 => ColorPointerType.UnsignedShort,

                VertexElementFormat.NormalizedShort4 => ColorPointerType.UnsignedShort,

#if MONOMAC
                VertexElementFormat.HalfVector2 => ColorPointerType.HalfFloat,

                VertexElementFormat.HalfVector4 => ColorPointerType.HalfFloat,
#endif
                _ => throw new ArgumentException()
            };
        }

        public static NormalPointerType OpenGLNormalPointerType(this VertexElementFormat elementFormat)
        {
            return elementFormat switch
            {
                VertexElementFormat.Single => NormalPointerType.Float,

                VertexElementFormat.Vector2 => NormalPointerType.Float,

                VertexElementFormat.Vector3 => NormalPointerType.Float,

                VertexElementFormat.Vector4 => NormalPointerType.Float,

                VertexElementFormat.Color => NormalPointerType.Byte,

                VertexElementFormat.Byte4 => NormalPointerType.Byte,

                VertexElementFormat.Short2 => NormalPointerType.Short,

                VertexElementFormat.Short4 => NormalPointerType.Short,

                VertexElementFormat.NormalizedShort2 => NormalPointerType.Short,

                VertexElementFormat.NormalizedShort4 => NormalPointerType.Short,

#if MONOMAC
                VertexElementFormat.HalfVector2 => NormalPointerType.HalfFloat,

                VertexElementFormat.HalfVector4 => NormalPointerType.HalfFloat,
#endif
                _ => throw new ArgumentException()
            };
        }

        public static TexCoordPointerType OpenGLTexCoordPointerType(this VertexElementFormat elementFormat)
        {
            return elementFormat switch
            {
                VertexElementFormat.Single => TexCoordPointerType.Float,

                VertexElementFormat.Vector2 => TexCoordPointerType.Float,

                VertexElementFormat.Vector3 => TexCoordPointerType.Float,

                VertexElementFormat.Vector4 => TexCoordPointerType.Float,

                VertexElementFormat.Color => TexCoordPointerType.Float,

                VertexElementFormat.Byte4 => TexCoordPointerType.Float,

                VertexElementFormat.Short2 => TexCoordPointerType.Short,

                VertexElementFormat.Short4 => TexCoordPointerType.Short,

                VertexElementFormat.NormalizedShort2 => TexCoordPointerType.Short,

                VertexElementFormat.NormalizedShort4 => TexCoordPointerType.Short,

#if MONOMAC
                VertexElementFormat.HalfVector2 => TexCoordPointerType.HalfFloat,

                VertexElementFormat.HalfVector4 => TexCoordPointerType.HalfFloat,
#endif
                _ => throw new ArgumentException()
            };
        }


        public static BlendEquationMode GetBlendEquationMode (this BlendFunction function)
		{
            switch (function)
            {
                case BlendFunction.Add:
                    return BlendEquationMode.FuncAdd;
#if WINDOWS || DESKTOPGL || IOS
                case BlendFunction.Max:
                    return BlendEquationMode.Max;
                case BlendFunction.Min:
                    return BlendEquationMode.Min;
#endif
                case BlendFunction.ReverseSubtract:
                    return BlendEquationMode.FuncReverseSubtract;
                case BlendFunction.Subtract:
                    return BlendEquationMode.FuncSubtract;

                default:
                    throw new ArgumentException();
            }
        }

		public static BlendingFactorSrc GetBlendFactorSrc (this Blend blend)
		{
            return blend switch
            {
                Blend.BlendFactor => BlendingFactorSrc.ConstantColor,
                Blend.DestinationAlpha => BlendingFactorSrc.DstAlpha,
                Blend.DestinationColor => BlendingFactorSrc.DstColor,
                Blend.InverseBlendFactor => BlendingFactorSrc.OneMinusConstantColor,
                Blend.InverseDestinationAlpha => BlendingFactorSrc.OneMinusDstAlpha,
                Blend.InverseDestinationColor => BlendingFactorSrc.OneMinusDstColor,
                Blend.InverseSourceAlpha => BlendingFactorSrc.OneMinusSrcAlpha,
                Blend.InverseSourceColor => BlendingFactorSrc.OneMinusSrcColor,
                Blend.One => BlendingFactorSrc.One,
                Blend.SourceAlpha => BlendingFactorSrc.SrcAlpha,
                Blend.SourceAlphaSaturation => BlendingFactorSrc.SrcAlphaSaturate,
                Blend.SourceColor => BlendingFactorSrc.SrcColor,
                Blend.Zero => BlendingFactorSrc.Zero,
                _ => throw new ArgumentOutOfRangeException(nameof(blend), "The specified blend function is not implemented."),
            };
        }

		public static BlendingFactorDest GetBlendFactorDest (this Blend blend)
		{
            return blend switch
            {
                Blend.BlendFactor => BlendingFactorDest.ConstantColor,
                Blend.DestinationAlpha => BlendingFactorDest.DstAlpha,
                Blend.DestinationColor => BlendingFactorDest.DstColor,
                Blend.InverseBlendFactor => BlendingFactorDest.OneMinusConstantColor,
                Blend.InverseDestinationAlpha => BlendingFactorDest.OneMinusDstAlpha,
                Blend.InverseDestinationColor => BlendingFactorDest.OneMinusDstColor,
                Blend.InverseSourceAlpha => BlendingFactorDest.OneMinusSrcAlpha,
                Blend.InverseSourceColor => BlendingFactorDest.OneMinusSrcColor,
                Blend.One => BlendingFactorDest.One,
                Blend.SourceAlpha => BlendingFactorDest.SrcAlpha,
                Blend.SourceAlphaSaturation => BlendingFactorDest.SrcAlphaSaturate,
                Blend.SourceColor => BlendingFactorDest.SrcColor,
                Blend.Zero => BlendingFactorDest.Zero,
                _ => throw new ArgumentOutOfRangeException(nameof(blend), "The specified blend function is not implemented."),
            };
        }

        public static DepthFunction GetDepthFunction(this CompareFunction compare)
        {
            return compare switch
            {
                CompareFunction.Equal => DepthFunction.Equal,
                CompareFunction.Greater => DepthFunction.Greater,
                CompareFunction.GreaterEqual => DepthFunction.Gequal,
                CompareFunction.Less => DepthFunction.Less,
                CompareFunction.LessEqual => DepthFunction.Lequal,
                CompareFunction.Never => DepthFunction.Never,
                CompareFunction.NotEqual => DepthFunction.Notequal,
                _ => DepthFunction.Always,
            };
        }

#if WINDOWS || DESKTOPGL || ANGLE
        /// <summary>
        /// Convert a <see cref="SurfaceFormat"/> to an OpenTK.Graphics.ColorFormat.
        /// This is used for setting up the backbuffer format of the OpenGL context.
        /// </summary>
        /// <returns>An OpenTK.Graphics.ColorFormat instance.</returns>
        /// <param name="format">The <see cref="SurfaceFormat"/> to convert.</param>
        internal static ColorFormat GetColorFormat(this SurfaceFormat format)
        {
            return format switch
            {
                SurfaceFormat.Alpha8 => new ColorFormat(0, 0, 0, 8),
                SurfaceFormat.Bgr565 => new ColorFormat(5, 6, 5, 0),
                SurfaceFormat.Bgra4444 => new ColorFormat(4, 4, 4, 4),
                SurfaceFormat.Bgra5551 => new ColorFormat(5, 5, 5, 1),
                SurfaceFormat.Bgr32 => new ColorFormat(8, 8, 8, 0),
                SurfaceFormat.Bgra32 or SurfaceFormat.Color or SurfaceFormat.ColorSRgb => new ColorFormat(8, 8, 8, 8),
                SurfaceFormat.Rgba1010102 => new ColorFormat(10, 10, 10, 2),
                _ => throw new NotSupportedException(),// Floating point backbuffers formats could be implemented
                                                       // but they are not typically used on the backbuffer. In
                                                       // those cases it is better to create a render target instead.
            };
        }

        /// <summary>
        /// Converts <see cref="PresentInterval"/> to OpenGL swap interval.
        /// </summary>
        /// <returns>A value according to EXT_swap_control</returns>
        /// <param name="interval">The <see cref="PresentInterval"/> to convert.</param>
        internal static int GetSwapInterval(this PresentInterval interval)
        {
            // See http://www.opengl.org/registry/specs/EXT/swap_control.txt
            // and https://www.opengl.org/registry/specs/EXT/glx_swap_control_tear.txt
            // OpenTK checks for EXT_swap_control_tear:
            // if supported, a swap interval of -1 enables adaptive vsync;
            // otherwise -1 is converted to 1 (vsync enabled.)

            return interval switch
            {
                PresentInterval.Immediate => 0,
                PresentInterval.One => 1,
                PresentInterval.Two => 2,
                _ => -1,
            };
        }
#endif

        const SurfaceFormat InvalidFormat = (SurfaceFormat)int.MaxValue;
		internal static void GetGLFormat (this SurfaceFormat format,
            GraphicsDevice graphicsDevice,
            out PixelInternalFormat glInternalFormat,
            out PixelFormat glFormat,
            out PixelType glType)
		{
			glInternalFormat = PixelInternalFormat.Rgba;
			glFormat = PixelFormat.Rgba;
			glType = PixelType.UnsignedByte;

		    var supportsSRgb = graphicsDevice.GraphicsCapabilities.SupportsSRgb;
            var supportsS3tc = graphicsDevice.GraphicsCapabilities.SupportsS3tc;
            var supportsPvrtc = graphicsDevice.GraphicsCapabilities.SupportsPvrtc;
            var supportsEtc1 = graphicsDevice.GraphicsCapabilities.SupportsEtc1;
            var supportsEtc2 = graphicsDevice.GraphicsCapabilities.SupportsEtc2;
            var supportsAtitc = graphicsDevice.GraphicsCapabilities.SupportsAtitc;
            var supportsFloat = graphicsDevice.GraphicsCapabilities.SupportsFloatTextures;
            var supportsHalfFloat = graphicsDevice.GraphicsCapabilities.SupportsHalfFloatTextures;
            var supportsNormalized = graphicsDevice.GraphicsCapabilities.SupportsNormalized;
            var isGLES2 = GL.BoundApi == GL.RenderApi.ES && graphicsDevice.glMajorVersion == 2;

			switch (format) {
			case SurfaceFormat.Color:
				glInternalFormat = PixelInternalFormat.Rgba;
				glFormat = PixelFormat.Rgba;
				glType = PixelType.UnsignedByte;
				break;
            case SurfaceFormat.ColorSRgb:
                if (!supportsSRgb)
                    goto case SurfaceFormat.Color;
                glInternalFormat = PixelInternalFormat.Srgb;
                glFormat = PixelFormat.Rgba;
                glType = PixelType.UnsignedByte;
                break;
			case SurfaceFormat.Bgr565:
				glInternalFormat = PixelInternalFormat.Rgb;
				glFormat = PixelFormat.Rgb;
				glType = PixelType.UnsignedShort565;
				break;
			case SurfaceFormat.Bgra4444:
#if IOS || ANDROID
				glInternalFormat = PixelInternalFormat.Rgba;
#else
				glInternalFormat = PixelInternalFormat.Rgba4;
#endif
				glFormat = PixelFormat.Rgba;
				glType = PixelType.UnsignedShort4444;
				break;
			case SurfaceFormat.Bgra5551:
				glInternalFormat = PixelInternalFormat.Rgba;
				glFormat = PixelFormat.Rgba;
				glType = PixelType.UnsignedShort5551;
				break;
			case SurfaceFormat.Alpha8:
				glInternalFormat = PixelInternalFormat.Luminance;
				glFormat = PixelFormat.Luminance;
				glType = PixelType.UnsignedByte;
				break;
			case SurfaceFormat.Dxt1:
                if (!supportsS3tc)
                    goto case InvalidFormat;
				glInternalFormat = PixelInternalFormat.CompressedRgbS3tcDxt1Ext;
                glFormat = (PixelFormat)GLPixelFormat.CompressedTextureFormats;
				break;
            case SurfaceFormat.Dxt1SRgb:
                if (!supportsSRgb)
                    goto case SurfaceFormat.Dxt1;
                glInternalFormat = PixelInternalFormat.CompressedSrgbS3tcDxt1Ext;
                glFormat = (PixelFormat)GLPixelFormat.CompressedTextureFormats;
                break;
            case SurfaceFormat.Dxt1a:
                if (!supportsS3tc)
                    goto case InvalidFormat;
                glInternalFormat = PixelInternalFormat.CompressedRgbaS3tcDxt1Ext;
                glFormat = (PixelFormat)GLPixelFormat.CompressedTextureFormats;
                break;
            case SurfaceFormat.Dxt3:
                if (!supportsS3tc)
                    goto case InvalidFormat;
				glInternalFormat = PixelInternalFormat.CompressedRgbaS3tcDxt3Ext;
                glFormat = (PixelFormat)GLPixelFormat.CompressedTextureFormats;
				break;
            case SurfaceFormat.Dxt3SRgb:
                if (!supportsSRgb)
                    goto case SurfaceFormat.Dxt3;
                glInternalFormat = PixelInternalFormat.CompressedSrgbAlphaS3tcDxt3Ext;
                glFormat = (PixelFormat)GLPixelFormat.CompressedTextureFormats;
                break;
			case SurfaceFormat.Dxt5:
                if (!supportsS3tc)
                    goto case InvalidFormat;
				glInternalFormat = PixelInternalFormat.CompressedRgbaS3tcDxt5Ext;
                glFormat = (PixelFormat)GLPixelFormat.CompressedTextureFormats;
				break;
            case SurfaceFormat.Dxt5SRgb:
                if (!supportsSRgb)
                    goto case SurfaceFormat.Dxt5;
                glInternalFormat = PixelInternalFormat.CompressedSrgbAlphaS3tcDxt5Ext;
                glFormat = (PixelFormat)GLPixelFormat.CompressedTextureFormats;
                break;
#if !IOS && !ANDROID && !ANGLE
            case SurfaceFormat.Rgba1010102:
                glInternalFormat = PixelInternalFormat.Rgb10A2ui;
                glFormat = PixelFormat.Rgba;
                glType = PixelType.UnsignedInt1010102;
                break;
#endif
            case SurfaceFormat.Single:
                if (!supportsFloat)
                    goto case InvalidFormat;
                glInternalFormat = PixelInternalFormat.R32f;
                glFormat = PixelFormat.Red;
                glType = PixelType.Float;
                break;

            case SurfaceFormat.HalfVector2:
                if (!supportsHalfFloat)
                    goto case InvalidFormat;
                glInternalFormat = PixelInternalFormat.Rg16f;
				glFormat = PixelFormat.Rg;
				glType = PixelType.HalfFloat;
                break;

            // HdrBlendable implemented as HalfVector4 (see http://blogs.msdn.com/b/shawnhar/archive/2010/07/09/surfaceformat-hdrblendable.aspx)
            case SurfaceFormat.HdrBlendable:
            case SurfaceFormat.HalfVector4:
                if (!supportsHalfFloat)
                    goto case InvalidFormat;
                glInternalFormat = PixelInternalFormat.Rgba16f;
                glFormat = PixelFormat.Rgba;
                glType = PixelType.HalfFloat;
                break;

            case SurfaceFormat.HalfSingle:
                if (!supportsHalfFloat)
                    goto case InvalidFormat;
                glInternalFormat = PixelInternalFormat.R16f;
                glFormat = PixelFormat.Red;
                glType = isGLES2 ? PixelType.HalfFloatOES : PixelType.HalfFloat;
                break;

            case SurfaceFormat.Vector2:
                if (!supportsFloat)
                    goto case InvalidFormat;
                glInternalFormat = PixelInternalFormat.Rg32f;
                glFormat = PixelFormat.Rg;
                glType = PixelType.Float;
                break;

            case SurfaceFormat.Vector4:
                if (!supportsFloat)
                    goto case InvalidFormat;
                glInternalFormat = PixelInternalFormat.Rgba32f;
                glFormat = PixelFormat.Rgba;
                glType = PixelType.Float;
                break;

            case SurfaceFormat.NormalizedByte2:
                glInternalFormat = PixelInternalFormat.Rg8i;
                glFormat = PixelFormat.Rg;
                glType = PixelType.Byte;
                break;

            case SurfaceFormat.NormalizedByte4:
                glInternalFormat = PixelInternalFormat.Rgba8i;
                glFormat = PixelFormat.Rgba;
                glType = PixelType.Byte;
                break;

            case SurfaceFormat.Rg32:
                if (!supportsNormalized)
                    goto case InvalidFormat;
                glInternalFormat = PixelInternalFormat.Rg16ui;
                glFormat = PixelFormat.Rg;
                glType = PixelType.UnsignedShort;
                break;

            case SurfaceFormat.Rgba64:
                if (!supportsNormalized)
                    goto case InvalidFormat;
                glInternalFormat = PixelInternalFormat.Rgba16;
                glFormat = PixelFormat.Rgba;
                glType = PixelType.UnsignedShort;
                break;
            case SurfaceFormat.RgbaAtcExplicitAlpha:
                if (!supportsAtitc)
                    goto case InvalidFormat;
				glInternalFormat = PixelInternalFormat.AtcRgbaExplicitAlphaAmd;
				glFormat = PixelFormat.CompressedTextureFormats;
				break;
            case SurfaceFormat.RgbaAtcInterpolatedAlpha:
                if (!supportsAtitc)
                    goto case InvalidFormat;
				glInternalFormat = PixelInternalFormat.AtcRgbaInterpolatedAlphaAmd;
				glFormat = PixelFormat.CompressedTextureFormats;
				break;
            case SurfaceFormat.RgbEtc1:
                if (!supportsEtc1)
                    goto case InvalidFormat;
                glInternalFormat = PixelInternalFormat.Etc1; // GL_ETC1_RGB8_OES
                glFormat = PixelFormat.CompressedTextureFormats;
                break;
            case SurfaceFormat.Rgb8Etc2:
                if (!supportsEtc2)
                    goto case InvalidFormat;
                glInternalFormat = PixelInternalFormat.Etc2Rgb8; // GL_COMPRESSED_RGB8_ETC2
                glFormat = PixelFormat.CompressedTextureFormats;
                break;
            case SurfaceFormat.Srgb8Etc2:
                if (!supportsEtc2)
                    goto case InvalidFormat;
                glInternalFormat = PixelInternalFormat.Etc2Srgb8; // GL_COMPRESSED_SRGB8_ETC2
                glFormat = PixelFormat.CompressedTextureFormats;
                break;
            case SurfaceFormat.Rgb8A1Etc2:
                if (!supportsEtc2)
                    goto case InvalidFormat;
                glInternalFormat = PixelInternalFormat.Etc2Rgb8A1; // GL_COMPRESSED_RGB8_PUNCHTHROUGH_ALPHA1_ETC2
                glFormat = PixelFormat.CompressedTextureFormats;
                break;
            case SurfaceFormat.Srgb8A1Etc2:
                if (!supportsEtc2)
                    goto case InvalidFormat;
                glInternalFormat = PixelInternalFormat.Etc2Srgb8A1; // GL_COMPRESSED_SRGB8_PUNCHTHROUGH_ALPHA1_ETC2
                glFormat = PixelFormat.CompressedTextureFormats;
                break;
            case SurfaceFormat.Rgba8Etc2:
                if (!supportsEtc2)
                    goto case InvalidFormat;
                glInternalFormat = PixelInternalFormat.Etc2Rgba8Eac; // GL_COMPRESSED_RGBA8_ETC2_EAC
                glFormat = PixelFormat.CompressedTextureFormats;
                break;
            case SurfaceFormat.SRgb8A8Etc2:
                if (!supportsEtc2)
                    goto case InvalidFormat;
                glInternalFormat = PixelInternalFormat.Etc2SRgb8A8Eac; // GL_COMPRESSED_SRGB8_ALPHA8_ETC2_EAC
                glFormat = PixelFormat.CompressedTextureFormats;
                break;
			case SurfaceFormat.RgbPvrtc2Bpp:
                if (!supportsPvrtc)
                    goto case InvalidFormat;
				glInternalFormat = PixelInternalFormat.CompressedRgbPvrtc2Bppv1Img;
				glFormat = PixelFormat.CompressedTextureFormats;
				break;
			case SurfaceFormat.RgbPvrtc4Bpp:
                if (!supportsPvrtc)
                    goto case InvalidFormat;
				glInternalFormat = PixelInternalFormat.CompressedRgbPvrtc4Bppv1Img;
				glFormat = PixelFormat.CompressedTextureFormats;
				break;
			case SurfaceFormat.RgbaPvrtc2Bpp:
                if (!supportsPvrtc)
                    goto case InvalidFormat;
				glInternalFormat = PixelInternalFormat.CompressedRgbaPvrtc2Bppv1Img;
				glFormat = PixelFormat.CompressedTextureFormats;
				break;
			case SurfaceFormat.RgbaPvrtc4Bpp:
                if (!supportsPvrtc)
                    goto case InvalidFormat;
				glInternalFormat = PixelInternalFormat.CompressedRgbaPvrtc4Bppv1Img;
				glFormat = PixelFormat.CompressedTextureFormats;
				break;
            case InvalidFormat: 
            default:
                    throw new NotSupportedException($"The requested SurfaceFormat `{format}` is not supported.");
            }
        }

#endif // OPENGL

        public static int GetSyncInterval(this PresentInterval interval)
        {
            return interval switch
            {
                PresentInterval.Immediate => 0,
                PresentInterval.Two => 2,
                _ => 1,
            };
        }

        public static bool IsCompressedFormat(this SurfaceFormat format)
        {
            return format switch
            {
                SurfaceFormat.Dxt1 or SurfaceFormat.Dxt1a or SurfaceFormat.Dxt1SRgb or SurfaceFormat.Dxt3 or SurfaceFormat.Dxt3SRgb or SurfaceFormat.Dxt5 or SurfaceFormat.Dxt5SRgb or SurfaceFormat.RgbaAtcExplicitAlpha or SurfaceFormat.RgbaAtcInterpolatedAlpha or SurfaceFormat.RgbaPvrtc2Bpp or SurfaceFormat.RgbaPvrtc4Bpp or SurfaceFormat.RgbEtc1 or SurfaceFormat.Rgb8Etc2 or SurfaceFormat.Srgb8Etc2 or SurfaceFormat.Rgb8A1Etc2 or SurfaceFormat.Srgb8A1Etc2 or SurfaceFormat.Rgba8Etc2 or SurfaceFormat.SRgb8A8Etc2 or SurfaceFormat.RgbPvrtc2Bpp or SurfaceFormat.RgbPvrtc4Bpp => true,
                _ => false,
            };
        }

        public static int GetSize(this SurfaceFormat surfaceFormat)
        {
            return surfaceFormat switch
            {
                SurfaceFormat.Dxt1 or SurfaceFormat.Dxt1SRgb or SurfaceFormat.Dxt1a or SurfaceFormat.RgbPvrtc2Bpp or SurfaceFormat.RgbaPvrtc2Bpp or SurfaceFormat.RgbPvrtc4Bpp or SurfaceFormat.RgbaPvrtc4Bpp or SurfaceFormat.RgbEtc1 or SurfaceFormat.Rgb8Etc2 or SurfaceFormat.Srgb8Etc2 or SurfaceFormat.Rgb8A1Etc2 or SurfaceFormat.Srgb8A1Etc2 => 8,// One texel in DXT1, PVRTC (2bpp and 4bpp) and ETC1 is a minimum 4x4 block (8x4 for PVRTC 2bpp), which is 8 bytes
                SurfaceFormat.Dxt3 or SurfaceFormat.Dxt3SRgb or SurfaceFormat.Dxt5 or SurfaceFormat.Dxt5SRgb or SurfaceFormat.RgbaAtcExplicitAlpha or SurfaceFormat.RgbaAtcInterpolatedAlpha or SurfaceFormat.Rgba8Etc2 or SurfaceFormat.SRgb8A8Etc2 => 16,// One texel in DXT3 and DXT5 is a minimum 4x4 block, which is 16 bytes
                SurfaceFormat.Alpha8 => 1,
                SurfaceFormat.Bgr565 or SurfaceFormat.Bgra4444 or SurfaceFormat.Bgra5551 or SurfaceFormat.HalfSingle or SurfaceFormat.NormalizedByte2 => 2,
                SurfaceFormat.Color or SurfaceFormat.ColorSRgb or SurfaceFormat.Single or SurfaceFormat.Rg32 or SurfaceFormat.HalfVector2 or SurfaceFormat.NormalizedByte4 or SurfaceFormat.Rgba1010102 or SurfaceFormat.Bgra32 or SurfaceFormat.Bgra32SRgb or SurfaceFormat.Bgr32 or SurfaceFormat.Bgr32SRgb => 4,
                SurfaceFormat.HalfVector4 or SurfaceFormat.Rgba64 or SurfaceFormat.Vector2 => 8,
                SurfaceFormat.Vector4 => 16,
                _ => throw new ArgumentException(),
            };
        }

        public static int GetSize(this VertexElementFormat elementFormat)
        {
            return elementFormat switch
            {
                VertexElementFormat.Single => 4,
                VertexElementFormat.Vector2 => 8,
                VertexElementFormat.Vector3 => 12,
                VertexElementFormat.Vector4 => 16,
                VertexElementFormat.Color => 4,
                VertexElementFormat.Byte4 => 4,
                VertexElementFormat.Short2 => 4,
                VertexElementFormat.Short4 => 8,
                VertexElementFormat.NormalizedShort2 => 4,
                VertexElementFormat.NormalizedShort4 => 8,
                VertexElementFormat.HalfVector2 => 4,
                VertexElementFormat.HalfVector4 => 8,
                _ => 0,
            };
        }

        public static void GetBlockSize(this SurfaceFormat surfaceFormat, out int width, out int height)
        {
            switch (surfaceFormat)
            {
                case SurfaceFormat.RgbPvrtc2Bpp:
                case SurfaceFormat.RgbaPvrtc2Bpp:
                    width = 8;
                    height = 4;
                    break;
                case SurfaceFormat.Dxt1:
                case SurfaceFormat.Dxt1SRgb:
                case SurfaceFormat.Dxt1a:
                case SurfaceFormat.Dxt3:
                case SurfaceFormat.Dxt3SRgb:
                case SurfaceFormat.Dxt5:
                case SurfaceFormat.Dxt5SRgb:
                case SurfaceFormat.RgbPvrtc4Bpp:
                case SurfaceFormat.RgbaPvrtc4Bpp:
                case SurfaceFormat.RgbEtc1:
                case SurfaceFormat.Rgb8Etc2:
                case SurfaceFormat.Srgb8Etc2:
                case SurfaceFormat.Rgb8A1Etc2:
                case SurfaceFormat.Srgb8A1Etc2:
                case SurfaceFormat.Rgba8Etc2:
                case SurfaceFormat.SRgb8A8Etc2:
                case SurfaceFormat.RgbaAtcExplicitAlpha:
                case SurfaceFormat.RgbaAtcInterpolatedAlpha:
                    width = 4;
                    height = 4;
                    break;
                default:
                    width = 1;
                    height = 1;
                    break;
            }
        }

#if OPENGL

        public static int GetBoundTexture2D()
        {
            GL.GetInteger(GetPName.TextureBinding2D, out int prevTexture);
            GraphicsExtensions.LogGLError("GraphicsExtensions.GetBoundTexture2D() GL.GetInteger");
            return prevTexture;
        }

        [Conditional("DEBUG")]
		[DebuggerHidden]
        public static void CheckGLError()
        {
           var error = GL.GetError();
            //Console.WriteLine(error);
            if (error != ErrorCode.NoError)
                throw new MonoGameGLException($"GL.GetError() returned {error}");
        }
#endif

#if OPENGL
        [Conditional("DEBUG")]
        public static void LogGLError(string location)
        {
            try
            {
                GraphicsExtensions.CheckGLError();
            }
            catch (MonoGameGLException ex)
            {
#if ANDROID
                // Todo: Add generic MonoGame logging interface
                Android.Util.Log.Debug($"MonoGame", "MonoGameGLException at " + location + " - {ex.Message}");
#else
                Debug.WriteLine($"MonoGameGLException at " + location + " - {ex.Message}");
#endif
            }
        }
#endif
    }

    internal class MonoGameGLException : Exception
    {
        public MonoGameGLException(string message)
            : base(message)
        {
        }
    }
}
