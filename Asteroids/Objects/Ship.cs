
using Asteroids.Engine;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using System;

namespace Asteroids.Objects
{
    public class Ship
    {
        public Transform2D Transform;

        bool thrusting;

        int framesPassed;
        Vector2 velocity;

        Polygon shipGraphic;
        Polygon flame;

        KeyboardState stateOld;

        GameEngine gameState;

        public Ship(GameEngine gameEngine)
        {
            gameState = gameEngine;
            Transform.Position = Vector2.Zero;
            Transform.Rotation = 270;

            shipGraphic = new Polygon(
                new Vector2[]
                {
                    new Vector2(24, 0),
                    new Vector2(-24, -18),
                    new Vector2(-18, 0),
                    new Vector2(-24, 18)
                });

            flame = new Polygon(
                new Vector2[]
                {
                    new Vector2(-21, 8),
                    new Vector2(-32, 0),
                    new Vector2(-21, -8),
                });
        }

        public void Update()
        {
            KeyboardState state = Keyboard.GetState();

            if (state.IsKeyDown(Keys.W) || state.IsKeyDown(Keys.Up))
            {
                velocity += Transform.Forward * 5f;
                thrusting = true;
            }
            else
            {
                thrusting = false;
            }

            if (state.IsKeyDown(Keys.A) || state.IsKeyDown(Keys.Left))
            {
                Transform.Rotation -= GameEngine.deltaTime * 180;
            }
            else if (state.IsKeyDown(Keys.D) || state.IsKeyDown(Keys.Up))
            {
                Transform.Rotation += GameEngine.deltaTime * 180;
            }

            if (state.IsKeyDown(Keys.Space) && stateOld.IsKeyUp(Keys.Space))
            {
                gameState.CreateBullet(Transform.Position+Transform.Forward*24,Transform.Forward);
            }

            Transform.Position += velocity * GameEngine.deltaTime;
            velocity *= 0.99f;

            BorderWrap();

            stateOld = state;
        }

        public void BorderWrap()
        {
            if (Transform.Position.Y < (-GameEngine.WINDOW_HEIGHT / 2) - 30)
            {
                Transform.Position.Y += GameEngine.WINDOW_HEIGHT + 60;
            }
            else if (Transform.Position.Y > (GameEngine.WINDOW_HEIGHT / 2) + 30)
            {
                Transform.Position.Y -= GameEngine.WINDOW_HEIGHT + 60;
            }

            if (Transform.Position.X < (-GameEngine.WINDOW_WIDTH / 2) - 30)
            {
                Transform.Position.X += GameEngine.WINDOW_WIDTH + 60;
            }
            else if (Transform.Position.X > (GameEngine.WINDOW_WIDTH / 2) + 30)
            {
                Transform.Position.X -= GameEngine.WINDOW_WIDTH + 60;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            flame.Transform = shipGraphic.Transform = Transform;

            shipGraphic.Draw(spriteBatch, true);

            framesPassed--;

            if (framesPassed <= 0)
            {
                if (thrusting == true)
                {
                    flame.Draw(spriteBatch, false);
                }
                framesPassed = 5;
            }
        }
    }
}
