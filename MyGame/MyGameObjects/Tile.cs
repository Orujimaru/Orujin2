using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Orujin.Framework;

namespace MyGame.MyGameObjects
{
    public class Tile : GameObject
    {
        public Tile(Texture2D texture, Vector2 position, Vector2 scale, float rotation, bool flipH, bool flipV, Vector2 scrollOffset, String name, String tag)
            : base(position, name, tag)
        {
            this.rendererComponents.AddSprite(texture, new Vector2(0, 0), scale, rotation, flipH, flipV, null, Color.White, scrollOffset, name);
            GameManager.game.AddObject(this);
        }
    }
}
