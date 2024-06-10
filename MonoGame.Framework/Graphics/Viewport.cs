// MonoGame - Copyright (C) MonoGame Foundation, Inc
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using System.Runtime.Serialization;

namespace Microsoft.Xna.Framework.Graphics
{
    /// <summary>
    /// Describes the view bounds for render-target surface.
    /// </summary>
    [DataContract]
    public struct Viewport
    {
        private int x;
        private int y;
        private int width;
        private int height;
        private float minDepth;
        private float maxDepth;

        #region Properties

        /// <summary>
        /// The height of the bounds in pixels.
        /// </summary>
        [DataMember]
        public int Height
        {
            readonly get
            {
                return height;
            }
            set
            {
                height = value;
            }
        }

        /// <summary>
        /// The upper limit of depth of this viewport.
        /// </summary>
        [DataMember]
        public float MaxDepth
        {
            readonly get
            {
                return maxDepth;
            }
            set
            {
                maxDepth = value;
            }
        }

        /// <summary>
        /// The lower limit of depth of this viewport.
        /// </summary>
        [DataMember]
        public float MinDepth
        {
            readonly get
            {
                return minDepth;
            }
            set
            {
                minDepth = value;
            }
        }

        /// <summary>
        /// The width of the bounds in pixels.
        /// </summary>
        [DataMember]
        public int Width
        {
            readonly get
            {
                return width;
            }
            set
            {
                width = value;
            }
        }

        /// <summary>
        /// The y coordinate of the beginning of this viewport.
        /// </summary>
        [DataMember]
        public int Y
        {
            readonly get
            {
                return y;

            }
            set
            {
                y = value;
            }
        }

        /// <summary>
        /// The x coordinate of the beginning of this viewport.
        /// </summary>
        [DataMember]
        public int X
        {
            readonly get { return x; }
            set { x = value; }
        }

        #endregion

        /// <summary>
        /// Gets the aspect ratio of this <see cref="Viewport"/>, which is width / height. 
        /// </summary>
        public readonly float AspectRatio
        {
            get
            {
                if ((height != 0) && (width != 0))
                {
                    return ((float)width) / ((float)height);
                }
                return 0f;
            }
        }

        /// <summary>
        /// Gets or sets a boundary of this <see cref="Viewport"/>.
        /// </summary>
        public Rectangle Bounds
        {
            readonly get
            {
                return new Rectangle(x, y, width, height);
            }

            set
            {
                x = value.X;
                y = value.Y;
                width = value.Width;
                height = value.Height;
            }
        }

        /// <summary>
        /// Returns the subset of the viewport that is guaranteed to be visible on a lower quality display.
        /// </summary>
		public readonly Rectangle TitleSafeArea
        {
            get { return GraphicsDevice.GetTitleSafeArea(x, y, width, height); }
        }

        /// <summary>
        /// Constructs a viewport from the given values. The <see cref="MinDepth"/> will be 0.0 and <see cref="MaxDepth"/> will be 1.0.
        /// </summary>
        /// <param name="x">The x coordinate of the upper-left corner of the view bounds in pixels.</param>
        /// <param name="y">The y coordinate of the upper-left corner of the view bounds in pixels.</param>
        /// <param name="width">The width of the view bounds in pixels.</param>
        /// <param name="height">The height of the view bounds in pixels.</param>
        public Viewport(int x, int y, int width, int height)
        {
            this.x = x;
            this.y = y;
            this.width = width;
            this.height = height;
            minDepth = 0.0f;
            maxDepth = 1.0f;
        }

        /// <summary>
        /// Constructs a viewport from the given values.
        /// </summary>
        /// <param name="x">The x coordinate of the upper-left corner of the view bounds in pixels.</param>
        /// <param name="y">The y coordinate of the upper-left corner of the view bounds in pixels.</param>
        /// <param name="width">The width of the view bounds in pixels.</param>
        /// <param name="height">The height of the view bounds in pixels.</param>
        /// <param name="minDepth">The lower limit of depth.</param>
        /// <param name="maxDepth">The upper limit of depth.</param>
        public Viewport(int x, int y, int width, int height, float minDepth, float maxDepth)
        {
            this.x = x;
            this.y = y;
            this.width = width;
            this.height = height;
            this.minDepth = minDepth;
            this.maxDepth = maxDepth;
        }

        /// <summary>
        /// Creates a new instance of <see cref="Viewport"/> struct.
        /// </summary>
        /// <param name="bounds">A <see cref="Rectangle"/> that defines the location and size of the <see cref="Viewport"/> in a render target.</param>
		public Viewport(Rectangle bounds) : this(bounds.X, bounds.Y, bounds.Width, bounds.Height)
        {
        }

        /// <summary>
        /// Projects a <see cref="Vector3"/> from model space into screen space.
        /// The source point is transformed from model space to world space by the world matrix,
        /// then from world space to view space by the view matrix, and
        /// finally from view space to screen space by the projection matrix.
        /// </summary>
        /// <param name="source">The <see cref="Vector3"/> to project.</param>
        /// <param name="projection">The projection <see cref="Matrix"/>.</param>
        /// <param name="view">The view <see cref="Matrix"/>.</param>
        /// <param name="world">The world <see cref="Matrix"/>.</param>
        /// <returns></returns>
        public readonly Vector3 Project(Vector3 source, Matrix projection, Matrix view, Matrix world)
        {
            Matrix matrix = Matrix.Multiply(Matrix.Multiply(world, view), projection);
            Vector3 vector = Vector3.Transform(source, matrix);
            float a = (source.X * matrix.M14) + (source.Y * matrix.M24) + (source.Z * matrix.M34) + matrix.M44;
            if (!WithinEpsilon(a, 1f))
            {
                vector.X = vector.X / a;
                vector.Y = vector.Y / a;
                vector.Z = vector.Z / a;
            }
            vector.X = ((vector.X + 1f) * 0.5f * width) + x;
            vector.Y = ((-vector.Y + 1f) * 0.5f * height) + y;
            vector.Z = (vector.Z * (maxDepth - minDepth)) + minDepth;
            return vector;
        }

        /// <summary>
        /// Unprojects a <see cref="Vector3"/> from screen space into model space.
        /// The source point is transformed from screen space to view space by the inverse of the projection matrix,
        /// then from view space to world space by the inverse of the view matrix, and
        /// finally from world space to model space by the inverse of the world matrix.
        /// Note source.Z must be less than or equal to MaxDepth.
        /// </summary>
        /// <param name="source">The <see cref="Vector3"/> to unproject.</param>
        /// <param name="projection">The projection <see cref="Matrix"/>.</param>
        /// <param name="view">The view <see cref="Matrix"/>.</param>
        /// <param name="world">The world <see cref="Matrix"/>.</param>
        /// <returns></returns>
        public readonly Vector3 Unproject(Vector3 source, Matrix projection, Matrix view, Matrix world)
        {
            Matrix matrix = Matrix.Invert(Matrix.Multiply(Matrix.Multiply(world, view), projection));
            source.X = ((source.X - x) / ((float)width) * 2f) - 1f;
            source.Y = -(((source.Y - y) / ((float)height) * 2f) - 1f);
            source.Z = (source.Z - minDepth) / (maxDepth - minDepth);
            Vector3 vector = Vector3.Transform(source, matrix);
            float a = (source.X * matrix.M14) + (source.Y * matrix.M24) + (source.Z * matrix.M34) + matrix.M44;
            if (!WithinEpsilon(a, 1f))
            {
                vector.X = vector.X / a;
                vector.Y = vector.Y / a;
                vector.Z = vector.Z / a;
            }
            return vector;

        }

        private static bool WithinEpsilon(float a, float b)
        {
            float num = a - b;
            return (-1.401298E-45f <= num) && (num <= float.Epsilon);
        }

        /// <summary>
        /// Returns a <see cref="string"/> representation of this <see cref="Viewport"/> in the format:
        /// {X:[<see cref="X"/>] Y:[<see cref="Y"/>] Width:[<see cref="Width"/>] Height:[<see cref="Height"/>] MinDepth:[<see cref="MinDepth"/>] MaxDepth:[<see cref="MaxDepth"/>]}
        /// </summary>
        /// <returns>A <see cref="string"/> representation of this <see cref="Viewport"/>.</returns>
        public override readonly string ToString()
        {
            return "{X:" + x + " Y:" + y + " Width:" + width + " Height:" + height + " MinDepth:" + minDepth + " MaxDepth:" + maxDepth + "}";
        }
    }
}

