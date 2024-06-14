// MonoGame - Copyright (C) MonoGame Foundation, Inc
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using System;
using System.Diagnostics;

namespace Microsoft.Xna.Framework.Graphics
{
    public partial class SamplerState
    {
        private SharpDX.Direct3D11.SamplerState _state;

        /// <inheritdoc />
        protected internal override void GraphicsDeviceResetting()
        {
            SharpDX.Utilities.Dispose(ref _state);
            base.GraphicsDeviceResetting();
        }

        internal SharpDX.Direct3D11.SamplerState GetState(GraphicsDevice device)
        {
            if (_state == null)
            {
                // Build the description.
                var desc = new SharpDX.Direct3D11.SamplerStateDescription();

                desc.AddressU = GetAddressMode(AddressU);
                desc.AddressV = GetAddressMode(AddressV);
                desc.AddressW = GetAddressMode(AddressW);

#if WINDOWS_UAP
				desc.BorderColor = new SharpDX.Mathematics.Interop.RawColor4(
					BorderColor.R / 255.0f,
					BorderColor.G / 255.0f,
					BorderColor.B / 255.0f,
					BorderColor.A / 255.0f);
#else
				desc.BorderColor = BorderColor.ToColor4();
#endif
				desc.Filter = GetFilter(Filter, FilterMode);
                desc.MaximumAnisotropy = Math.Min(MaxAnisotropy, device.GraphicsCapabilities.MaxTextureAnisotropy);
                desc.MipLodBias = MipMapLevelOfDetailBias;
                desc.ComparisonFunction = ComparisonFunction.ToComparison();

                // TODO: How do i do this?
                desc.MinimumLod = 0.0f;

                // To support feature level 9.1 these must
                // be set to these exact values.
                desc.MaximumLod = float.MaxValue;

                // Create the state.
                _state = new SharpDX.Direct3D11.SamplerState(GraphicsDevice._d3dDevice, desc);
            }

            Debug.Assert(GraphicsDevice == device, "The state was created for a different device!");

            return _state;
        }

        private static SharpDX.Direct3D11.Filter GetFilter(TextureFilter filter, TextureFilterMode mode)
        {
            return mode switch
            {
                TextureFilterMode.Comparison => filter switch
                {
                    TextureFilter.Anisotropic => SharpDX.Direct3D11.Filter.ComparisonAnisotropic,
                    TextureFilter.Linear => SharpDX.Direct3D11.Filter.ComparisonMinMagMipLinear,
                    TextureFilter.LinearMipPoint => SharpDX.Direct3D11.Filter.ComparisonMinMagLinearMipPoint,
                    TextureFilter.MinLinearMagPointMipLinear => SharpDX.Direct3D11.Filter.ComparisonMinLinearMagPointMipLinear,
                    TextureFilter.MinLinearMagPointMipPoint => SharpDX.Direct3D11.Filter.ComparisonMinLinearMagMipPoint,
                    TextureFilter.MinPointMagLinearMipLinear => SharpDX.Direct3D11.Filter.ComparisonMinPointMagMipLinear,
                    TextureFilter.MinPointMagLinearMipPoint => SharpDX.Direct3D11.Filter.ComparisonMinPointMagLinearMipPoint,
                    TextureFilter.Point => SharpDX.Direct3D11.Filter.ComparisonMinMagMipPoint,
                    TextureFilter.PointMipLinear => SharpDX.Direct3D11.Filter.ComparisonMinMagPointMipLinear,
                    _ => throw new ArgumentException("Invalid texture filter!"),
                },
                TextureFilterMode.Default => filter switch
                {
                    TextureFilter.Anisotropic => SharpDX.Direct3D11.Filter.Anisotropic,
                    TextureFilter.Linear => SharpDX.Direct3D11.Filter.MinMagMipLinear,
                    TextureFilter.LinearMipPoint => SharpDX.Direct3D11.Filter.MinMagLinearMipPoint,
                    TextureFilter.MinLinearMagPointMipLinear => SharpDX.Direct3D11.Filter.MinLinearMagPointMipLinear,
                    TextureFilter.MinLinearMagPointMipPoint => SharpDX.Direct3D11.Filter.MinLinearMagMipPoint,
                    TextureFilter.MinPointMagLinearMipLinear => SharpDX.Direct3D11.Filter.MinPointMagMipLinear,
                    TextureFilter.MinPointMagLinearMipPoint => SharpDX.Direct3D11.Filter.MinPointMagLinearMipPoint,
                    TextureFilter.Point => SharpDX.Direct3D11.Filter.MinMagMipPoint,
                    TextureFilter.PointMipLinear => SharpDX.Direct3D11.Filter.MinMagPointMipLinear,
                    _ => throw new ArgumentException("Invalid texture filter!"),
                },
                _ => throw new ArgumentException("Invalid texture filter mode!"),
            };
        }

        private static SharpDX.Direct3D11.TextureAddressMode GetAddressMode(TextureAddressMode mode)
        {
            return mode switch
            {
                TextureAddressMode.Clamp => SharpDX.Direct3D11.TextureAddressMode.Clamp,
                TextureAddressMode.Mirror => SharpDX.Direct3D11.TextureAddressMode.Mirror,
                TextureAddressMode.Wrap => SharpDX.Direct3D11.TextureAddressMode.Wrap,
                TextureAddressMode.Border => SharpDX.Direct3D11.TextureAddressMode.Border,
                _ => throw new ArgumentException("Invalid texture address mode!"),
            };
        }

        partial void PlatformDispose()
        {
            SharpDX.Utilities.Dispose(ref _state);
        }
    }
}

