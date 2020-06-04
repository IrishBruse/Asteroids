using Asteroids.Objects;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using System.Collections.Generic;
using System.Linq;

namespace Asteroids
{
    public class GameEngine : Game
    {
        public static float currentTime;
        public static float deltaTime;

        public const int WINDOW_WIDTH = 1280;
        public const int WINDOW_HEIGHT = 720;

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Ship player;

        int enabledAsteroids;
        List<Asteroid> asteroids = new List<Asteroid>();

        int currentBullet = 0;
        Bullet[] bullets;

        public GameEngine()
        {
            graphics = new GraphicsDeviceManager(this);
        }

        protected override void Initialize()
        {
            graphics.PreferredBackBufferWidth = WINDOW_WIDTH;
            graphics.PreferredBackBufferHeight = WINDOW_HEIGHT;

            // Enable AA
            graphics.PreferMultiSampling = true;
            GraphicsDevice.PresentationParameters.MultiSampleCount = 4;

            graphics.ApplyChanges();

            Content.RootDirectory = "Content";

            player = new Ship(this);

            bullets = new Bullet[10];
            for (int i = 0; i < 10; i++)
            {
                bullets[i] = new Bullet();
            }

            for (int i = 0; i < 10; i++)
            {
                asteroids.Add(new Asteroid());
                asteroids[i].Enabled = true;
                enabledAsteroids++;
            }

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            base.LoadContent();
        }

        protected override void UnloadContent()
        {
            base.UnloadContent();
        }

        protected override void Update(GameTime gameTime)
        {
            deltaTime = gameTime.ElapsedGameTime.Milliseconds / 1000f;

            if (Keyboard.GetState().IsKeyDown(Keys.F) == true)
            {
                deltaTime *= 5;
            }

            currentTime = (float)gameTime.TotalGameTime.TotalMilliseconds / 1000f;

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                Exit();
            }

            player.Update();



            for (int i = 0; i < asteroids.Count; i++)
            {
                asteroids[i].Update();
            }

            for (int i = 0; i < bullets.Count(); i++)
            {
                bullets[i].Update();

                for (int j = 0; j < asteroids.Count; j++)
                {
                    if (OverlapCircle(bullets[i].Transform.Position, asteroids[j].Transform.Position, asteroids[j].radius) == true)
                    {
                        bullets[i].Enabled = false;
                        asteroids[j].Destroy(this);
                    }
                }
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            Matrix center = Matrix.CreateTranslation(WINDOW_WIDTH / 2, WINDOW_HEIGHT / 2, 0);

            spriteBatch.Begin(transformMatrix: center);
            {
                player.Draw(spriteBatch);

                for (int i = 0; i < 10; i++)
                {
                    bullets[i].Draw(spriteBatch);
                    asteroids[i].Draw(spriteBatch);
                }
            }
            spriteBatch.End();

            base.Draw(gameTime);
        }

        public void CreateBullet(Vector2 position, Vector2 forward)
        {
            bullets[currentBullet].Enabled = true;
            bullets[currentBullet].Transform.Forward = forward;
            bullets[currentBullet].Transform.Position = position;
            currentBullet++;
            if (currentBullet > 9)
            {
                currentBullet = 0;
            }
        }

        public bool OverlapCircle(Vector2 point, Vector2 center, float radius)
        {
            return (point - center).LengthSquared() < radius * radius * 0.9f;
        }
    }
}
