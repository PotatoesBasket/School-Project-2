using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Platformer
{
    class Player_Cutscene
    {
        IntroCutscene game = null;
        Sprite sprite = new Sprite();
        Vector2 position = Vector2.Zero;
        Vector2 velocity = Vector2.Zero;

        public Vector2 Position
        {
            get { return sprite.position; }
            set { sprite.position = value; }
        }

        public Vector2 Velocity
        {
            get { return velocity; }
            set { velocity = value; }
        }


        public Player_Cutscene(IntroCutscene game)
        {
            this.game = game;
            position = Vector2.Zero;
            velocity = Vector2.Zero;
        }

        public void Load(ContentManager content)
        {
            AnimatedTexture animation = new AnimatedTexture(Vector2.Zero, 0, 0.6f, 1);
            animation.Load(content, "player_walk_x48", 4, 5);
            sprite.Add(animation, 4, -10);
        }

        public void Update(float deltaTime)
        {
            sprite.Update(deltaTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            sprite.Draw(spriteBatch);
        }


        public void Pause()
        {
            sprite.Pause();
        }

        public void Play()
        {
            sprite.Play();
        }

        public void Flip()
        {
            sprite.SetFlipped(true);
        }

        public void Unflip()
        {
            sprite.SetFlipped(false);
        }
    }
}
