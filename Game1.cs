using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace ShootingGallery
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        Texture2D targetSprite;
        Texture2D crosshairsSprite;
        Texture2D backgroundSprite;
        SpriteFont gameFont;

        Vector2 targetPosition = new Vector2(300, 300);
        const int targetRadius = 45;

        MouseState mouseState;
        bool mouseReleased = true;
        int score = 0;

        double timer = 10;

        Vector2 crosshairsPosition = new Vector2(100, 100);

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = false;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            targetSprite = Content.Load<Texture2D>("target");
            crosshairsSprite = Content.Load<Texture2D>("crosshairs");
            backgroundSprite = Content.Load<Texture2D>("sky");
            gameFont = Content.Load<SpriteFont>("galleryFont");
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            if (timer > 0)
            {
                timer -= gameTime.ElapsedGameTime.TotalSeconds;
            }
            if (timer < 0)
            {
                timer = 0;
            }

            mouseState = Mouse.GetState();
            if (mouseState.LeftButton == ButtonState.Pressed && mouseReleased == true)
            {
                float mouseTargetDistanse = Vector2.Distance(targetPosition, mouseState.Position.ToVector2());
                if (mouseTargetDistanse <= targetRadius && timer > 0)
                {
                    score++;

                    Random random = new Random();
                    targetPosition.X = random.Next(targetRadius, _graphics.PreferredBackBufferWidth - targetRadius);
                    targetPosition.Y = random.Next(targetRadius + 30, _graphics.PreferredBackBufferHeight - targetRadius);
                }
                mouseReleased = false;
            }
            if (mouseState.LeftButton == ButtonState.Released && mouseReleased == false)
            {
                mouseReleased = true;
            }
            crosshairsPosition.X = mouseState.X - crosshairsSprite.Width / 2;
            crosshairsPosition.Y = mouseState.Y - crosshairsSprite.Height / 2;

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();
            _spriteBatch.Draw(backgroundSprite, new Vector2(0, 0), Color.White);
            if (timer > 0)
            {
                _spriteBatch.Draw(targetSprite, targetPosition - new Vector2(targetRadius, targetRadius), Color.White);
            }
            _spriteBatch.Draw(crosshairsSprite, crosshairsPosition, Color.White);
            _spriteBatch.DrawString(gameFont, "Score: " + score.ToString(), new Vector2(3, 3), Color.White);
            _spriteBatch.DrawString(gameFont, "Time: " + Math.Ceiling(timer).ToString(), new Vector2(150, 3), Color.Red);
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}