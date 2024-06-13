// MonoGame - Copyright (C) MonoGame Foundation, Inc
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using System.Collections.Generic;
using Microsoft.Xna.Framework.Content.Pipeline.Graphics;

namespace Microsoft.Xna.Framework.Content.Pipeline.Processors
{
    public sealed class ModelMeshContent
    {
        private BoundingSphere _boundingSphere;

        internal ModelMeshContent() { }

        internal ModelMeshContent(string name, MeshContent sourceMesh, ModelBoneContent parentBone,
                                  BoundingSphere boundingSphere, IList<ModelMeshPartContent> meshParts)
        {
            Name = name;
            SourceMesh = sourceMesh;
            ParentBone = parentBone;
            _boundingSphere = boundingSphere;
            MeshParts = new ModelMeshPartContentCollection(meshParts);
        }

        public BoundingSphere BoundingSphere => _boundingSphere;

        public ModelMeshPartContentCollection MeshParts { get; }

        public string Name { get; }

        public ModelBoneContent ParentBone { get; }

        public MeshContent SourceMesh { get; }

        public object Tag { get; set; }
    }
}
