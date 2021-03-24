
using Asteroids.Engine;
using Engine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Asteroids.Objects
{
    public class Ship
    {
        public Transform2D Transform;
        private bool thrusting;
        private int framesPassed;
        private Vector2 velocity;
        private readonly Polygon shipGraphic;
        private readonly Polygon flame;
        private KeyboardState keyboardstateOld;
        private MouseState mousestateOld;
        private readonly AsteroidsGame gameState;

        public Ship(AsteroidsGame gameEngine)
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
                Transform.Rotation -= Time.DeltaTime * 180;
            }
            else if (state.IsKeyDown(Keys.D) || state.IsKeyDown(Keys.Right))
            {
                Transform.Rotation += Time.DeltaTime * 180;
            }

            if ((state.IsKeyDown(Keys.Space) && keyboardstateOld.IsKeyUp(Keys.Space)) || (Mouse.GetState().LeftButton == ButtonState.Pressed && mousestateOld.LeftButton == ButtonState.Released))
            {
                gameState.CreateBullet(Transform.Position + (Transform.Forward * 24), Transform.Forward);
            }

            Transform.Position += velocity * Time.DeltaTime;
            velocity *= 0.99f;

            BorderWrap();

            keyboardstateOld = state;
            mousestateOld = Mouse.GetState();
        }

        public void BorderWrap()
        {
            if (Transform.Position.Y < (-AsteroidsGame.WINDOW_HEIGHT / 2))
            {
                Transform.Position.Y += AsteroidsGame.WINDOW_HEIGHT;
            }
            else if (Transform.Position.Y > (AsteroidsGame.WINDOW_HEIGHT / 2))
            {
                Transform.Position.Y -= AsteroidsGame.WINDOW_HEIGHT;
            }

            if (Transform.Position.X < (-AsteroidsGame.WINDOW_WIDTH / 2))
            {
                Transform.Position.X += AsteroidsGame.WINDOW_WIDTH;
            }
            else if (Transform.Position.X > (AsteroidsGame.WINDOW_WIDTH / 2))
            {
                Transform.Position.X -= AsteroidsGame.WINDOW_WIDTH;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            flame.Transform = shipGraphic.Transform = Transform;

            shipGraphic.Draw(spriteBatch, true);

            framesPassed--;

            if (framesPassed <= 0)
            {
                if (thrusting)
                {
                    flame.Draw(spriteBatch, false);
                }
                framesPassed = 5;
            }
        }
    }
}
