// MonoGame - Copyright (C) MonoGame Foundation, Inc
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

namespace Microsoft.Xna.Framework.Content.Pipeline.Processors
{
    public sealed class ModelBoneContent
    {
        internal ModelBoneContent() { }

        internal ModelBoneContent(string name, int index, Matrix transform, ModelBoneContent parent)
        {
            Name = name;
            Index = index;
            Transform = transform;
            Parent = parent;
        }

        public ModelBoneContentCollection Children { get; internal set; }

        public int Index { get; }

        public string Name { get; }

        public ModelBoneContent Parent { get; }

        public Matrix Transform { get; set; }
    }
}
