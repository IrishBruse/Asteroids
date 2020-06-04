using Microsoft.Xna.Framework;

using System;

namespace Asteroids.Engine
{
    public struct Transform2D
    {
        /// <summary>
        /// The position in World Space
        /// </summary>
        public Vector2 Position;

        /// <summary>
        /// The rotation
        /// </summary>
        public float Rotation;

        /// <summary>
        /// The vector in the direction of the rotation
        /// </summary>
        public Vector2 Forward
        {
            get
            {
                return new Vector2((float)Math.Cos(MathHelper.ToRadians(Rotation)), (float)Math.Sin(MathHelper.ToRadians(Rotation)));
            }

            set
            {
                Rotation = (float)(Math.Atan2(value.Y, value.X) * 180 / Math.PI);
            }
        }

        /// <summary>
        /// The vector in the direction of the rotation 90 degrees to the right
        /// </summary>
        public Vector2 Right
        {
            get
            {
                return new Vector2((float)Math.Cos(MathHelper.ToRadians(Rotation + 90)), (float)Math.Sin(MathHelper.ToRadians(Rotation + 90)));
            }
        }
    }
}
