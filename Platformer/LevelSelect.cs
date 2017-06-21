using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;

namespace Platformer
{
    class LevelSelect : AIE.State
    {
        Game1 game = null;
        bool isLoaded = false;
        SpriteFont font = null;

        public LevelSelect(Game1 game) : base()
        {
            this.game = game;
        }

        public override void Update(ContentManager content, GameTime gameTime)
        {
            if (isLoaded == false)
            {
                isLoaded = true;
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
        }

        public override void CleanUp()
        {
            isLoaded = false;
        }
    }
}
