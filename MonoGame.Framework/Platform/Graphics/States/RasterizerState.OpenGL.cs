// MonoGame - Copyright (C) MonoGame Foundation, Inc
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using System;
using MonoGame.OpenGL;

namespace Microsoft.Xna.Framework.Graphics
{
    public partial class RasterizerState
    {
        internal void PlatformApplyState(GraphicsDevice device, bool force = false)
        {
            // When rendering offscreen the faces change order.
            var offscreen = device.IsRenderTargetBound;

            if (force)
            {
                // Turn off dithering to make sure data returned by Texture.GetData is accurate
                GL.Disable(EnableCap.Dither);
            }

            if (CullMode == CullMode.None)
            {
                GL.Disable(EnableCap.CullFace);
                GraphicsExtensions.CheckGLError();
            }
            else
            {
                GL.Enable(EnableCap.CullFace);
                GraphicsExtensions.CheckGLError();
                GL.CullFace(CullFaceMode.Back);
                GraphicsExtensions.CheckGLError();

                if (CullMode == CullMode.CullClockwiseFace)
                {
                    if (offscreen)
                        GL.FrontFace(FrontFaceDirection.Cw);
                    else
                        GL.FrontFace(FrontFaceDirection.Ccw);
                    GraphicsExtensions.CheckGLError();
                }
                else
                {
                    if (offscreen)
                        GL.FrontFace(FrontFaceDirection.Ccw);
                    else
                        GL.FrontFace(FrontFaceDirection.Cw);
                    GraphicsExtensions.CheckGLError();
                }
            }

#if WINDOWS || DESKTOPGL
			if (FillMode == FillMode.Solid) 
				GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Fill);
            else
				GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Line);
#else
            if (FillMode != FillMode.Solid)
                throw new NotImplementedException();
#endif

            if (force || ScissorTestEnable != device._lastRasterizerState.ScissorTestEnable)
            {
			    if (ScissorTestEnable)
				    GL.Enable(EnableCap.ScissorTest);
			    else
				    GL.Disable(EnableCap.ScissorTest);
                GraphicsExtensions.CheckGLError();
                device._lastRasterizerState.ScissorTestEnable = ScissorTestEnable;
            }

            if (force ||
                DepthBias != device._lastRasterizerState.DepthBias ||
                SlopeScaleDepthBias != device._lastRasterizerState.SlopeScaleDepthBias)
            {
                if (DepthBias != 0 || SlopeScaleDepthBias != 0)
                {
                    // from the docs it seems this works the same as for Direct3D
                    // https://www.khronos.org/opengles/sdk/docs/man/xhtml/glPolygonOffset.xml
                    // explanation for Direct3D is  in https://github.com/MonoGame/MonoGame/issues/4826
                    var depthMul = device.ActiveDepthFormat switch
                    {
                        DepthFormat.None => 0,
                        DepthFormat.Depth16 => 1 << 16 - 1,
                        DepthFormat.Depth24 or DepthFormat.Depth24Stencil8 => 1 << 24 - 1,
                        _ => throw new ArgumentOutOfRangeException(),
                    };
                    GL.Enable(EnableCap.PolygonOffsetFill);
                    GL.PolygonOffset(SlopeScaleDepthBias, DepthBias * depthMul);
                }
                else
                    GL.Disable(EnableCap.PolygonOffsetFill);
                GraphicsExtensions.CheckGLError();
                device._lastRasterizerState.DepthBias = DepthBias;
                device._lastRasterizerState.SlopeScaleDepthBias = SlopeScaleDepthBias;
            }

            if (device.GraphicsCapabilities.SupportsDepthClamp &&
                (force || DepthClipEnable != device._lastRasterizerState.DepthClipEnable))
            {
                if (!DepthClipEnable)
                    GL.Enable((EnableCap) 0x864F); // should be EnableCap.DepthClamp, but not available in OpenTK.Graphics.ES20.EnableCap
                else
                    GL.Disable((EnableCap) 0x864F);
                GraphicsExtensions.CheckGLError();
                device._lastRasterizerState.DepthClipEnable = DepthClipEnable;
            }

            // TODO: Implement MultiSampleAntiAlias
        }
    }
}
