﻿// MonoGame - Copyright (C) MonoGame Foundation, Inc
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using System.Collections.Generic;

namespace Microsoft.Xna.Framework.Content.Pipeline.Processors
{
    public sealed class ModelContent
    {
        internal ModelContent() { }

        internal ModelContent(ModelBoneContent root, IList<ModelBoneContent> bones, IList<ModelMeshContent> meshes)
        {
            Root = root;
            Bones = new ModelBoneContentCollection(bones);
            Meshes = new ModelMeshContentCollection(meshes);
        }

        public ModelBoneContentCollection Bones { get; }

        public ModelMeshContentCollection Meshes { get; }

        public ModelBoneContent Root { get; }

        public object Tag { get; set; }
    }
}
