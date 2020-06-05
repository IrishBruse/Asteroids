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
        List<Asteroid> asteroids;

        int currentBullet = 0;
        Bullet[] bullets;

        int score;
        SpriteFont font;

        bool inMenu = true;

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

            ResetGame();

            base.Initialize();
        }

        private void ResetGame()
        {
            score = 0;
            player = new Ship(this);

            bullets = new Bullet[10];
            for (int i = 0; i < 10; i++)
            {
                bullets[i] = new Bullet();
            }

            asteroids = new List<Asteroid>();
            for (int i = 0; i < 4; i++)
            {
                asteroids.Add(new Asteroid(3));
                enabledAsteroids++;
            }
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            font = Content.Load<SpriteFont>("Score");

            base.LoadContent();
        }

        protected override void UnloadContent()
        {
            base.UnloadContent();
        }

        protected override void Update(GameTime gameTime)
        {
            if (inMenu == true)
            {
                if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                {
                    Exit();
                }

                if (Keyboard.GetState().IsKeyDown(Keys.Space) == true)
                {
                    inMenu = false;
                }
            }
            else
            {
                if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                {
                    inMenu = true;
                }

                deltaTime = gameTime.ElapsedGameTime.Milliseconds / 1000f;

                currentTime = (float)gameTime.TotalGameTime.TotalMilliseconds / 1000f;

                player.Update();

                for (int i = 0; i < asteroids.Count; i++)
                {
                    asteroids[i].Update();
                    if (OverlapCircle(player.Transform.Position, asteroids[i].Transform.Position, asteroids[i].size * Asteroid.RADIUS) == true)
                    {
                        ResetGame();
                        inMenu = true;
                    }
                }

                for (int i = 0; i < bullets.Count(); i++)
                {
                    bullets[i].Update();

                    for (int j = 0; j < asteroids.Count; j++)
                    {
                        if (bullets[i].Enabled == true)
                        {
                            if (OverlapCircle(bullets[i].Transform.Position, asteroids[j].Transform.Position, asteroids[j].size * Asteroid.RADIUS) == true)
                            {
                                bullets[i].Enabled = false;
                                if (asteroids[j].size > 1)
                                {
                                    CreateAsteroid(asteroids[j].Transform.Position, asteroids[j].size - 1);
                                    CreateAsteroid(asteroids[j].Transform.Position, asteroids[j].size - 1);
                                }

                                switch (asteroids[j].size)
                                {
                                    case 3:
                                        score += 100;
                                        break;
                                    case 2:
                                        score += 200;
                                        break;
                                    case 1:
                                        score += 250;
                                        break;
                                }

                                asteroids.Remove(asteroids[j]);
                                break;
                            }
                        }
                    }
                }
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            Matrix center = Matrix.CreateTranslation(WINDOW_WIDTH / 2, WINDOW_HEIGHT / 2, 0);

            spriteBatch.Begin(transformMatrix: center);
            {
                if (inMenu == true)
                {
                    string msg = "Press space to play";

                    Vector2 stringSize = font.MeasureString(msg);

                    spriteBatch.DrawString(font, msg, new Vector2(-(stringSize.X / 2), (WINDOW_HEIGHT / 2) - 100), Color.White);
                }
                else
                {
                    GraphicsDevice.Clear(Color.Black);

                    player.Draw(spriteBatch);

                    for (int i = 0; i < bullets.Length; i++)
                    {
                        bullets[i].Draw(spriteBatch);
                    }
                    for (int i = 0; i < asteroids.Count; i++)
                    {
                        asteroids[i].Draw(spriteBatch);
                    }

                    spriteBatch.DrawString(font, score.ToString(), new Vector2(0, -(WINDOW_HEIGHT / 2) + 40), Color.White);
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
            return (point - center).LengthSquared() < radius * radius;
        }

        public void CreateAsteroid(Vector2 position, int size)
        {
            Asteroid asteroid = new Asteroid(size);
            asteroid.Transform.Position = position;

            asteroids.Add(asteroid);
        }
    }
}
