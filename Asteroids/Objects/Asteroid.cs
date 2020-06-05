using Asteroids.Engine;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using System;
using System.Collections.Generic;

namespace Asteroids.Objects
{
    class Asteroid
    {
        public const int RADIUS = 24;

        private Polygon shape;
        public readonly int size;
        static Random random = new Random();

        public Transform2D Transform;
        public Vector2 direction;
        float speed;
        float rotationSpeed = 1;

        public Asteroid(int size)
        {
            this.size = size;

            List<Vector2> verts = new List<Vector2>();

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
                    radiusMult = (float)(0.8f + (random.NextDouble() * 0.4f));
                }
                verts.Add(new Vector2((float)(size * RADIUS * radiusMult * Math.Cos(theta)), (float)(size * RADIUS * radiusMult * Math.Sin(theta))));
            }

            speed = 2+((float)random.NextDouble()*2);

            if (size == 1)
            {
                speed += 2;
            }

            shape = new Polygon(verts.ToArray());

            Rectangle edge = new Rectangle(-GameEngine.WINDOW_WIDTH / 2, -GameEngine.WINDOW_HEIGHT / 2, GameEngine.WINDOW_WIDTH, GameEngine.WINDOW_HEIGHT);
            float a = GameEngine.WINDOW_HEIGHT;
            float b = GameEngine.WINDOW_WIDTH;
            float edgeLength = 2 * a + 2 * b;

            float randomEdgeLength = (float)random.NextDouble() * edgeLength;

            if (randomEdgeLength < a)
            {
                Transform.Position = new Vector2(edge.Left, edge.Bottom + a);
            }
            else if (randomEdgeLength < a + b)
            {
                Transform.Position = new Vector2(edge.Right + randomEdgeLength - a, edge.Bottom + edge.Size.Y);
            }
            else if (randomEdgeLength < (a + b) + a)
            {
                Transform.Position = new Vector2(edge.Right + edge.Width, edge.Top + randomEdgeLength - (a + b));
            }
            else
            {
                Transform.Position = new Vector2(edge.Left + randomEdgeLength - (a + b + a), edge.Top);
            }

            direction.X = (float)random.NextDouble() - .5f;
            direction.Y = (float)random.NextDouble() - .5f;

            direction.Normalize();
        }

        public void Update()
        {
            Transform.Position += direction * speed * GameEngine.deltaTime * 20;
            Transform.Rotation += rotationSpeed * GameEngine.deltaTime;

            BorderWrap();
        }

        public void BorderWrap()
        {
            if (Transform.Position.Y < (-GameEngine.WINDOW_HEIGHT / 2) - 1)
            {
                Transform.Position.Y += GameEngine.WINDOW_HEIGHT + 2;
            }
            else if (Transform.Position.Y > (GameEngine.WINDOW_HEIGHT / 2) + 1)
            {
                Transform.Position.Y -= GameEngine.WINDOW_HEIGHT + 2;
            }

            if (Transform.Position.X < (-GameEngine.WINDOW_WIDTH / 2) - 1)
            {
                Transform.Position.X += GameEngine.WINDOW_WIDTH + 2;
            }
            else if (Transform.Position.X > (GameEngine.WINDOW_WIDTH / 2) + 1)
            {
                Transform.Position.X -= GameEngine.WINDOW_WIDTH + 2;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            shape.Transform = Transform;
            shape.Draw(spriteBatch, true);
        }
    }
}
