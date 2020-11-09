using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Shmup
{
    class PlayerSprite : Sprite
    {
        float moveSpeed = 500;
        public int playerLives = 10;

        public PlayerSprite(Texture2D newTxr, Vector2 newPos) : base(newTxr, newPos)
        {
        }

        public override void Update(GameTime gameTime, Point screenSize)
        {
            KeyboardState _keyboard = Keyboard.GetState();

            if (_keyboard.IsKeyDown(Keys.A)) spritePos.X += moveSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            else if (_keyboard.IsKeyDown(Keys.D)) spritePos.X -= moveSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (_keyboard.IsKeyDown(Keys.W)) spritePos.Y -= moveSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            else if (_keyboard.IsKeyDown(Keys.S)) spritePos.Y += moveSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;

            spritePos = Vector2.Clamp(
                spritePos,
                new Vector2(),
                new Vector2(screenSize.X - spriteTexture.Width, screenSize.Y - spriteTexture.Height)
            );
        }
    }
}
