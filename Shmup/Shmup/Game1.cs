using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace Shmup
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        Point screenSize = new Point(800, 450);

        Texture2D saucerTex, missileTex, backTex;

        Sprite background;
        PlayerSprite playerSprite;

        List<MissileSprite> missiles = new List<MissileSprite>();
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

            background = new Sprite(backTex, new Vector2());
            playerSprite = new PlayerSprite(saucerTex, new Vector2(screenSize.X / 7, screenSize.Y / 2));
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            if (missiles.Count < 5) missiles.Add(new MissileSprite(missileTex, new Vector2(500, 200)));

            playerSprite.Update(gameTime, screenSize);
            foreach (MissileSprite missile in missiles) missile.Update(gameTime, screenSize);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            _spriteBatch.Begin();

            background.Draw(_spriteBatch);
            playerSprite.Draw(_spriteBatch);
            foreach (MissileSprite missile in missiles) missile.Draw(_spriteBatch);

            _spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
