using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace Shmup
{
    class MissileSprite : Sprite
    {
        Texture2D missileTxr;
        Vector2 missilePos;

        float missileSpeed = 4;
        public bool dead = false;

        public MissileSprite(Texture2D newTxr, Vector2 newPos) : base(newTxr, newPos)
        {
            missileTxr = newTxr;
            missilePos = newPos;
        }

        public override void Update(GameTime gameTime, Point screenSize)
        {
            spritePos.X -= missileSpeed;
            if (spritePos.X < -spriteTexture.Width) dead = true;

            base.Update(gameTime, screenSize);
        }
    }
}
