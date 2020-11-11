using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Security.Cryptography;

//sound

namespace Shmup
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        Point screenSize = new Point(800, 450);
        Random rand = new Random();
        float spawnCooldown = 2;
        float playTime = 0;
        bool musicPlaying = false;
        float volume = 0.3f;

        Texture2D saucerTex, missileTex, backTex, particleTex;
        SpriteFont uiFont, bigFont;

        Sprite background;
        PlayerSprite playerSprite;
        SoundEffect missileBoom, shipBoom;
        Song music;

        List<MissileSprite> missiles = new List<MissileSprite>();
        List<ParticleSprite> particleList = new List<ParticleSprite>();
        //Random comment for no reason

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            _graphics.PreferredBackBufferWidth = screenSize.X;
            _graphics.PreferredBackBufferHeight = screenSize.Y;
            _graphics.ApplyChanges();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            backTex = Content.Load<Texture2D>("back");
            saucerTex = Content.Load<Texture2D>("saucer");
            missileTex = Content.Load<Texture2D>("missile");
            uiFont = Content.Load<SpriteFont>("DMFont");
            bigFont = Content.Load<SpriteFont>("BigFont");
            particleTex = Content.Load<Texture2D>("particle");
            missileBoom = Content.Load<SoundEffect>("Missile Boom");
            shipBoom = Content.Load<SoundEffect>("Ship Boom");
            music = Content.Load<Song>("Landing v1 Looping");

            background = new Sprite(backTex, new Vector2());
            playerSprite = new PlayerSprite(saucerTex, new Vector2(screenSize.X / 7, screenSize.Y / 2));
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            if (!musicPlaying)
            {
                MediaPlayer.Volume = volume;
                MediaPlayer.IsRepeating = true;
                MediaPlayer.Play(music);
                musicPlaying = true;
            }

            if(spawnCooldown > 0)
            {
                spawnCooldown -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            else if(playerSprite.playerLives > 0 && missiles.Count < (Math.Min(playTime, 120f) / 120f) * 8 + 2)
            {
                missiles.Add(new MissileSprite(missileTex, new Vector2(rand.Next(750, 1000), rand.Next(450 - missileTex.Height))));
                spawnCooldown = (float)(rand.NextDouble() + 0.5);
            }

            if (playerSprite.playerLives > 0)
            {
                playerSprite.Update(gameTime, screenSize);
                playTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
            }

            foreach (MissileSprite missile in missiles)
            {
                missile.Update(gameTime, screenSize);

                if(playerSprite.playerLives > 0 && playerSprite.isColliding(missile))
                {
                    for(int i = 0; i < 16; i++)
                        particleList.Add(new ParticleSprite(particleTex,
                            new Vector2(
                                missile.spritePos.X + (missileTex.Width / 2) - (particleTex.Width / 2),
                                missile.spritePos.Y + (missileTex.Height / 2) - (particleTex.Width / 2)
                                )
                            ));

                    missile.dead = true;
                    playerSprite.playerLives--;
                    missileBoom.Play();
                    if(playerSprite.playerLives == 0)
                    {
                        for (int i = 0; i < 32; i++)
                            particleList.Add(new ParticleSprite(particleTex,
                                new Vector2(
                                    missile.spritePos.X + (missileTex.Width / 2) - (particleTex.Width / 2),
                                    missile.spritePos.Y + (missileTex.Height / 2) - (particleTex.Width / 2)
                                    )
                                ));
                        shipBoom.Play();
                    }
                }
            }

            foreach (ParticleSprite particle in particleList) particle.Update(gameTime, screenSize);

            missiles.RemoveAll(missile => missile.dead);
            particleList.RemoveAll(particle => particle.currentLife <= 0);

            //Debug.Write(missiles.Count);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();

            background.Draw(_spriteBatch);
            if(playerSprite.playerLives >= 0) playerSprite.Draw(_spriteBatch);
            foreach (MissileSprite missile in missiles) missile.Draw(_spriteBatch);
            foreach (ParticleSprite particle in particleList) particle.Draw(_spriteBatch);

            _spriteBatch.DrawString(uiFont, "Player lives: " + playerSprite.playerLives, new Vector2(6, 22), Color.White);

            _spriteBatch.DrawString(uiFont, "Time: " + Math.Round(playTime), new Vector2(6, 44), Color.White);

            if(playerSprite.playerLives <= 0)
            {
                Vector2 textSize = bigFont.MeasureString("GAME OVER");
                _spriteBatch.DrawString(
                    bigFont,
                    "GAME OVER",
                    new Vector2((screenSize.X / 2) - (textSize.X / 2) - 5, (screenSize.Y / 2) - (textSize.Y / 2) - 5),
                    Color.Gray);
                _spriteBatch.DrawString(
                    bigFont,
                    "GAME OVER",
                    new Vector2((screenSize.X / 2) - (textSize.X / 2), (screenSize.Y / 2) - (textSize.Y / 2)),
                    Color.White);
            }

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
