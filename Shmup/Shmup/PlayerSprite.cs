using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Shmup
{
    class PlayerSprite : Sprite
    {
        Texture2D spriteTexture;
        public Vector2 spritePos;
        float moveSpeed = 50;
        public int playerLives = 3;

        public PlayerSprite(Texture2D newTxr, Vector2 newPos) : base(newTxr, newPos)
        {
            spriteTexture = newTxr;
            spritePos = newPos;
        }

        public override void Update(GameTime gameTime, Point screenSize)
        {
            KeyboardState _keyboard = Keyboard.GetState();

            if (_keyboard.IsKeyDown(Keys.A)) spritePos.X += moveSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            else if (_keyboard.IsKeyDown(Keys.D)) spritePos.X -= moveSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (_keyboard.IsKeyDown(Keys.W)) spritePos.Y -= moveSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            else if (_keyboard.IsKeyDown(Keys.S)) spritePos.Y += moveSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
        }

        public void Draw(SpriteBatch _spriteBatch)
        {
            _spriteBatch.Draw(
                spriteTexture,
                new Rectangle((int)spritePos.X, (int)spritePos.Y + 30, spriteTexture.Width, spriteTexture.Height),
                Color.White);
        }
    }
}
