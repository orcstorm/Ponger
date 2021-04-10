using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Pong
{
    public class PongGame : Game
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
        bool ballHDirection;
        bool ballVDirection;
        List<SoundEffect> soundEffects;

        public PongGame()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = false;
            soundEffects = new List<SoundEffect>();
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            ballHDirection = true;
            ballPosition = new Vector2(_graphics.PreferredBackBufferWidth / 2, _graphics.PreferredBackBufferHeight / 2);
            ballSpeed = 400f;
            paddlePosition = new Vector2(0, _graphics.PreferredBackBufferHeight / 2);
            paddleSpeed = 1000f;
            rightPaddlePosition = new Vector2(_graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight / 2);
            soundEffects.Add(Content.Load < SoundEffect >("ping"));
            soundEffects.Add(Content.Load<SoundEffect>("pong"));
            soundEffects.Add(Content.Load<SoundEffect>("goal"));
            soundEffects.Add(Content.Load<SoundEffect>("plink"));
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            ballTexture = Content.Load<Texture2D>("orange_square");
            paddleTexture = Content.Load<Texture2D>("paddle");
            rightPaddleTexture = Content.Load<Texture2D>("paddle");
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
            
            if (paddlePosition.Y > _graphics.PreferredBackBufferHeight - paddleTexture.Height / 2)
                paddlePosition.Y = _graphics.PreferredBackBufferHeight - paddleTexture.Height / 2;
            else if (paddlePosition.Y < paddleTexture.Height / 2)
                paddlePosition.Y = paddleTexture.Height / 2;

            //right paddle position
            if (kstate.IsKeyDown(Keys.Up))
                rightPaddlePosition.Y -= paddleSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (kstate.IsKeyDown(Keys.Down))
                rightPaddlePosition.Y += paddleSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (rightPaddlePosition.Y > _graphics.PreferredBackBufferHeight - rightPaddleTexture.Height / 2)
                rightPaddlePosition.Y = _graphics.PreferredBackBufferHeight - rightPaddleTexture.Height / 2;
            else if (rightPaddlePosition.Y < rightPaddleTexture.Height / 2)
                rightPaddlePosition.Y = rightPaddleTexture.Height / 2;

            //ball X position
            //check for collision with left paddle, and if so reverse the direction
            if((ballPosition.Y <= paddlePosition.Y + paddleTexture.Height / 2) && (ballPosition.Y >= paddlePosition.Y - paddleTexture.Height ))
            {
                if(ballPosition.X <= paddleTexture.Width)
                {
                    soundEffects[3].Play();
                    ballHDirection = false;
                }
            }

            //check for a left goal
            if (ballPosition.X < 0 || ballPosition.X > _graphics.PreferredBackBufferWidth)
            {
                //play a goal sound
                soundEffects[2].Play();
                //move ball to the middle of the screen
                ballPosition.X = _graphics.PreferredBackBufferWidth / 2;
                ballPosition.Y = _graphics.PreferredBackBufferHeight / 2;
            }

            //check for collision with right paddle, and if so reverse the direction
            if ((ballPosition.Y <= rightPaddlePosition.Y + rightPaddleTexture.Height / 2) && (ballPosition.Y >= rightPaddlePosition.Y - rightPaddleTexture.Height))
            {
                if (ballPosition.X >= _graphics.PreferredBackBufferWidth - rightPaddleTexture.Width)
                {
                    soundEffects[3].Play();
                    ballHDirection = true;
                }
            }

            //check for a goal
            if (ballPosition.X < 0 || ballPosition.X > _graphics.PreferredBackBufferWidth)
            {
                //play a goal sound
                soundEffects[2].Play();
                //move ball to the middle of the screen
                ballPosition.X = _graphics.PreferredBackBufferWidth / 2;
                ballPosition.Y = _graphics.PreferredBackBufferHeight / 2;
            }

            //move the ball
            if (ballHDirection == true) {
                ballPosition.X -= (ballSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds) / 2;
            } else {
                ballPosition.X += (ballSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds) / 2;
            }

            //Ball Y position
            if (ballPosition.Y <= 0)
            {
                ballVDirection = false;
                soundEffects[0].Play();
            }
            else if (ballPosition.Y >= _graphics.PreferredBackBufferHeight)
            {
                ballVDirection = true;
                soundEffects[1].Play();
            }

            if (ballVDirection == true)
            {
                ballPosition.Y -= (ballSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds) / 2;
            }
            else
            {
                ballPosition.Y += (ballSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds) / 2;
            }

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
