// MonoGame - Copyright (C) MonoGame Foundation, Inc
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using Microsoft.Xna.Framework.Graphics;

namespace Microsoft.Xna.Framework.Content;
class SkinnedEffectReader : ContentTypeReader<SkinnedEffect>
{
    protected internal override SkinnedEffect Read(ContentReader input, SkinnedEffect existingInstance)
    {
        var effect = new SkinnedEffect(input.GetGraphicsDevice())
        {
            Texture = input.ReadExternalReference<Texture>() as Texture2D,
            WeightsPerVertex = input.ReadInt32(),
            DiffuseColor = input.ReadVector3(),
            EmissiveColor = input.ReadVector3(),
            SpecularColor = input.ReadVector3(),
            SpecularPower = input.ReadSingle(),
            Alpha = input.ReadSingle()
        };
        return effect;
    }
}

