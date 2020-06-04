using Asteroids.Engine;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using System;

using Utility.Drawing;

namespace Asteroids.Objects
{
    class Polygon
    {
        public Transform2D Transform;

        Vector2[] vertices;
        public Polygon(Vector2[] points)
        {
            vertices = points;
        }

        public void Draw(SpriteBatch spriteBatch,bool closed)
        {
            Vector2[] verts = new Vector2[vertices.Length];

            for (int i = 0; i < vertices.Length; i++)
            {
                verts[i] = Rotate(vertices[i], Transform.Rotation);
            }

            spriteBatch.DrawPolygon(Transform.Position, verts, Color.White, 1, closed);
        }

        static Vector2 Rotate(Vector2 v, float degrees)
        {
            float radians = (float)Math.PI * degrees / 180.0f;
            float sin = (float)Math.Sin(radians);
            float cos = (float)Math.Cos(radians);

            float tx = v.X;
            float ty = v.Y;
            v.X = (cos * tx) - (sin * ty);
            v.Y = (sin * tx) + (cos * ty);
            return v;
        }
    }
}