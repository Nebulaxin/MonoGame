// MonoGame - Copyright (C) MonoGame Foundation, Inc
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.


using System;

namespace Microsoft.Xna.Framework.Graphics.PackedVector
{
    /// <summary>
    /// Packed vector type containing two 8-bit signed normalized values, ranging from −1 to 1.
    /// </summary>
    public struct NormalizedByte2 : IPackedVector<ushort>, IEquatable<NormalizedByte2>
    {
        /// <summary>
        /// Initializes a new instance of this structure.
        /// </summary>
        /// <param name="vector">
        /// A <see cref="Vector2"/> value who's components contain the initial values for this structure.
        /// </param>
        public NormalizedByte2(Vector2 vector)
        {
            PackedValue = Pack(vector.X, vector.Y);
        }

        /// <summary>
        /// Initializes a new instance of this structure.
        /// </summary>
        /// <param name="x">The initial x-component value for this structure.</param>
        /// <param name="y">The initial y-component value for this structure.</param>
        public NormalizedByte2(float x, float y)
        {
            PackedValue = Pack(x, y);
        }

        /// <summary>
        /// Returns a value that indicates whether the two value are not equal.
        /// </summary>
        /// <param name="a">The value on the left of the inequality operator.</param>
        /// <param name="b">The value on the right of the inequality operator.</param>
        /// <returns>true if the two value are not equal; otherwise, false.</returns>
        public static bool operator !=(NormalizedByte2 a, NormalizedByte2 b)
        {
            return a.PackedValue != b.PackedValue;
        }

        /// <summary>
        /// Returns a value that indicates whether the two values are equal.
        /// </summary>
        /// <param name="a">The value on the left of the equality operator.</param>
        /// <param name="b">The value on the right of the equality operator.</param>
        /// <returns>true if the two values are equal; otherwise, false.</returns>
        public static bool operator ==(NormalizedByte2 a, NormalizedByte2 b)
        {
            return a.PackedValue == b.PackedValue;
        }

        /// <inheritdoc />
        public ushort PackedValue { get; set; }

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            return (obj is NormalizedByte2) &&
                    ((NormalizedByte2)obj).PackedValue == PackedValue;
        }

        /// <inheritdoc />
        public bool Equals(NormalizedByte2 other)
        {
            return PackedValue == other.PackedValue;
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            return PackedValue.GetHashCode();
        }

        /// <inheritdoc />
        public override string ToString()
        {
            return PackedValue.ToString("X");
        }

        private static ushort Pack(float x, float y)
        {
            var byte2 = (((ushort) MathF.Round(MathHelper.Clamp(x, -1.0f, 1.0f) * 127.0f)) & 0xFF) << 0;
            var byte1 = (((ushort) MathF.Round(MathHelper.Clamp(y, -1.0f, 1.0f) * 127.0f)) & 0xFF) << 8;

            return (ushort)(byte2 | byte1);
        }

        void IPackedVector.PackFromVector4(Vector4 vector)
        {
            PackedValue = Pack(vector.X, vector.Y);
        }

        /// <inheritdoc />
        public Vector4 ToVector4()
        {
            return new Vector4(ToVector2(), 0.0f, 1.0f);
        }

        /// <summary>
        /// Expands the packed representation to a <see cref="Vector2"/>.
        /// </summary>
        /// <returns>The expanded value.</returns>
        public Vector2 ToVector2()
        {
            return new Vector2(
                ((sbyte)((PackedValue >> 0) & 0xFF)) / 127.0f,
                ((sbyte)((PackedValue >> 8) & 0xFF)) / 127.0f);
        }
    }
}
