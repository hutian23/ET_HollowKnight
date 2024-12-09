using System;
using System.Numerics;

namespace Box2DSharp.Common
{
    public struct Transform : IFormattable, IEquatable<Transform>
    {
        public Vector2 Position;

        public Rotation Rotation;

        /// Initialize using a position vector and a rotation.
        public Transform(in Vector2 position, in Rotation rotation)
        {
            Position = position;
            Rotation = rotation;
        }

        public Transform(in Vector2 position, float angle)
        {
            Position = position;
            Rotation = new Rotation(angle);
        }

        /// Set this to the identity transform.
        public void SetIdentity()
        {
            Position = Vector2.Zero;
            Rotation.SetIdentity();
        }

        /// Set this based on the position and angle.
        public void Set(in Vector2 position, float angle)
        {
            Position = position;
            Rotation.Set(angle);
        }

        /// <inheritdoc />
        public string ToString(string format, IFormatProvider formatProvider)
        {
            return ToString();
        }

        public new string ToString()
        {
            return $"({Position.X},{Position.Y}), Cos:{Rotation.Cos}, Sin:{Rotation.Sin})";
        }

        public bool Equals(Transform trans2)
        {
            return Vector2.DistanceSquared(trans2.Position, Position) < Settings.Epsilon * Settings.Epsilon && 
                   Math.Abs(trans2.Rotation.Sin - Rotation.Sin) < Settings.Epsilon &&
                   Math.Abs(trans2.Rotation.Cos - Rotation.Cos) < Settings.Epsilon;
        }
    }
}