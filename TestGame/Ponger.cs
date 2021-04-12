using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace Pong
{
    public class PongGame : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        Texture2D ballTexture;
        Texture2D paddleTexture;
        Texture2D rightPaddleTexture;
        Texture2D PongerTexture;
        Vector2 ballPosition;
        Vector2 paddlePosition;
        Vector2 rightPaddlePosition;
        Vector2 PongerPosition;
        float ballSpeed;
        float paddleSpeed;
        bool ballHDirection;
        bool ballVDirection;
        List<SoundEffect> soundEffects;
        bool isGameWon;
        int leftPaddleScore;
        int rightPaddleScore;
        int winningScore;
        List<Texture2D> scoreCards;
        Vector2 leftScorePosition;
        Texture2D leftScoreTexture;
        Vector2 rightScorePosition;
        Texture2D rightScoreTexture;
        float Ydivisor;
        Random rand;

        public PongGame()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = false;
            soundEffects = new List<SoundEffect>();
            scoreCards = new List<Texture2D>();
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            ballHDirection = true;
            ballPosition = new Vector2(_graphics.PreferredBackBufferWidth / 2, _graphics.PreferredBackBufferHeight / 2);
            ballSpeed = 1800f;
            paddlePosition = new Vector2(0, _graphics.PreferredBackBufferHeight / 2);
            paddleSpeed = 1000f;
            PongerPosition = new Vector2(_graphics.PreferredBackBufferWidth / 2 , 0);
            rightPaddlePosition = new Vector2(_graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight / 2);
            soundEffects.Add(Content.Load < SoundEffect >("ping"));
            soundEffects.Add(Content.Load<SoundEffect>("pong"));
            soundEffects.Add(Content.Load<SoundEffect>("goal"));
            soundEffects.Add(Content.Load<SoundEffect>("plink"));
            leftPaddleScore = 0;
            rightPaddleScore = 0;
            scoreCards.Add(Content.Load<Texture2D>("zero"));
            scoreCards.Add(Content.Load<Texture2D>("one"));
            scoreCards.Add(Content.Load<Texture2D>("two"));
            scoreCards.Add(Content.Load<Texture2D>("three"));
            scoreCards.Add(Content.Load<Texture2D>("four"));
            scoreCards.Add(Content.Load<Texture2D>("five"));
            winningScore = 5;
            leftScorePosition = new Vector2(5, 5);
            rightScorePosition = new Vector2(_graphics.PreferredBackBufferWidth, 5);
            isGameWon = true;
            rand = new Random();
            Ydivisor = (float)(rand.NextDouble() * 4d);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            ballTexture = Content.Load<Texture2D>("orange_square");
            paddleTexture = Content.Load<Texture2D>("paddle");
            rightPaddleTexture = Content.Load<Texture2D>("paddle");
            PongerTexture = Content.Load<Texture2D>("ponger");
            leftScoreTexture = scoreCards[0];
            rightScoreTexture = scoreCards[0];
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            var kstate = Keyboard.GetState();

            //left paddle position
            if (kstate.IsKeyDown(Keys.Up))
                paddlePosition.Y -= paddleSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (kstate.IsKeyDown(Keys.Down))
                paddlePosition.Y += paddleSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            
            if (paddlePosition.Y > _graphics.PreferredBackBufferHeight - paddleTexture.Height / 2)
                paddlePosition.Y = _graphics.PreferredBackBufferHeight - paddleTexture.Height / 2;
            else if (paddlePosition.Y < paddleTexture.Height / 2)
                paddlePosition.Y = paddleTexture.Height / 2;

            //right paddle position
            rightPaddlePosition.Y = ballPosition.Y;

            if (rightPaddlePosition.Y > _graphics.PreferredBackBufferHeight - paddleTexture.Height / 2)
                rightPaddlePosition.Y = _graphics.PreferredBackBufferHeight - paddleTexture.Height / 2;
            else if (rightPaddlePosition.Y < paddleTexture.Height / 2)
                rightPaddlePosition.Y = paddleTexture.Height / 2;

            if (isGameWon == true)
            {
                if (gameTime.TotalGameTime.Ticks % 8m == 0m)
                {
                    ballPosition.Y = (float)(rand.NextDouble() * _graphics.PreferredBackBufferHeight);
                }
                ballPosition.X = _graphics.PreferredBackBufferWidth / 2;
                rightPaddlePosition.Y = _graphics.PreferredBackBufferHeight / 2;
                if(Keyboard.GetState().IsKeyDown(Keys.R)) {
                    Reset(gameTime);
                }
                base.Update(gameTime);
            }
            else
            {


                //ball Xposition
                //check for collision with left paddle, and if so reverse the direction
                if ((ballPosition.Y <= paddlePosition.Y + paddleTexture.Height / 2) && (ballPosition.Y >= paddlePosition.Y - paddleTexture.Height))
                {
                    if (ballPosition.X <= paddleTexture.Width)
                    {
                        soundEffects[3].Play();
                        Ydivisor = (float)rand.NextDouble() * 2.0f;
                        ballHDirection = false;
                    }
                }

                //check for a right goal
                if (ballPosition.X < 0)
                {
                    //play a goal sound
                    soundEffects[2].Play();

                    //increment the score
                    rightPaddleScore = rightPaddleScore + 1;
                    rightScoreTexture = scoreCards[rightPaddleScore];

                    //move ball to the middle of the screen
                    ballPosition.X = _graphics.PreferredBackBufferWidth / 2;
                    ballPosition.Y = (float)(rand.NextDouble()) * _graphics.PreferredBackBufferHeight;

                    if (rightPaddleScore == winningScore)
                    {
                        isGameWon = true;
                    }
                }

                //check for collision with right paddle, and if so reverse the direction
                if ((ballPosition.Y <= rightPaddlePosition.Y + rightPaddleTexture.Height / 2) && (ballPosition.Y >= rightPaddlePosition.Y - rightPaddleTexture.Height))
                {
                    if (ballPosition.X >= _graphics.PreferredBackBufferWidth - rightPaddleTexture.Width)
                    {
                        soundEffects[3].Play();
                        Ydivisor = (float)rand.NextDouble() * 2.0f;
                        ballHDirection = true;
                    }
                }

                //check for a left goal
                if (ballPosition.X > _graphics.PreferredBackBufferWidth)
                {
                    //play a goal sound
                    soundEffects[2].Play();
                    leftPaddleScore = leftPaddleScore + 1;
                    leftScoreTexture = scoreCards[leftPaddleScore];
                    //move ball to the middle of the screen
                    ballPosition.X = _graphics.PreferredBackBufferWidth / 2;
                    ballPosition.Y = (float)(rand.NextDouble()) * _graphics.PreferredBackBufferHeight;

                    if (leftPaddleScore == winningScore)
                    {
                        isGameWon = true;
                    }
                }

                //move the ball
                if (ballHDirection == true)
                {
                    ballPosition.X -= (ballSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds) / 2;
                }
                else
                {
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
                    ballPosition.Y -= (ballSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds) / (Ydivisor * 1.5f + 2.0f);
                }
                else
                {
                    ballPosition.Y += (ballSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds) / (Ydivisor * 1.5f + 2.0f);
                }


                base.Update(gameTime);
            }
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            // TODO: Add your drawing code here
            _spriteBatch.Begin();

            _spriteBatch.Draw(
                PongerTexture,
                PongerPosition,
                null,
                Color.White,
                0f,
                new Vector2(PongerTexture.Width / 2, 0),
                Vector2.One,
                SpriteEffects.None,
                0f
            );

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

            _spriteBatch.Draw(
                leftScoreTexture,
                leftScorePosition,
                null,
                Color.White,
                0f,
                new Vector2(0, 0),
                Vector2.One,
                SpriteEffects.None,
                0f
            );
  
            _spriteBatch.Draw(
                rightScoreTexture,
                rightScorePosition,
                null,
                Color.White,
                0f,
                new Vector2(rightPaddleTexture.Width + 5, 0),
                Vector2.One,
                SpriteEffects.None,
                0f
            );



            _spriteBatch.End();

            base.Draw(gameTime);
        }
        protected void Reset(GameTime gameTime)
        {
            rightPaddleScore = 0;
            rightScoreTexture = scoreCards[0];
            leftPaddleScore = 0;
            leftScoreTexture = scoreCards[0];
            ballPosition = new Vector2(_graphics.PreferredBackBufferWidth / 2, _graphics.PreferredBackBufferHeight / 2);
            isGameWon = false;
            base.Update(gameTime);
        }
    }



}