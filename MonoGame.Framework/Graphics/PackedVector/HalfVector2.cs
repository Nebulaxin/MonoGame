// MonoGame - Copyright (C) MonoGame Foundation, Inc
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using System;

namespace Microsoft.Xna.Framework.Graphics.PackedVector;
/// <summary>
/// Packed vector type containing two 16-bit floating-point values.
/// </summary>
public struct HalfVector2 : IPackedVector<uint>, IPackedVector, IEquatable<HalfVector2>
{
    private uint packedValue;

    /// <summary>
    /// Initializes a new instance of this structure.
    /// </summary>
    /// <param name="x">The initial x-component value for this structure.</param>
    /// <param name="y">The initial y-component value for this structure.</param>
    public HalfVector2(float x, float y)
    {
        packedValue = PackHelper(x, y);
    }

    /// <summary>
    /// Initializes a new instance of this structure.
    /// </summary>
    /// <param name="vector">
    /// A <see cref="Vector2"/> value who's components contain the initial values for this structure.
    /// </param>
    public HalfVector2(Vector2 vector)
    {
        packedValue = PackHelper(vector.X, vector.Y);
    }

    /// <inheritdoc />
    void IPackedVector.PackFromVector4(Vector4 vector)
    {
        packedValue = PackHelper(vector.X, vector.Y);
    }

    private static uint PackHelper(float vectorX, float vectorY)
    {
        uint num2 = HalfTypeHelper.Convert(vectorX);
        uint num = (uint)(HalfTypeHelper.Convert(vectorY) << 0x10);
        return num2 | num;
    }

    /// <summary>
    /// Expands the packed representation to a <see cref="Vector2"/>.
    /// </summary>
    /// <returns>The expanded value.</returns>
    public readonly Vector2 ToVector2()
    {
        Vector2 vector;
        vector.X = HalfTypeHelper.Convert((ushort)packedValue);
        vector.Y = HalfTypeHelper.Convert((ushort)(packedValue >> 0x10));
        return vector;
    }

    /// <inheritdoc />
    public readonly Vector4 ToVector4()
    {
        Vector2 vector = ToVector2();
        return new Vector4(vector.X, vector.Y, 0f, 1f);
    }

    /// <inheritdoc />
    public uint PackedValue
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

    /// <inheritdoc />
    public override readonly string ToString()
    {
        return ToVector2().ToString();
    }

    /// <inheritdoc />
    public override readonly int GetHashCode()
    {
        return packedValue.GetHashCode();
    }

    /// <inheritdoc />
    public override readonly bool Equals(object obj)
    {
        return (obj is HalfVector2) && Equals((HalfVector2)obj);
    }

    /// <inheritdoc />
    public readonly bool Equals(HalfVector2 other)
    {
        return packedValue.Equals(other.packedValue);
    }

    /// <summary>
    /// Returns a value that indicates whether the two values are equal.
    /// </summary>
    /// <param name="a">The value on the left of the equality operator.</param>
    /// <param name="b">The value on the right of the equality operator.</param>
    /// <returns>true if the two values are equal; otherwise, false.</returns>
    public static bool operator ==(HalfVector2 a, HalfVector2 b)
    {
        return a.Equals(b);
    }

    /// <summary>
    /// Returns a value that indicates whether the two value are not equal.
    /// </summary>
    /// <param name="a">The value on the left of the inequality operator.</param>
    /// <param name="b">The value on the right of the inequality operator.</param>
    /// <returns>true if the two value are not equal; otherwise, false.</returns>
    public static bool operator !=(HalfVector2 a, HalfVector2 b)
    {
        return !a.Equals(b);
    }
}

