// MIT License - Copyright (C) The Mono.Xna Team
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace Microsoft.Xna.Framework
{
    /// <summary>
    /// Represents an axis-aligned bounding box (AABB) in 3D space.
    /// </summary>
    [DataContract]
    [DebuggerDisplay("{DebugDisplayString,nq}")]
    public struct BoundingBox : IEquatable<BoundingBox>
    {

        #region Public Fields

        /// <summary>
        ///   The minimum extent of this <see cref="BoundingBox"/>.
        /// </summary>
        [DataMember]
        public Vector3 Min;
      
        /// <summary>
        ///   The maximum extent of this <see cref="BoundingBox"/>.
        /// </summary>
        [DataMember]
        public Vector3 Max;

        /// <summary>
        ///   The number of corners in a <see cref="BoundingBox"/>. This is equal to 8.
        /// </summary>
        public const int CornerCount = 8;

        #endregion Public Fields


        #region Public Constructors

        /// <summary>
        ///   Create a <see cref="BoundingBox"/>.
        /// </summary>
        /// <param name="min">The minimum extent of the <see cref="BoundingBox"/>.</param>
        /// <param name="max">The maximum extent of the <see cref="BoundingBox"/>.</param>
        public BoundingBox(Vector3 min, Vector3 max)
        {
            Min = min;
            Max = max;
        }

        #endregion Public Constructors


        #region Public Methods

        /// <summary>
        ///   Check if this <see cref="BoundingBox"/> contains another <see cref="BoundingBox"/>.
        /// </summary>
        /// <param name="box">The <see cref="BoundingBox"/> to test for overlap.</param>
        /// <returns>
        ///   A value indicating if this <see cref="BoundingBox"/> contains,
        ///   intersects with or is disjoint with <paramref name="box"/>.
        /// </returns>
        public ContainmentType Contains(BoundingBox box)
        {
            //test if all corner is in the same side of a face by just checking min and max
            if (box.Max.X < Min.X
                || box.Min.X > Max.X
                || box.Max.Y < Min.Y
                || box.Min.Y > Max.Y
                || box.Max.Z < Min.Z
                || box.Min.Z > Max.Z)
                return ContainmentType.Disjoint;


            if (box.Min.X >= Min.X
                && box.Max.X <= Max.X
                && box.Min.Y >= Min.Y
                && box.Max.Y <= Max.Y
                && box.Min.Z >= Min.Z
                && box.Max.Z <= Max.Z)
                return ContainmentType.Contains;

            return ContainmentType.Intersects;
        }

        /// <summary>
        ///   Check if this <see cref="BoundingBox"/> contains another <see cref="BoundingBox"/>.
        /// </summary>
        /// <param name="box">The <see cref="BoundingBox"/> to test for overlap.</param>
        /// <param name="result">
        ///   A value indicating if this <see cref="BoundingBox"/> contains,
        ///   intersects with or is disjoint with <paramref name="box"/>.
        /// </param>
        public void Contains(ref BoundingBox box, out ContainmentType result)
        {
            result = Contains(box);
        }

        /// <summary>
        ///   Check if this <see cref="BoundingBox"/> contains a <see cref="BoundingFrustum"/>.
        /// </summary>
        /// <param name="frustum">The <see cref="BoundingFrustum"/> to test for overlap.</param>
        /// <returns>
        ///   A value indicating if this <see cref="BoundingBox"/> contains,
        ///   intersects with or is disjoint with <paramref name="frustum"/>.
        /// </returns>
        public ContainmentType Contains(BoundingFrustum frustum)
        {
            //TODO: bad done here need a fix. 
            //Because question is not frustum contain box but reverse and this is not the same
            int i;
            ContainmentType contained;
            Vector3[] corners = frustum.GetCorners();

            // First we check if frustum is in box
            for (i = 0; i < corners.Length; i++)
            {
                Contains(ref corners[i], out contained);
                if (contained == ContainmentType.Disjoint)
                    break;
            }

            if (i == corners.Length) // This means we checked all the corners and they were all contain or instersect
                return ContainmentType.Contains;

            if (i != 0)             // if i is not equal to zero, we can fastpath and say that this box intersects
                return ContainmentType.Intersects;


            // If we get here, it means the first (and only) point we checked was actually contained in the frustum.
            // So we assume that all other points will also be contained. If one of the points is disjoint, we can
            // exit immediately saying that the result is Intersects
            i++;
            for (; i < corners.Length; i++)
            {
                Contains(ref corners[i], out contained);
                if (contained != ContainmentType.Contains)
                    return ContainmentType.Intersects;

            }

            // If we get here, then we know all the points were actually contained, therefore result is Contains
            return ContainmentType.Contains;
        }

        /// <summary>
        ///   Check if this <see cref="BoundingBox"/> contains a <see cref="BoundingSphere"/>.
        /// </summary>
        /// <param name="sphere">The <see cref="BoundingSphere"/> to test for overlap.</param>
        /// <returns>
        ///   A value indicating if this <see cref="BoundingBox"/> contains,
        ///   intersects with or is disjoint with <paramref name="sphere"/>.
        /// </returns>
        public ContainmentType Contains(BoundingSphere sphere)
        {
            if (sphere.Center.X - Min.X >= sphere.Radius
                && sphere.Center.Y - Min.Y >= sphere.Radius
                && sphere.Center.Z - Min.Z >= sphere.Radius
                && Max.X - sphere.Center.X >= sphere.Radius
                && Max.Y - sphere.Center.Y >= sphere.Radius
                && Max.Z - sphere.Center.Z >= sphere.Radius)
                return ContainmentType.Contains;

            double dmin = 0;

            double e = sphere.Center.X - Min.X;
            if (e < 0)
            {
                if (e < -sphere.Radius)
                {
                    return ContainmentType.Disjoint;
                }
                dmin += e * e;
            }
            else
            {
                e = sphere.Center.X - Max.X;
                if (e > 0)
                {
                    if (e > sphere.Radius)
                    {
                        return ContainmentType.Disjoint;
                    }
                    dmin += e * e;
                }
            }

            e = sphere.Center.Y - Min.Y;
            if (e < 0)
            {
                if (e < -sphere.Radius)
                {
                    return ContainmentType.Disjoint;
                }
                dmin += e * e;
            }
            else
            {
                e = sphere.Center.Y - Max.Y;
                if (e > 0)
                {
                    if (e > sphere.Radius)
                    {
                        return ContainmentType.Disjoint;
                    }
                    dmin += e * e;
                }
            }

            e = sphere.Center.Z - Min.Z;
            if (e < 0)
            {
                if (e < -sphere.Radius)
                {
                    return ContainmentType.Disjoint;
                }
                dmin += e * e;
            }
            else
            {
                e = sphere.Center.Z - Max.Z;
                if (e > 0)
                {
                    if (e > sphere.Radius)
                    {
                        return ContainmentType.Disjoint;
                    }
                    dmin += e * e;
                }
            }

            if (dmin <= sphere.Radius * sphere.Radius)
                return ContainmentType.Intersects;

            return ContainmentType.Disjoint;
        }

        /// <summary>
        ///   Check if this <see cref="BoundingBox"/> contains a <see cref="BoundingSphere"/>.
        /// </summary>
        /// <param name="sphere">The <see cref="BoundingSphere"/> to test for overlap.</param>
        /// <param name="result">
        ///   A value indicating if this <see cref="BoundingBox"/> contains,
        ///   intersects with or is disjoint with <paramref name="sphere"/>.
        /// </param>
        public void Contains(ref BoundingSphere sphere, out ContainmentType result)
        {
            result = Contains(sphere);
        }

        /// <summary>
        ///   Check if this <see cref="BoundingBox"/> contains a point.
        /// </summary>
        /// <param name="point">The <see cref="Vector3"/> to test.</param>
        /// <returns>
        ///   <see cref="ContainmentType.Contains"/> if this <see cref="BoundingBox"/> contains
        ///   <paramref name="point"/> or <see cref="ContainmentType.Disjoint"/> if it does not.
        /// </returns>
        public ContainmentType Contains(Vector3 point)
        {
            Contains(ref point, out ContainmentType result);
            return result;
        }

        /// <summary>
        ///   Check if this <see cref="BoundingBox"/> contains a point.
        /// </summary>
        /// <param name="point">The <see cref="Vector3"/> to test.</param>
        /// <param name="result">
        ///   <see cref="ContainmentType.Contains"/> if this <see cref="BoundingBox"/> contains
        ///   <paramref name="point"/> or <see cref="ContainmentType.Disjoint"/> if it does not.
        /// </param>
        public void Contains(ref Vector3 point, out ContainmentType result)
        {
            //first we get if point is out of box
            if (point.X < Min.X
                || point.X > Max.X
                || point.Y < Min.Y
                || point.Y > Max.Y
                || point.Z < Min.Z
                || point.Z > Max.Z)
            {
                result = ContainmentType.Disjoint;
            }
            else
            {
                result = ContainmentType.Contains;
            }
        }

        private static readonly Vector3 MaxVector3 = new(float.MaxValue);
        private static readonly Vector3 MinVector3 = new(float.MinValue);


        /// <summary>
        /// Create a bounding box from the given list of points.
        /// </summary>
        /// <param name="points">The array of Vector3 instances defining the point cloud to bound</param>
        /// <param name="index">The base index to start iterating from</param>
        /// <param name="count">The number of points to iterate</param>
        /// <returns>A bounding box that encapsulates the given point cloud.</returns>
        /// <exception cref="System.ArgumentException">Thrown if the given array is null or has no points.</exception>
        public static BoundingBox CreateFromPoints(Vector3[] points, int index = 0, int count = -1)
        {
            if (points == null || points.Length == 0)
                throw new ArgumentException();

            if (count == -1)
                count = points.Length;

            var minVec = MaxVector3;
            var maxVec = MinVector3;
            for (int i = index; i < count; i++)
            {                
                minVec.X = (minVec.X < points[i].X) ? minVec.X : points[i].X;
                minVec.Y = (minVec.Y < points[i].Y) ? minVec.Y : points[i].Y;
                minVec.Z = (minVec.Z < points[i].Z) ? minVec.Z : points[i].Z;

                maxVec.X = (maxVec.X > points[i].X) ? maxVec.X : points[i].X;
                maxVec.Y = (maxVec.Y > points[i].Y) ? maxVec.Y : points[i].Y;
                maxVec.Z = (maxVec.Z > points[i].Z) ? maxVec.Z : points[i].Z;
            }

            return new BoundingBox(minVec, maxVec);
        }


        /// <summary>
        /// Create a bounding box from the given list of points.
        /// </summary>
        /// <param name="points">The list of Vector3 instances defining the point cloud to bound</param>
        /// <param name="index">The base index to start iterating from</param>
        /// <param name="count">The number of points to iterate</param>
        /// <returns>A bounding box that encapsulates the given point cloud.</returns>
        /// <exception cref="System.ArgumentException">Thrown if the given list is null or has no points.</exception>
        public static BoundingBox CreateFromPoints(List<Vector3> points, int index = 0, int count = -1)
        {
            if (points == null || points.Count == 0)
                throw new ArgumentException();

            if (count == -1)
                count = points.Count;

            var minVec = MaxVector3;
            var maxVec = MinVector3;
            for (int i = index; i < count; i++)
            {
                minVec.X = (minVec.X < points[i].X) ? minVec.X : points[i].X;
                minVec.Y = (minVec.Y < points[i].Y) ? minVec.Y : points[i].Y;
                minVec.Z = (minVec.Z < points[i].Z) ? minVec.Z : points[i].Z;

                maxVec.X = (maxVec.X > points[i].X) ? maxVec.X : points[i].X;
                maxVec.Y = (maxVec.Y > points[i].Y) ? maxVec.Y : points[i].Y;
                maxVec.Z = (maxVec.Z > points[i].Z) ? maxVec.Z : points[i].Z;
            }

            return new BoundingBox(minVec, maxVec);
        }


        /// <summary>
        ///   Create the enclosing <see cref="BoundingBox"/> from the given list of points.
        /// </summary>
        /// <param name="points">The list of <see cref="Vector3"/> instances defining the point cloud to bound.</param>
        /// <returns>A <see cref="BoundingBox"/> that encloses the given point cloud.</returns>
        /// <exception cref="System.ArgumentException">Thrown if the given list has no points.</exception>
        public static BoundingBox CreateFromPoints(IEnumerable<Vector3> points)
        {
            ArgumentNullException.ThrowIfNull(points);

            var empty = true;
            var minVec = MaxVector3;
            var maxVec = MinVector3;
            foreach (var ptVector in points)
            {
                minVec.X = (minVec.X < ptVector.X) ? minVec.X : ptVector.X;
                minVec.Y = (minVec.Y < ptVector.Y) ? minVec.Y : ptVector.Y;
                minVec.Z = (minVec.Z < ptVector.Z) ? minVec.Z : ptVector.Z;

                maxVec.X = (maxVec.X > ptVector.X) ? maxVec.X : ptVector.X;
                maxVec.Y = (maxVec.Y > ptVector.Y) ? maxVec.Y : ptVector.Y;
                maxVec.Z = (maxVec.Z > ptVector.Z) ? maxVec.Z : ptVector.Z;

                empty = false;
            }
            if (empty)
                throw new ArgumentException();

            return new BoundingBox(minVec, maxVec);
        }

        /// <summary>
        ///   Create the enclosing <see cref="BoundingBox"/> of a <see cref="BoundingSphere"/>.
        /// </summary>
        /// <param name="sphere">The <see cref="BoundingSphere"/> to enclose.</param>
        /// <returns>A <see cref="BoundingBox"/> enclosing <paramref name="sphere"/>.</returns>
        public static BoundingBox CreateFromSphere(BoundingSphere sphere)
        {
            CreateFromSphere(ref sphere, out BoundingBox result);
            return result;
        }

        /// <summary>
        ///   Create the enclosing <see cref="BoundingBox"/> of a <see cref="BoundingSphere"/>.
        /// </summary>
        /// <param name="sphere">The <see cref="BoundingSphere"/> to enclose.</param>
        /// <param name="result">A <see cref="BoundingBox"/> enclosing <paramref name="sphere"/>.</param>
        public static void CreateFromSphere(ref BoundingSphere sphere, out BoundingBox result)
        {
            var corner = new Vector3(sphere.Radius);
            result.Min = sphere.Center - corner;
            result.Max = sphere.Center + corner;
        }

        /// <summary>
        ///   Create the <see cref="BoundingBox"/> enclosing two other <see cref="BoundingBox"/> instances.
        /// </summary>
        /// <param name="original">A <see cref="BoundingBox"/> to enclose.</param>
        /// <param name="additional">A <see cref="BoundingBox"/> to enclose.</param>
        /// <returns>
        ///   The <see cref="BoundingBox"/> enclosing <paramref name="original"/> and <paramref name="additional"/>.
        /// </returns>
        public static BoundingBox CreateMerged(BoundingBox original, BoundingBox additional)
        {
            CreateMerged(ref original, ref additional, out BoundingBox result);
            return result;
        }

        /// <summary>
        ///   Create the <see cref="BoundingBox"/> enclosing two other <see cref="BoundingBox"/> instances.
        /// </summary>
        /// <param name="original">A <see cref="BoundingBox"/> to enclose.</param>
        /// <param name="additional">A <see cref="BoundingBox"/> to enclose.</param>
        /// <param name="result">
        ///   The <see cref="BoundingBox"/> enclosing <paramref name="original"/> and <paramref name="additional"/>.
        /// </param>
        public static void CreateMerged(ref BoundingBox original, ref BoundingBox additional, out BoundingBox result)
        {
            result.Min.X = Math.Min(original.Min.X, additional.Min.X);
            result.Min.Y = Math.Min(original.Min.Y, additional.Min.Y);
            result.Min.Z = Math.Min(original.Min.Z, additional.Min.Z);
            result.Max.X = Math.Max(original.Max.X, additional.Max.X);
            result.Max.Y = Math.Max(original.Max.Y, additional.Max.Y);
            result.Max.Z = Math.Max(original.Max.Z, additional.Max.Z);
        }

        /// <summary>
        ///   Check if two <see cref="BoundingBox"/> instances are equal.
        /// </summary>
        /// <param name="other">The <see cref="BoundingBox"/> to compare with this <see cref="BoundingBox"/>.</param>
        /// <returns>
        ///   <code>true</code> if <paramref name="other"/> is equal to this <see cref="BoundingBox"/>,
        ///   <code>false</code> if it is not.
        /// </returns>
        public bool Equals(BoundingBox other)
        {
            return (Min == other.Min) && (Max == other.Max);
        }

        /// <summary>
        ///   Check if two <see cref="BoundingBox"/> instances are equal.
        /// </summary>
        /// <param name="obj">The <see cref="Object"/> to compare with this <see cref="BoundingBox"/>.</param>
        /// <returns>
        ///   <code>true</code> if <paramref name="obj"/> is equal to this <see cref="BoundingBox"/>,
        ///   <code>false</code> if it is not.
        /// </returns>
        public override bool Equals(object obj)
        {
            return (obj is BoundingBox) && Equals((BoundingBox)obj);
        }

        /// <summary>
        ///   Get an array of <see cref="Vector3"/> containing the corners of this <see cref="BoundingBox"/>.
        /// </summary>
        /// <returns>An array of <see cref="Vector3"/> containing the corners of this <see cref="BoundingBox"/>.</returns>
        public Vector3[] GetCorners()
        {
            return new Vector3[] {
                new(Min.X, Max.Y, Max.Z),
                new(Max.X, Max.Y, Max.Z),
                new(Max.X, Min.Y, Max.Z),
                new(Min.X, Min.Y, Max.Z),
                new(Min.X, Max.Y, Min.Z),
                new(Max.X, Max.Y, Min.Z),
                new(Max.X, Min.Y, Min.Z),
                new(Min.X, Min.Y, Min.Z)
            };
        }

        /// <summary>
        ///   Fill the first 8 places of an array of <see cref="Vector3"/>
        ///   with the corners of this <see cref="BoundingBox"/>.
        /// </summary>
        /// <param name="corners">The array to fill.</param>
        /// <exception cref="ArgumentNullException">If <paramref name="corners"/> is <code>null</code>.</exception>
        /// <exception cref="ArgumentOutOfRangeException">
        ///   If <paramref name="corners"/> has a length of less than 8.
        /// </exception>
        public void GetCorners(Vector3[] corners)
        {
            ArgumentNullException.ThrowIfNull(corners);
            if (corners.Length < 8)
            {
                throw new ArgumentOutOfRangeException(nameof(corners), "Not Enought Corners");
            }
            corners[0].X = Min.X;
            corners[0].Y = Max.Y;
            corners[0].Z = Max.Z;
            corners[1].X = Max.X;
            corners[1].Y = Max.Y;
            corners[1].Z = Max.Z;
            corners[2].X = Max.X;
            corners[2].Y = Min.Y;
            corners[2].Z = Max.Z;
            corners[3].X = Min.X;
            corners[3].Y = Min.Y;
            corners[3].Z = Max.Z;
            corners[4].X = Min.X;
            corners[4].Y = Max.Y;
            corners[4].Z = Min.Z;
            corners[5].X = Max.X;
            corners[5].Y = Max.Y;
            corners[5].Z = Min.Z;
            corners[6].X = Max.X;
            corners[6].Y = Min.Y;
            corners[6].Z = Min.Z;
            corners[7].X = Min.X;
            corners[7].Y = Min.Y;
            corners[7].Z = Min.Z;
        }

        /// <summary>
        ///   Get the hash code for this <see cref="BoundingBox"/>.
        /// </summary>
        /// <returns>A hash code for this <see cref="BoundingBox"/>.</returns>
        public override int GetHashCode()
        {
            return Min.GetHashCode() + Max.GetHashCode();
        }

        /// <summary>
        ///   Check if this <see cref="BoundingBox"/> intersects another <see cref="BoundingBox"/>.
        /// </summary>
        /// <param name="box">The <see cref="BoundingBox"/> to test for intersection.</param>
        /// <returns>
        ///   <code>true</code> if this <see cref="BoundingBox"/> intersects <paramref name="box"/>,
        ///   <code>false</code> if it does not.
        /// </returns>
        public bool Intersects(BoundingBox box)
        {
            Intersects(ref box, out bool result);
            return result;
        }

        /// <summary>
        ///   Check if this <see cref="BoundingBox"/> intersects another <see cref="BoundingBox"/>.
        /// </summary>
        /// <param name="box">The <see cref="BoundingBox"/> to test for intersection.</param>
        /// <param name="result">
        ///   <code>true</code> if this <see cref="BoundingBox"/> intersects <paramref name="box"/>,
        ///   <code>false</code> if it does not.
        /// </param>
        public void Intersects(ref BoundingBox box, out bool result)
        {
            if ((Max.X >= box.Min.X) && (Min.X <= box.Max.X))
            {
                if ((Max.Y < box.Min.Y) || (Min.Y > box.Max.Y))
                {
                    result = false;
                    return;
                }

                result = (Max.Z >= box.Min.Z) && (Min.Z <= box.Max.Z);
                return;
            }

            result = false;
            return;
        }

        /// <summary>
        ///   Check if this <see cref="BoundingBox"/> intersects a <see cref="BoundingFrustum"/>.
        /// </summary>
        /// <param name="frustum">The <see cref="BoundingFrustum"/> to test for intersection.</param>
        /// <returns>
        ///   <code>true</code> if this <see cref="BoundingBox"/> intersects <paramref name="frustum"/>,
        ///   <code>false</code> if it does not.
        /// </returns>
        public bool Intersects(BoundingFrustum frustum)
        {
            return frustum.Intersects(this);
        }

        /// <summary>
        ///   Check if this <see cref="BoundingBox"/> intersects a <see cref="BoundingFrustum"/>.
        /// </summary>
        /// <param name="sphere">The <see cref="BoundingFrustum"/> to test for intersection.</param>
        /// <returns>
        ///   <code>true</code> if this <see cref="BoundingBox"/> intersects <paramref name="sphere"/>,
        ///   <code>false</code> if it does not.
        /// </returns>
        public bool Intersects(BoundingSphere sphere)
        {
            Intersects(ref sphere, out bool result);
            return result;
        }

        /// <summary>
        ///   Check if this <see cref="BoundingBox"/> intersects a <see cref="BoundingFrustum"/>.
        /// </summary>
        /// <param name="sphere">The <see cref="BoundingFrustum"/> to test for intersection.</param>
        /// <param name="result">
        ///   <code>true</code> if this <see cref="BoundingBox"/> intersects <paramref name="sphere"/>,
        ///   <code>false</code> if it does not.
        /// </param>
        public void Intersects(ref BoundingSphere sphere, out bool result)
        {
            var squareDistance = 0.0f;
            var point = sphere.Center;
            if (point.X < Min.X) squareDistance += (Min.X - point.X) * (Min.X - point.X);
            if (point.X > Max.X) squareDistance += (point.X - Max.X) * (point.X - Max.X);
            if (point.Y < Min.Y) squareDistance += (Min.Y - point.Y) * (Min.Y - point.Y);
            if (point.Y > Max.Y) squareDistance += (point.Y - Max.Y) * (point.Y - Max.Y);
            if (point.Z < Min.Z) squareDistance += (Min.Z - point.Z) * (Min.Z - point.Z);
            if (point.Z > Max.Z) squareDistance += (point.Z - Max.Z) * (point.Z - Max.Z);
            result = squareDistance <= sphere.Radius * sphere.Radius;
        }

        /// <summary>
        ///   Check if this <see cref="BoundingBox"/> intersects a <see cref="Plane"/>.
        /// </summary>
        /// <param name="plane">The <see cref="Plane"/> to test for intersection.</param>
        /// <returns>
        ///   <code>true</code> if this <see cref="BoundingBox"/> intersects <paramref name="plane"/>,
        ///   <code>false</code> if it does not.
        /// </returns>
        public PlaneIntersectionType Intersects(Plane plane)
        {
            Intersects(ref plane, out PlaneIntersectionType result);
            return result;
        }

        /// <summary>
        ///   Check if this <see cref="BoundingBox"/> intersects a <see cref="Plane"/>.
        /// </summary>
        /// <param name="plane">The <see cref="Plane"/> to test for intersection.</param>
        /// <param name="result">
        ///   <code>true</code> if this <see cref="BoundingBox"/> intersects <paramref name="plane"/>,
        ///   <code>false</code> if it does not.
        /// </param>
        public void Intersects(ref Plane plane, out PlaneIntersectionType result)
        {
            // See https://cgvr.informatik.uni-bremen.de/teaching/cg_literatur/lighthouse3d_view_frustum_culling/index.html

            Vector3 positiveVertex;
            Vector3 negativeVertex;

            if (plane.Normal.X >= 0)
            {
                positiveVertex.X = Max.X;
                negativeVertex.X = Min.X;
            }
            else
            {
                positiveVertex.X = Min.X;
                negativeVertex.X = Max.X;
            }

            if (plane.Normal.Y >= 0)
            {
                positiveVertex.Y = Max.Y;
                negativeVertex.Y = Min.Y;
            }
            else
            {
                positiveVertex.Y = Min.Y;
                negativeVertex.Y = Max.Y;
            }

            if (plane.Normal.Z >= 0)
            {
                positiveVertex.Z = Max.Z;
                negativeVertex.Z = Min.Z;
            }
            else
            {
                positiveVertex.Z = Min.Z;
                negativeVertex.Z = Max.Z;
            }

            // Inline Vector3.Dot(plane.Normal, negativeVertex) + plane.D;
            var distance = plane.Normal.X * negativeVertex.X + plane.Normal.Y * negativeVertex.Y + plane.Normal.Z * negativeVertex.Z + plane.D;
            if (distance > 0)
            {
                result = PlaneIntersectionType.Front;
                return;
            }

            // Inline Vector3.Dot(plane.Normal, positiveVertex) + plane.D;
            distance = plane.Normal.X * positiveVertex.X + plane.Normal.Y * positiveVertex.Y + plane.Normal.Z * positiveVertex.Z + plane.D;
            if (distance < 0)
            {
                result = PlaneIntersectionType.Back;
                return;
            }

            result = PlaneIntersectionType.Intersecting;
        }

        /// <summary>
        ///   Check if this <see cref="BoundingBox"/> intersects a <see cref="Ray"/>.
        /// </summary>
        /// <param name="ray">The <see cref="Ray"/> to test for intersection.</param>
        /// <returns>
        ///   The distance along the <see cref="Ray"/> to the intersection point or
        ///   <code>null</code> if the <see cref="Ray"/> does not intesect this <see cref="BoundingBox"/>.
        /// </returns>
        public Nullable<float> Intersects(Ray ray)
        {
            return ray.Intersects(this);
        }

        /// <summary>
        ///   Check if this <see cref="BoundingBox"/> intersects a <see cref="Ray"/>.
        /// </summary>
        /// <param name="ray">The <see cref="Ray"/> to test for intersection.</param>
        /// <param name="result">
        ///   The distance along the <see cref="Ray"/> to the intersection point or
        ///   <code>null</code> if the <see cref="Ray"/> does not intesect this <see cref="BoundingBox"/>.
        /// </param>
        public void Intersects(ref Ray ray, out Nullable<float> result)
        {
            result = Intersects(ray);
        }

        /// <summary>
        ///   Check if two <see cref="BoundingBox"/> instances are equal.
        /// </summary>
        /// <param name="a">A <see cref="BoundingBox"/> to compare the other.</param>
        /// <param name="b">A <see cref="BoundingBox"/> to compare the other.</param>
        /// <returns>
        ///   <code>true</code> if <paramref name="a"/> is equal to this <paramref name="b"/>,
        ///   <code>false</code> if it is not.
        /// </returns>
        public static bool operator ==(BoundingBox a, BoundingBox b)
        {
            return a.Equals(b);
        }

        /// <summary>
        ///   Check if two <see cref="BoundingBox"/> instances are not equal.
        /// </summary>
        /// <param name="a">A <see cref="BoundingBox"/> to compare the other.</param>
        /// <param name="b">A <see cref="BoundingBox"/> to compare the other.</param>
        /// <returns>
        ///   <code>true</code> if <paramref name="a"/> is not equal to this <paramref name="b"/>,
        ///   <code>false</code> if it is.
        /// </returns>
        public static bool operator !=(BoundingBox a, BoundingBox b)
        {
            return !a.Equals(b);
        }

        internal string DebugDisplayString => $"Min( {Min.DebugDisplayString} ); Max( {Max.DebugDisplayString} )";

        /// <summary>
        /// Get a <see cref="String"/> representation of this <see cref="BoundingBox"/>.
        /// </summary>
        /// <returns>A <see cref="String"/> representation of this <see cref="BoundingBox"/>.</returns>
        public override string ToString()
        {
            return $"{{Min:{Min} Max:{Max}}}";
        }

        /// <summary>
        /// Deconstruction method for <see cref="BoundingBox"/>.
        /// </summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        public void Deconstruct(out Vector3 min, out Vector3 max)
        {
            min = Min;
            max = Max;
        }

        #endregion Public Methods
    }
}
