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

        float missileSpeed = 2;

        public MissileSprite(Texture2D newTxr, Vector2 newPos) : base(newTxr, newPos)
        {
            missileTxr = newTxr;
            missilePos = newPos;
        }

        public override void Update(GameTime gameTime, Point screenSize)
        {
            spritePos.X -= missileSpeed;

            base.Update(gameTime, screenSize);
        }
    }
}
