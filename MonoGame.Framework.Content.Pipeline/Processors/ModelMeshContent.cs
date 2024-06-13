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
        private ModelMeshPartContentCollection _meshParts;
        private string _name;
        private ModelBoneContent _parentBone;
        private MeshContent _sourceMesh;

        internal ModelMeshContent() { }

        internal ModelMeshContent(string name, MeshContent sourceMesh, ModelBoneContent parentBone,
                                  BoundingSphere boundingSphere, IList<ModelMeshPartContent> meshParts)
        {
            _name = name;
            _sourceMesh = sourceMesh;
            _parentBone = parentBone;
            _boundingSphere = boundingSphere;
            _meshParts = new ModelMeshPartContentCollection(meshParts);
        }

        public BoundingSphere BoundingSphere => _boundingSphere;

        public ModelMeshPartContentCollection MeshParts => _meshParts;

        public string Name => _name;

        public ModelBoneContent ParentBone => _parentBone;

        public MeshContent SourceMesh => _sourceMesh;

        public object Tag { get; set; }
    }
}
