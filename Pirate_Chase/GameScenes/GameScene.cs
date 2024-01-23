using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Pirate_Chase.GameScenes;
using System.Collections.Generic;

namespace Pirate_Chase
{
    public abstract class GameScene : DrawableGameComponent
    {

        public List<GameComponent> Components { get; set; }
        public virtual void hide()
        {
            this.Visible = false;
            this.Enabled = false;
        }

        public virtual void show()
        {
            this.Enabled = true;
            this.Visible = true;
        }

        protected GameScene(Game game) : base(game)
        {
            Components = new List<GameComponent>();
            hide();
        }

        public override void Update(GameTime gameTime)
        {
            foreach (GameComponent item in Components)
            {
                if (item.Enabled)
                {
                    item.Update(gameTime);
                }
            }

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            foreach (GameComponent item in Components)
            {
                if (item is DrawableGameComponent comp && comp.Visible)
                {
                    comp.Draw(gameTime);
                }
            }

            base.Draw(gameTime);
        }

    }
}
