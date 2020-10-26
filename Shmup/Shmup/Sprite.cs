using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Shmup
{
    class Sprite : Game1
    {
        Texture2D spriteTexture;
        public Vector2 spritePos;

        public Sprite(Texture2D newTex, Vector2 newPos)
        {
            spriteTexture = newTex;
            spritePos = newPos;
        }

        public virtual void Update(GameTime gameTime, Point screenSize) { }

        public void Draw(SpriteBatch _spriteBatch)
        {
            _spriteBatch.Draw(
                spriteTexture,
                new Rectangle((int)spritePos.X, (int)spritePos.Y + 30, spriteTexture.Width, spriteTexture.Height),
                Color.White);
        }
    }
}
