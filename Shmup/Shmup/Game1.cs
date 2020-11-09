using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Security.Cryptography;

//bigger particles
//particles on player death
//progression / timer
//sound
//missile acceleration

namespace Shmup
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        Point screenSize = new Point(800, 450);
        Random rand = new Random();
        float spawnCooldown = 2;

        Texture2D saucerTex, missileTex, backTex, particleTex;
        SpriteFont uiFont, bigFont;

        Sprite background;
        PlayerSprite playerSprite;

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

            background = new Sprite(backTex, new Vector2());
            playerSprite = new PlayerSprite(saucerTex, new Vector2(screenSize.X / 7, screenSize.Y / 2));
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            if(spawnCooldown > 0)
            {
                spawnCooldown -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            else if(playerSprite.playerLives > 0 && missiles.Count < 5)
            {
                missiles.Add(new MissileSprite(missileTex, new Vector2(rand.Next(750, 1000), rand.Next(450 - missileTex.Height))));
                spawnCooldown = (float)(rand.NextDouble() + 0.5);
            }

            if(playerSprite.playerLives > 0) playerSprite.Update(gameTime, screenSize);

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
                    if(playerSprite.playerLives == 0)
                    {
                        for (int i = 0; i < 32; i++)
                            particleList.Add(new ParticleSprite(particleTex,
                                new Vector2(
                                    missile.spritePos.X + (missileTex.Width / 2) - (particleTex.Width / 2),
                                    missile.spritePos.Y + (missileTex.Height / 2) - (particleTex.Width / 2)
                                    )
                                ));
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
