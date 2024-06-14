// MonoGame - Copyright (C) MonoGame Foundation, Inc
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using System;

namespace Microsoft.Xna.Framework.Graphics
{
    /// <summary>
    /// Defines the blend state for a single render target.
    /// </summary>
	public class TargetBlendState
	{
	    private readonly BlendState _parent;
	    private BlendFunction _alphaBlendFunction;
	    private Blend _alphaDestinationBlend;
	    private Blend _alphaSourceBlend;
	    private BlendFunction _colorBlendFunction;
	    private Blend _colorDestinationBlend;
	    private Blend _colorSourceBlend;
	    private ColorWriteChannels _colorWriteChannels;

	    internal TargetBlendState(BlendState parent)
        {
	        _parent = parent;
	        AlphaBlendFunction = BlendFunction.Add;
            AlphaDestinationBlend = Blend.Zero;
            AlphaSourceBlend = Blend.One;
            ColorBlendFunction = BlendFunction.Add;
            ColorDestinationBlend = Blend.Zero;
            ColorSourceBlend = Blend.One;
            ColorWriteChannels = ColorWriteChannels.All;
        }

	    internal TargetBlendState Clone(BlendState parent)
	    {
	        return new TargetBlendState(parent)
	        {
	            AlphaBlendFunction = AlphaBlendFunction,
                AlphaDestinationBlend = AlphaDestinationBlend,
                AlphaSourceBlend = AlphaSourceBlend,
                ColorBlendFunction = ColorBlendFunction,
                ColorDestinationBlend = ColorDestinationBlend,
                ColorSourceBlend = ColorSourceBlend,
                ColorWriteChannels = ColorWriteChannels
	        };
	    }

        /// <summary>
        /// Gets or Sets the blend function for the alpha component.
        /// </summary>
        /// <remarks>
        /// This property specifies the blending operation that will be used to combine the alpha components
        /// of the source and destination pixels. The blend function affects how the final alpha value is calculated.
        /// </remarks>
        /// <exception cref="InvalidOperationException">
        /// The exception is thrown if you attempt to change the property after the <see cref="BlendState"/> has been
        /// bound to the graphics device.
        /// </exception>
	    public BlendFunction AlphaBlendFunction
        {
            get => _alphaBlendFunction;
            set
            {
                _parent.ThrowIfBound();
                _alphaBlendFunction = value;
            }
        }

        /// <summary>
        /// Gets or Sets the blend factor for the alpha component of the destination pixel.
        /// </summary>
        /// <remarks>
        /// This property specifies the blend factor that will be used for the destination alpha component
        /// when combining the source and destination pixels. The blend factor affects how the final alpha value is
        /// calculated.
        /// </remarks>
        /// <exception cref="InvalidOperationException">
        /// The exception is thrown if you attempt to change the property after the <see cref="BlendState"/> has been
        /// bound to the graphics device.
        /// </exception>
	    public Blend AlphaDestinationBlend
        {
            get => _alphaDestinationBlend;
            set
            {
                _parent.ThrowIfBound();
                _alphaDestinationBlend = value;
            }
        }

        /// <summary>
        /// Gets or Sets the blend factor for the alpha component of the source pixel.
        /// </summary>
        /// <remarks>
        /// This property specifies the blend factor that will be used for the source alpha component
        /// when combining the source and destination pixels. The blend factor affects how the final alpha value is
        /// calculated.
        /// </remarks>
        /// <exception cref="InvalidOperationException">
        /// The exception is thrown if you attempt to change the property after the <see cref="BlendState"/> has been
        /// bound to the graphics device.
        /// </exception>
	    public Blend AlphaSourceBlend
        {
            get => _alphaSourceBlend;
            set
            {
                _parent.ThrowIfBound();
                _alphaSourceBlend = value;
            }
        }

        /// <summary>
        /// Gets or Sets the blend function for the color components (red, green, and blue).
        /// </summary>
        /// <remarks>
        /// This property specifies the blending operation that will be used to combine the color components
        /// (red, green, and blue) of the source and destination pixels. The blend function affects how the final color
        /// values are calculated.
        /// </remarks>
        /// <exception cref="InvalidOperationException">
        /// The exception is thrown if you attempt to change the property after the <see cref="BlendState"/> has been
        /// bound to the graphics device.
        /// </exception>
	    public BlendFunction ColorBlendFunction
        {
            get => _colorBlendFunction;
            set
            {
                _parent.ThrowIfBound();
                _colorBlendFunction = value;
            }
        }

        /// <summary>
        /// Gets or Sets the blend factor for the color components (red, green, and blue) of the destination pixel.
        /// </summary>
        /// <remarks>
        /// This property specifies the blend factor that will be used for the destination color components
        /// when combining the source and destination pixels. The blend factor affects how the final color values are
        /// calculated.
        /// </remarks>
        /// <exception cref="InvalidOperationException">
        /// The exception is thrown if you attempt to change the property after the <see cref="BlendState"/> has been
        /// bound to the graphics device.
        /// </exception>
	    public Blend ColorDestinationBlend
        {
            get => _colorDestinationBlend;
            set
            {
                _parent.ThrowIfBound();
                _colorDestinationBlend = value;
            }
        }

        /// <summary>
        /// Gets or Sets the blend factor for the color components (red, green, and blue) of the source pixel.
        /// </summary>
        /// <remarks>
        /// This property specifies the blend factor that will be used for the source color components
        /// when combining the source and destination pixels. The blend factor affects how the final color values are
        /// calculated.
        /// </remarks>
        /// <exception cref="InvalidOperationException">
        /// The exception is thrown if you attempt to change the property after the <see cref="BlendState"/> has been
        /// bound to the graphics device.
        /// </exception>
	    public Blend ColorSourceBlend
        {
            get => _colorSourceBlend;
            set
            {
                _parent.ThrowIfBound();
                _colorSourceBlend = value;
            }
        }

        /// <summary>
        /// Gets or Sets a value that determines which color channels to enable for writing to the render target.
        /// </summary>
        /// <remarks>
        /// This property controls which color channels (red, green, blue, and alpha) are enabled for writing
        /// to the render target during the blending operation. By default, all color channels are enabled for writing.
        /// </remarks>
        /// <exception cref="InvalidOperationException">
        /// The exception is thrown if you attempt to change the property after the <see cref="BlendState"/> has been
        /// bound to the graphics device.
        /// </exception>
	    public ColorWriteChannels ColorWriteChannels
        {
            get => _colorWriteChannels;
            set
            {
                _parent.ThrowIfBound();
                _colorWriteChannels = value;
            }
        }

#if DIRECTX

        internal void GetState(ref SharpDX.Direct3D11.RenderTargetBlendDescription desc)
        {
            // We're blending if we're not in the opaque state.
            desc.IsBlendEnabled =   !(  ColorSourceBlend == Blend.One &&
                                        ColorDestinationBlend == Blend.Zero &&
                                        AlphaSourceBlend == Blend.One &&
                                        AlphaDestinationBlend == Blend.Zero);

            desc.BlendOperation = GetBlendOperation(ColorBlendFunction);
            desc.SourceBlend = GetBlendOption(ColorSourceBlend, false);
            desc.DestinationBlend = GetBlendOption(ColorDestinationBlend, false);

            desc.AlphaBlendOperation = GetBlendOperation(AlphaBlendFunction);
            desc.SourceAlphaBlend = GetBlendOption(AlphaSourceBlend, true);
            desc.DestinationAlphaBlend = GetBlendOption(AlphaDestinationBlend, true);

            desc.RenderTargetWriteMask = GetColorWriteMask(ColorWriteChannels);
        }

        static private SharpDX.Direct3D11.BlendOperation GetBlendOperation(BlendFunction blend)
        {
            return blend switch
            {
                BlendFunction.Add => SharpDX.Direct3D11.BlendOperation.Add,
                BlendFunction.Max => SharpDX.Direct3D11.BlendOperation.Maximum,
                BlendFunction.Min => SharpDX.Direct3D11.BlendOperation.Minimum,
                BlendFunction.ReverseSubtract => SharpDX.Direct3D11.BlendOperation.ReverseSubtract,
                BlendFunction.Subtract => SharpDX.Direct3D11.BlendOperation.Subtract,
                _ => throw new ArgumentException("Invalid blend function!"),
            };
        }

        static private SharpDX.Direct3D11.BlendOption GetBlendOption(Blend blend, bool alpha)
        {
            return blend switch
            {
                Blend.BlendFactor => SharpDX.Direct3D11.BlendOption.BlendFactor,
                Blend.DestinationAlpha => SharpDX.Direct3D11.BlendOption.DestinationAlpha,
                Blend.DestinationColor => alpha ? SharpDX.Direct3D11.BlendOption.DestinationAlpha : SharpDX.Direct3D11.BlendOption.DestinationColor,
                Blend.InverseBlendFactor => SharpDX.Direct3D11.BlendOption.InverseBlendFactor,
                Blend.InverseDestinationAlpha => SharpDX.Direct3D11.BlendOption.InverseDestinationAlpha,
                Blend.InverseDestinationColor => alpha ? SharpDX.Direct3D11.BlendOption.InverseDestinationAlpha : SharpDX.Direct3D11.BlendOption.InverseDestinationColor,
                Blend.InverseSourceAlpha => SharpDX.Direct3D11.BlendOption.InverseSourceAlpha,
                Blend.InverseSourceColor => alpha ? SharpDX.Direct3D11.BlendOption.InverseSourceAlpha : SharpDX.Direct3D11.BlendOption.InverseSourceColor,
                Blend.One => SharpDX.Direct3D11.BlendOption.One,
                Blend.SourceAlpha => SharpDX.Direct3D11.BlendOption.SourceAlpha,
                Blend.SourceAlphaSaturation => SharpDX.Direct3D11.BlendOption.SourceAlphaSaturate,
                Blend.SourceColor => alpha ? SharpDX.Direct3D11.BlendOption.SourceAlpha : SharpDX.Direct3D11.BlendOption.SourceColor,
                Blend.Zero => SharpDX.Direct3D11.BlendOption.Zero,
                _ => throw new ArgumentException("Invalid blend!"),
            };
        }

        static private SharpDX.Direct3D11.ColorWriteMaskFlags GetColorWriteMask(ColorWriteChannels mask)
        {
            return  ((mask & ColorWriteChannels.Red) != 0 ? SharpDX.Direct3D11.ColorWriteMaskFlags.Red : 0) |
                    ((mask & ColorWriteChannels.Green) != 0 ? SharpDX.Direct3D11.ColorWriteMaskFlags.Green : 0) |
                    ((mask & ColorWriteChannels.Blue) != 0 ? SharpDX.Direct3D11.ColorWriteMaskFlags.Blue : 0) |
                    ((mask & ColorWriteChannels.Alpha) != 0 ? SharpDX.Direct3D11.ColorWriteMaskFlags.Alpha : 0);
        }
#endif

    }
}

