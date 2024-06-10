// MonoGame - Copyright (C) MonoGame Foundation, Inc
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using System;

namespace Microsoft.Xna.Framework.Graphics.PackedVector;
/// <summary>
/// Packed vector type containing a single 16-bit floating point
/// </summary>
public struct HalfSingle : IPackedVector<ushort>, IEquatable<HalfSingle>, IPackedVector
{
    ushort packedValue;

    /// <summary>
    /// Initializes a new instance of this structure.
    /// </summary>
    /// <param name="single">TheThe initial value for this structure.</param>
    public HalfSingle(float single)
    {
        packedValue = HalfTypeHelper.Convert(single);
    }

    /// <inheritdoc />
    public ushort PackedValue
    {
        readonly get
        {
            return packedValue;
        }
        set
        {
            packedValue = value;
        }
    }

    /// <summary>
    /// Expands the packed representation to a <see cref="float">System.Single</see>
    /// </summary>
    /// <returns>The expanded value.</returns>
    public readonly float ToSingle()
    {
        return HalfTypeHelper.Convert(packedValue);
    }

    /// <inheritdoc />
    void IPackedVector.PackFromVector4(Vector4 vector)
    {
        packedValue = HalfTypeHelper.Convert(vector.X);
    }

    /// <inheritdoc />
    public readonly Vector4 ToVector4()
    {
        return new Vector4(ToSingle(), 0f, 0f, 1f);
    }

    /// <inheritdoc />
    public override readonly bool Equals(object obj)
    {
        if (obj != null && obj.GetType() == GetType())
        {
            return this == (HalfSingle)obj;
        }

        return false;
    }

    /// <inheritdoc />
    public readonly bool Equals(HalfSingle other)
    {
        return packedValue == other.packedValue;
    }

    /// <inheritdoc />
    public override readonly string ToString()
    {
        return ToSingle().ToString();
    }

    /// <inheritdoc />
    public override readonly int GetHashCode()
    {
        return packedValue.GetHashCode();
    }

    /// <summary>
    /// Returns a value that indicates whether the two values are equal.
    /// </summary>
    /// <param name="lhs">The value on the left of the equality operator.</param>
    /// <param name="rhs">The value on the right of the equality operator.</param>
    /// <returns>true if the two values are equal; otherwise, false.</returns>
    public static bool operator ==(HalfSingle lhs, HalfSingle rhs)
    {
        return lhs.packedValue == rhs.packedValue;
    }

    /// <summary>
    /// Returns a value that indicates whether the two value are not equal.
    /// </summary>
    /// <param name="lhs">The value on the left of the inequality operator.</param>
    /// <param name="rhs">The value on the right of the inequality operator.</param>
    /// <returns>true if the two value are not equal; otherwise, false.</returns>
    public static bool operator !=(HalfSingle lhs, HalfSingle rhs)
    {
        return lhs.packedValue != rhs.packedValue;
    }
}

