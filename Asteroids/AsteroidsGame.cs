using System.Collections.Generic;
using System.IO;
using Asteroids.Objects;
using Engine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SpriteFontPlus;

namespace Asteroids
{
    public class AsteroidsGame : Game
    {
        public const int WINDOW_WIDTH = 1280;
        public const int WINDOW_HEIGHT = 720;
        private readonly GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        private Ship player;
        private List<Asteroid> asteroids;
        private int currentWave;
        private int currentBullet;
        private Bullet[] bullets;
        private SpriteFont font;
        private SpriteFont titleFont;
        private bool inMenu = true;

        public AsteroidsGame()
        {
            graphics = new GraphicsDeviceManager(this);
        }

        protected override void Initialize()
        {
            graphics.PreferredBackBufferWidth = WINDOW_WIDTH;
            graphics.PreferredBackBufferHeight = WINDOW_HEIGHT;

            // Enable AA
            graphics.PreferMultiSampling = true;
            GraphicsDevice.PresentationParameters.MultiSampleCount = 8;

            graphics.ApplyChanges();

            ResetGame();

            base.Initialize();
        }

        private void ResetGame()
        {
            currentWave = 0;
            player = new Ship(this);

            bullets = new Bullet[10];
            for (int i = 0; i < 10; i++)
            {
                bullets[i] = new Bullet();
            }

            asteroids = new List<Asteroid>();
            SpawnWave();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            var fontBakeResult = TtfFontBaker.Bake(File.ReadAllBytes(@"C:\\Windows\\Fonts\arial.ttf"),
            24,
            1024,
            512,
            new[]
            {
                CharacterRange.BasicLatin,
                CharacterRange.Latin1Supplement,
                CharacterRange.LatinExtendedA,
                CharacterRange.Cyrillic
            }
            );

            font = fontBakeResult.CreateSpriteFont(GraphicsDevice);

            fontBakeResult = TtfFontBaker.Bake(File.ReadAllBytes(@"C:\\Windows\\Fonts\arial.ttf"),
            128,
            1024,
            1024,
            new[]
            {
                CharacterRange.BasicLatin,
                CharacterRange.Latin1Supplement,
                CharacterRange.LatinExtendedA,
                CharacterRange.Cyrillic
            }
            );

            titleFont = fontBakeResult.CreateSpriteFont(GraphicsDevice);

            base.LoadContent();
        }

        protected override void UnloadContent()
        {
            base.UnloadContent();
        }

        protected override void Update(GameTime gameTime)
        {
            if (inMenu)
            {
                if (Keyboard.GetState().IsKeyDown(Keys.Space))
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

                Time.DeltaTime = gameTime.ElapsedGameTime.Milliseconds / 1000f;

                Time.ElapsedTime = (float)gameTime.TotalGameTime.TotalMilliseconds / 1000f;

                player.Update();

                if (asteroids.Count <= 0)
                {
                    SpawnWave();
                }

                for (int i = 0; i < asteroids.Count; i++)
                {
                    asteroids[i].Update();
                    if (OverlapCircle(player.Transform.Position, asteroids[i].Transform.Position, asteroids[i].size * Asteroid.RADIUS))
                    {
                        ResetGame();
                        inMenu = true;
                    }
                }

                for (int i = 0; i < bullets.Length; i++)
                {
                    bullets[i].Update();

                    for (int j = 0; j < asteroids.Count; j++)
                    {
                        if (bullets[i].Enabled)
                        {
                            if (OverlapCircle(bullets[i].Transform.Position, asteroids[j].Transform.Position, asteroids[j].size * Asteroid.RADIUS))
                            {
                                bullets[i].Enabled = false;
                                if (asteroids[j].size > 1)
                                {
                                    CreateAsteroid(asteroids[j].Transform.Position, asteroids[j].size - 1);
                                    CreateAsteroid(asteroids[j].Transform.Position, asteroids[j].size - 1);
                                }

                                _ = asteroids.Remove(asteroids[j]);
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
                if (inMenu)
                {
                    string titleMessage = "Asteroids";
                    Vector2 titleSize = titleFont.MeasureString(titleMessage);
                    spriteBatch.DrawString(titleFont, titleMessage, -titleSize * 0.5f, Color.White);

                    string playMessage = "Press space to play";
                    Vector2 playMessageSize = font.MeasureString(playMessage);
                    spriteBatch.DrawString(font, playMessage, new Vector2(0, 80) - playMessageSize * 0.5f, Color.White);

                    string tutorialMessage = "WASD/Arrows keys to fly\nspace or left click to shoot";
                    Vector2 tutorialMessageSize = font.MeasureString(tutorialMessage);
                    spriteBatch.DrawString(font, tutorialMessage, new Vector2(0, 140) - tutorialMessageSize * 0.5f, Color.White);

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

                    string wave = "Wave: " + currentWave;
                    Vector2 waveSize = font.MeasureString(wave);
                    spriteBatch.DrawString(font, wave, new Vector2(-(waveSize.X / 2), -(WINDOW_HEIGHT / 2) + 20), Color.White);
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

        public static bool OverlapCircle(Vector2 point, Vector2 center, float radius)
        {
            return (point - center).LengthSquared() < radius * radius;
        }

        public void SpawnWave()
        {
            currentWave++;

            for (int i = 0; i < currentWave * currentWave; i++)
            {
                asteroids.Add(new Asteroid(3));
            }
        }

        public void CreateAsteroid(Vector2 position, int size)
        {
            Asteroid asteroid = new(size);
            asteroid.Transform.Position = position;

            asteroids.Add(asteroid);
        }
    }
}
