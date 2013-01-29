using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Orujin.Framework;

namespace MyGame.MyGameObjects
{
    public class SpriteSkeleton : GameObject
    {
        public SpriteSkeleton(Vector2 position)
            : base(position, "SpriteSkeleton", "TestObject")
        {
            this.rendererComponents.AddSprite(OrujinGame.GetTexture2DByName("Textures/SkeletonSpriteSheet"), Vector2.Zero, null, Color.White, "SpriteAnimation");
            this.rendererComponents.AddAnimationToComponent("SpriteAnimation", OrujinGame.GetSpriteAnimationByName("SkeletalSpriteAnimation"));
            GameManager.game.AddObject(this);
        }
    }
}
