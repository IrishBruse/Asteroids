using System;
using System.Collections.Generic;
using Asteroids.Engine;
using Engine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Asteroids.Objects
{
    internal class Asteroid
    {
        public const int RADIUS = 24;

        private readonly Polygon shape;
        public readonly int size;
        private static readonly Random random = new();

        public Transform2D Transform;
        public Vector2 direction;
        private readonly float speed;
        private readonly float rotationSpeed = 1;

        public Asteroid(int size)
        {
            this.size = size;

            List<Vector2> verts = new();

            rotationSpeed = 10 + ((float)random.NextDouble() * 50);

            if (random.Next(0, 2) == 0)
            {
                rotationSpeed *= -1;
            }

            const double max = 2.0 * Math.PI;
            double step = max / random.Next(7, 12);

            for (double theta = 0.0; theta < max; theta += step)
            {
                float radiusMult = 1f;
                if (random.Next(0, 4) == 0 || theta == 0)
                {
                    radiusMult = (float)(0.4f + (random.NextDouble() * 0.6f));
                }
                verts.Add(new Vector2((float)(size * RADIUS * radiusMult * Math.Cos(theta)), (float)(size * RADIUS * radiusMult * Math.Sin(theta))));
            }

            speed = 2 + ((float)random.NextDouble() * 2);

            if (size == 1)
            {
                speed += 2;
            }

            shape = new Polygon(verts.ToArray());

            Rectangle edge = new(-AsteroidsGame.WINDOW_WIDTH / 2, -AsteroidsGame.WINDOW_HEIGHT / 2, AsteroidsGame.WINDOW_WIDTH, AsteroidsGame.WINDOW_HEIGHT);
            float a = AsteroidsGame.WINDOW_HEIGHT;
            float b = AsteroidsGame.WINDOW_WIDTH;
            float edgeLength = (2 * a) + (2 * b);

            float randomEdgeLength = (float)random.NextDouble() * edgeLength;

            if (randomEdgeLength < a)
            {
                Transform.Position = new Vector2(edge.Left, edge.Bottom + a);
            }
            else if (randomEdgeLength < a + b)
            {
                Transform.Position = new Vector2(edge.Right + randomEdgeLength - a, edge.Bottom + edge.Size.Y);
            }
            else
            {
                Transform.Position = randomEdgeLength < a + b + a
                    ? new Vector2(edge.Right + edge.Width, edge.Top + randomEdgeLength - (a + b))
                    : new Vector2(edge.Left + randomEdgeLength - (a + b + a), edge.Top);
            }

            direction.X = (float)random.NextDouble() - .5f;
            direction.Y = (float)random.NextDouble() - .5f;

            direction.Normalize();
        }

        public void Update()
        {
            Transform.Position += direction * speed * Time.DeltaTime * 20;
            Transform.Rotation += rotationSpeed * Time.DeltaTime;

            BorderWrap();
        }

        public void BorderWrap()
        {
            if (Transform.Position.Y < (-AsteroidsGame.WINDOW_HEIGHT / 2) - 1)
            {
                Transform.Position.Y += AsteroidsGame.WINDOW_HEIGHT + 2;
            }
            else if (Transform.Position.Y > (AsteroidsGame.WINDOW_HEIGHT / 2) + 1)
            {
                Transform.Position.Y -= AsteroidsGame.WINDOW_HEIGHT + 2;
            }

            if (Transform.Position.X < (-AsteroidsGame.WINDOW_WIDTH / 2) - 1)
            {
                Transform.Position.X += AsteroidsGame.WINDOW_WIDTH + 2;
            }
            else if (Transform.Position.X > (AsteroidsGame.WINDOW_WIDTH / 2) + 1)
            {
                Transform.Position.X -= AsteroidsGame.WINDOW_WIDTH + 2;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            shape.Transform = Transform;
            shape.Draw(spriteBatch, true);
        }
    }
}
