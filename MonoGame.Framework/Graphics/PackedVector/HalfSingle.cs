// MonoGame - Copyright (C) MonoGame Foundation, Inc
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using System;

namespace Microsoft.Xna.Framework.Graphics.PackedVector
{
    /// <summary>
    /// Packed vector type containing a single 16-bit floating point
    /// </summary>
    public struct HalfSingle : IPackedVector<UInt16>, IEquatable<HalfSingle>, IPackedVector
    {
        /// <summary>
        /// Initializes a new instance of this structure.
        /// </summary>
        /// <param name="single">TheThe initial value for this structure.</param>
        public HalfSingle(float single)
        {
            PackedValue = HalfTypeHelper.Convert(single);
        }

        /// <inheritdoc />
        public ushort PackedValue { get; set; }

        /// <summary>
        /// Expands the packed representation to a <see cref="Single">System.Single</see>
        /// </summary>
        /// <returns>The expanded value.</returns>
        public float ToSingle()
        {
            return HalfTypeHelper.Convert(PackedValue);
        }

        /// <inheritdoc />
        void IPackedVector.PackFromVector4(Vector4 vector)
        {
            PackedValue = HalfTypeHelper.Convert(vector.X);
        }

        /// <inheritdoc />
        public Vector4 ToVector4()
        {
            return new Vector4(ToSingle(), 0f, 0f, 1f);
        }

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            if (obj != null && obj.GetType() == GetType())
            {
                return this == (HalfSingle)obj;
            }

            return false;
        }

        /// <inheritdoc />
        public bool Equals(HalfSingle other)
        {
            return PackedValue == other.PackedValue;
        }

        /// <inheritdoc />
        public override string ToString()
        {
            return ToSingle().ToString();
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
        public static bool operator ==(HalfSingle lhs, HalfSingle rhs)
        {
            return lhs.PackedValue == rhs.PackedValue;
        }

        /// <summary>
        /// Returns a value that indicates whether the two value are not equal.
        /// </summary>
        /// <param name="lhs">The value on the left of the inequality operator.</param>
        /// <param name="rhs">The value on the right of the inequality operator.</param>
        /// <returns>true if the two value are not equal; otherwise, false.</returns>
        public static bool operator !=(HalfSingle lhs, HalfSingle rhs)
        {
            return lhs.PackedValue != rhs.PackedValue;
        }
    }
}
