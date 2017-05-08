using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Platformer
{
    class MovingPlatform
    {
        Game1 game = null;
        Sprite sprite = new Sprite();

        public MovingPlatform (Game1 game)
        {
            this.game = game;
        }

        public void Load(ContentManager c)
        {
            AnimatedTexture sprite = new AnimatedTexture(Vector2.Zero, 0, 1, 1);
        }

        public void Update(GameTime gt)
        {
        }

        public void Draw(SpriteBatch sb)
        {
        }
    }
}
