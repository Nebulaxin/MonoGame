// MonoGame - Copyright (C) MonoGame Foundation, Inc
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using System;

namespace Microsoft.Xna.Framework.Graphics.PackedVector
{
    /// <summary>
    /// Packed vector type containing a single 8 bit normalized W values that is ranging from 0 to 1.
    /// </summary>
    public struct Alpha8 : IPackedVector<byte>, IEquatable<Alpha8>, IPackedVector
    {
        /// <inheritdoc />
        public byte PackedValue { get; set; }

        /// <summary>
        /// Initializes a new instance of this structure.
        /// </summary>
        /// <param name="alpha">The initial value for this structure.</param>
        public Alpha8(float alpha)
        {
            PackedValue = Pack(alpha);
        }

        /// <summary>
        /// Expands the packed representation to a <see cref="Single">System.Single</see>
        /// </summary>
        /// <returns>The expanded value.</returns>
        public float ToAlpha()
        {
            return (float)(PackedValue / 255.0f);
        }

        /// <inheritdoc />
        void IPackedVector.PackFromVector4(Vector4 vector)
        {
            PackedValue = Pack(vector.W);
        }

        /// <inheritdoc />
        public Vector4 ToVector4()
        {
            return new Vector4(
                0.0f,
                0.0f,
                0.0f,
                (float)(PackedValue / 255.0f)
            );
        }

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            return (obj is Alpha8) && Equals((Alpha8) obj);
        }

        /// <inheritdoc />
        public bool Equals(Alpha8 other)
        {
            return PackedValue == other.PackedValue;
        }

        /// <inheritdoc />
        public override string ToString()
        {
            return (PackedValue / 255.0f).ToString();
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            return PackedValue.GetHashCode();
        }

        /// <summary>
        /// Returns a value that indicates whether the two values are equal.
        /// </summary>
        /// <param name="lhs">The value on the left of the equality operator.</param>
        /// <param name="rhs">The value on the right of the equality operator.</param>
        /// <returns>true if the two values are equal; otherwise, false.</returns>
        public static bool operator ==(Alpha8 lhs, Alpha8 rhs)
        {
            return lhs.PackedValue == rhs.PackedValue;
        }

        /// <summary>
        /// Returns a value that indicates whether the two value are not equal.
        /// </summary>
        /// <param name="lhs">The value on the left of the inequality operator.</param>
        /// <param name="rhs">The value on the right of the inequality operator.</param>
        /// <returns>true if the two value are not equal; otherwise, false.</returns>
        public static bool operator !=(Alpha8 lhs, Alpha8 rhs)
        {
            return lhs.PackedValue != rhs.PackedValue;
        }

        private static byte Pack(float alpha)
        {
            return (byte) MathF.Round(
                MathHelper.Clamp(alpha, 0, 1) * 255.0f
            );
        }
    }
}
