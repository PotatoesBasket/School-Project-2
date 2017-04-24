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
    class Enemy
    {
        Sprite sprite = new Sprite();

        public Enemy()
        {
        }


        public void Load(ContentManager content)
        {
            sprite.Load(content, "enemy");
        }


        public void Update(float deltaTime)
        {
            sprite.Update(deltaTime);
        }


        public void Draw(SpriteBatch spriteBatch)
        {
            sprite.Draw(spriteBatch);
        }
    }
}
