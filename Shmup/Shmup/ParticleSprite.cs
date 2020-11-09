using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shmup
{
    class ParticleSprite : Sprite
    {
        Random rand = new Random();
        Vector2 velocity;
        float maxLife;
        public float currentLife;
        Color particleColor;

        public ParticleSprite(Texture2D newTxr, Vector2 newPos) : base(newTxr, newPos)
        {
            maxLife = (float)(rand.NextDouble() / 2 + 0.5);
            currentLife = maxLife;

            velocity = new Vector2(
                (float)(rand.NextDouble() * 100 + 50),
                (float)(rand.NextDouble() * 100 + 50)
                );

            if (rand.Next(2) > 0) velocity.X *= -1;
            if (rand.Next(2) > 0) velocity.Y *= -1;

            particleColor = new Color((float)
                (rand.NextDouble() / 2 + 0.5),
                0.25f,
                (float)(rand.NextDouble() / 2 + 0.25)
                );
        }

        public override void Update(GameTime gameTime, Point screenSize)
        {
            spritePos += velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;
            currentLife -= (float)gameTime.ElapsedGameTime.TotalSeconds;
        }

        public new void Draw(SpriteBatch _spriteBatch)
        {
            _spriteBatch.Draw(
                spriteTexture,
                new Rectangle(
                    (int)spritePos.X,
                    (int)spritePos.Y,
                    (int)(spriteTexture.Width * (currentLife / maxLife) * 2),
                    (int)(spriteTexture.Height * (currentLife / maxLife) * 2)
                    ),
                particleColor
                );
        }
    }
}
