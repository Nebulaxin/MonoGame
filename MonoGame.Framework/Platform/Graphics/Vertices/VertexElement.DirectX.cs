// MonoGame - Copyright (C) MonoGame Foundation, Inc
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using System;

namespace Microsoft.Xna.Framework.Graphics
{
    partial struct VertexElement
    {
        /// <summary>
        /// Gets the DirectX <see cref="SharpDX.Direct3D11.InputElement"/>.
        /// </summary>
        /// <param name="slot">The input resource slot.</param>
        /// <param name="instanceFrequency">
        /// The number of instances to draw using the same per-instance data before advancing in the
        /// buffer by one element. This value must be 0 for an element that contains per-vertex
        /// data.
        /// </param>
        /// <returns><see cref="SharpDX.Direct3D11.InputElement"/>.</returns>
        /// <exception cref="NotSupportedException">
        /// Unknown vertex element format or usage!
        /// </exception>
        internal SharpDX.Direct3D11.InputElement GetInputElement(int slot, int instanceFrequency)
        {
            var element = new SharpDX.Direct3D11.InputElement();

            element.SemanticName = VertexElementUsage switch
            {
                VertexElementUsage.Position => "POSITION",
                VertexElementUsage.Color => "COLOR",
                VertexElementUsage.Normal => "NORMAL",
                VertexElementUsage.TextureCoordinate => "TEXCOORD",
                VertexElementUsage.BlendIndices => "BLENDINDICES",
                VertexElementUsage.BlendWeight => "BLENDWEIGHT",
                VertexElementUsage.Binormal => "BINORMAL",
                VertexElementUsage.Tangent => "TANGENT",
                VertexElementUsage.PointSize => "PSIZE",
                _ => throw new NotSupportedException("Unknown vertex element usage!"),
            };
            element.SemanticIndex = UsageIndex;

            element.Format = VertexElementFormat switch
            {
                VertexElementFormat.Single => SharpDX.DXGI.Format.R32_Float,
                VertexElementFormat.Vector2 => SharpDX.DXGI.Format.R32G32_Float,
                VertexElementFormat.Vector3 => SharpDX.DXGI.Format.R32G32B32_Float,
                VertexElementFormat.Vector4 => SharpDX.DXGI.Format.R32G32B32A32_Float,
                VertexElementFormat.Color => SharpDX.DXGI.Format.R8G8B8A8_UNorm,
                VertexElementFormat.Byte4 => SharpDX.DXGI.Format.R8G8B8A8_UInt,
                VertexElementFormat.Short2 => SharpDX.DXGI.Format.R16G16_SInt,
                VertexElementFormat.Short4 => SharpDX.DXGI.Format.R16G16B16A16_SInt,
                VertexElementFormat.NormalizedShort2 => SharpDX.DXGI.Format.R16G16_SNorm,
                VertexElementFormat.NormalizedShort4 => SharpDX.DXGI.Format.R16G16B16A16_SNorm,
                VertexElementFormat.HalfVector2 => SharpDX.DXGI.Format.R16G16_Float,
                VertexElementFormat.HalfVector4 => SharpDX.DXGI.Format.R16G16B16A16_Float,
                _ => throw new NotSupportedException("Unknown vertex element format!"),
            };
            element.Slot = slot;
            element.AlignedByteOffset = Offset;
            
            // Note that instancing is only supported in feature level 9.3 and above.
            element.Classification = (instanceFrequency == 0) 
                                     ? SharpDX.Direct3D11.InputClassification.PerVertexData
                                     : SharpDX.Direct3D11.InputClassification.PerInstanceData;
            element.InstanceDataStepRate = instanceFrequency;

            return element;
        }
    }
}
