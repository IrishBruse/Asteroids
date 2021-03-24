using System;
using Asteroids.Engine;
using Engine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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
            creationTime = Time.ElapsedTime;
        }

        public void Update()
        {
            if (Enabled == true)
            {
                if (Time.ElapsedTime > creationTime + 2)
                {
                    enabled = false;
                }

                Transform.Position += Transform.Forward * Time.DeltaTime * 400;
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
