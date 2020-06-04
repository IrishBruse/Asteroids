using Asteroids.Engine;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Utility.Drawing;

namespace Asteroids.Objects
{
    class Bullet
    {
        private bool enabled;
        public bool Enabled
        {
            get
            {
                return enabled;
            }
            set
            {
                enabled = value;
                OnEnabled();
            }
        }

        private float creationTime;

        public Transform2D Transform;

        public void OnEnabled()
        {
            creationTime = GameEngine.currentTime;
        }

        public void Update()
        {
            if (Enabled == true)
            {
                if (GameEngine.currentTime > creationTime+2)
                {
                    enabled = false;
                }

                Transform.Position += Transform.Forward * GameEngine.deltaTime * 400;

                BorderWrap();
            }
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
            if (Enabled == true)
            {
                spriteBatch.DrawRectangle(new Rectangle((int)Transform.Position.X - 1, (int)Transform.Position.Y - 1, 3, 3), Color.White);
            }
        }
    }
}
