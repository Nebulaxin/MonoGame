﻿// MonoGame - Copyright (C) MonoGame Foundation, Inc
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using Microsoft.Xna.Framework.Graphics;

namespace Microsoft.Xna.Framework.Content;

class AlphaTestEffectReader : ContentTypeReader<AlphaTestEffect>
{
    protected internal override AlphaTestEffect Read(ContentReader input, AlphaTestEffect existingInstance)
    {
        var effect = new AlphaTestEffect(input.GetGraphicsDevice())
        {
            Texture = input.ReadExternalReference<Texture>() as Texture2D,
            AlphaFunction = (CompareFunction)input.ReadInt32(),
            ReferenceAlpha = (int)input.ReadUInt32(),
            DiffuseColor = input.ReadVector3(),
            Alpha = input.ReadSingle(),
            VertexColorEnabled = input.ReadBoolean()
        };
        return effect;
    }
}
