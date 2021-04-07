using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace TestGame
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        Texture2D ballTexture;
        Texture2D paddleTexture;
        Texture2D rightPaddleTexture;
        Vector2 ballPosition;
        Vector2 paddlePosition;
        Vector2 rightPaddlePosition;
        float ballSpeed;
        float paddleSpeed;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            ballPosition = new Vector2(_graphics.PreferredBackBufferWidth / 2, _graphics.PreferredBackBufferHeight / 2);
            ballSpeed = 500f;
            paddlePosition = new Vector2(0, _graphics.PreferredBackBufferHeight / 2);
            paddleSpeed = 1000f;
            rightPaddlePosition = new Vector2(_graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight / 2);
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            ballTexture = Content.Load<Texture2D>("orange_square");
            paddleTexture = Content.Load<Texture2D>("paddle");
            rightPaddleTexture = Content.Load<Texture2D>("paddle");
            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            var kstate = Keyboard.GetState();

            //left paddle position
            if (kstate.IsKeyDown(Keys.W))
                paddlePosition.Y -= paddleSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (kstate.IsKeyDown(Keys.S))
                paddlePosition.Y += paddleSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            
            //right paddle position
            if (paddlePosition.Y > _graphics.PreferredBackBufferHeight - paddleTexture.Height / 2)
                paddlePosition.Y = _graphics.PreferredBackBufferHeight - paddleTexture.Height / 2;
            else if (paddlePosition.Y < paddleTexture.Height / 2)
                paddlePosition.Y = paddleTexture.Height / 2;

            //ball position
            if (kstate.IsKeyDown(Keys.Up))
                rightPaddlePosition.Y -= paddleSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (kstate.IsKeyDown(Keys.Down))
                rightPaddlePosition.Y += paddleSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (rightPaddlePosition.Y > _graphics.PreferredBackBufferHeight - rightPaddleTexture.Height / 2)
                rightPaddlePosition.Y = _graphics.PreferredBackBufferHeight - rightPaddleTexture.Height / 2;
            else if (rightPaddlePosition.Y < rightPaddleTexture.Height / 2)
                rightPaddlePosition.Y = rightPaddleTexture.Height / 2;

            ballPosition.X -= (ballSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds) / 2;
            ballPosition.Y += (ballSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds) / 2f;

            if (ballPosition.Y > _graphics.PreferredBackBufferHeight - ballTexture.Height / 2)
                ballPosition.Y = _graphics.PreferredBackBufferHeight - ballTexture.Height / 2;
            else if (ballPosition.Y < ballTexture.Height / 2)
                ballPosition.Y = ballTexture.Height / 2;

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            // TODO: Add your drawing code here
            _spriteBatch.Begin();
            _spriteBatch.Draw(
                ballTexture,
                ballPosition,
                null,
                Color.White,
                0f,
                new Vector2(ballTexture.Width / 2, ballTexture.Height / 2),
                Vector2.One,
                SpriteEffects.None,
                0f
            );

            _spriteBatch.Draw(
                paddleTexture,
                paddlePosition,
                null,
                Color.White,
                0f,
                new Vector2(0, paddleTexture.Height / 2),
                Vector2.One,
                SpriteEffects.None,
                0f
            );

            _spriteBatch.Draw(
                rightPaddleTexture,
                rightPaddlePosition,
                null,
                Color.White,
                0f,
                new Vector2(rightPaddleTexture.Width, rightPaddleTexture.Height / 2),
                Vector2.One,
                SpriteEffects.None,
                0f
            );

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
