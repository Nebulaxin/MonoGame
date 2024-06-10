// MonoGame - Copyright (C) MonoGame Foundation, Inc
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using Microsoft.Xna.Framework.Graphics;

namespace Microsoft.Xna.Framework.Content
{
	class DualTextureEffectReader : ContentTypeReader<DualTextureEffect>
	{
		protected internal override DualTextureEffect Read(ContentReader input, DualTextureEffect existingInstance)
		{
			DualTextureEffect effect = new(input.GetGraphicsDevice())
			{
				Texture = input.ReadExternalReference<Texture>() as Texture2D,
				Texture2 = input.ReadExternalReference<Texture>() as Texture2D,
				DiffuseColor = input.ReadVector3(),
				Alpha = input.ReadSingle(),
				VertexColorEnabled = input.ReadBoolean()
			};
			return effect;
		}
	}
}

