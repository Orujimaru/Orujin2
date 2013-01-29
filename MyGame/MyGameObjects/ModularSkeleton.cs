using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Orujin.Framework;

namespace MyGame.MyGameObjects
{
    public class ModularSkeleton : GameObject
    {
        public ModularSkeleton(Vector2 position)
            : base(position, "ModularSkeleton", "TestObject")
        {
            this.rendererComponents.AddSprite(OrujinGame.GetTexture2DByName("Textures/Skeleton/RightArm"), new Vector2(-49, 6), new Vector2(41, 73), Color.White, "RightArm");
            this.rendererComponents.AddSprite(OrujinGame.GetTexture2DByName("Textures/Skeleton/TopRightArm"), new Vector2(-5, -20), new Vector2(73, 61), Color.White, "TopRightArm");

            this.rendererComponents.AddSprite(OrujinGame.GetTexture2DByName("Textures/Skeleton/TopRightLeg"), new Vector2(10, 40), new Vector2(48, 62), Color.White, "TopRightLeg");
            this.rendererComponents.AddSprite(OrujinGame.GetTexture2DByName("Textures/Skeleton/RightLeg"), new Vector2(8, 80), new Vector2(36, 74), Color.White, "RightLeg");

            this.rendererComponents.AddSprite(OrujinGame.GetTexture2DByName("Textures/Skeleton/Body"), new Vector2(20, -5), new Vector2(40, 72), Color.White, "Body");
            this.rendererComponents.AddSprite(OrujinGame.GetTexture2DByName("Textures/Skeleton/Head"), new Vector2(-15, -72), new Vector2(72, 73), Color.White, "Head");

            this.rendererComponents.AddSprite(OrujinGame.GetTexture2DByName("Textures/Skeleton/TopLeftLeg"), new Vector2(43, 48), new Vector2(46, 66), Color.White, "TopLeftLeg");
            this.rendererComponents.AddSprite(OrujinGame.GetTexture2DByName("Textures/Skeleton/LeftLeg"), new Vector2(43, 90), new Vector2(38, 73), Color.White, "LeftLeg");

            this.rendererComponents.AddSprite(OrujinGame.GetTexture2DByName("Textures/Skeleton/TopLeftArm"), new Vector2(35, -15), new Vector2(57, 39), Color.White, "TopLeftArm");
            this.rendererComponents.AddSprite(OrujinGame.GetTexture2DByName("Textures/Skeleton/LeftArm"), new Vector2(25, 22), new Vector2(60, 26), Color.White, "LeftArm");

            this.rendererComponents.AddModularAnimationToComponent("Head", OrujinGame.GetModularAnimationByName("SkeletonHead"));
            this.rendererComponents.AddModularAnimationToComponent("Body", OrujinGame.GetModularAnimationByName("SkeletonBody"));
            this.rendererComponents.AddModularAnimationToComponent("TopLeftArm", OrujinGame.GetModularAnimationByName("TopLeftArm"));
            this.rendererComponents.AddModularAnimationToComponent("LeftArm", OrujinGame.GetModularAnimationByName("LeftArm"));
            this.rendererComponents.AddModularAnimationToComponent("TopRightArm", OrujinGame.GetModularAnimationByName("TopRightArm"));
            this.rendererComponents.AddModularAnimationToComponent("RightArm", OrujinGame.GetModularAnimationByName("RightArm"));
            this.rendererComponents.AddModularAnimationToComponent("TopRightLeg", OrujinGame.GetModularAnimationByName("TopRightLeg"));
            this.rendererComponents.AddModularAnimationToComponent("RightLeg", OrujinGame.GetModularAnimationByName("RightLeg"));
            this.rendererComponents.AddModularAnimationToComponent("TopLeftLeg", OrujinGame.GetModularAnimationByName("TopLeftLeg"));
            this.rendererComponents.AddModularAnimationToComponent("LeftLeg", OrujinGame.GetModularAnimationByName("LeftLeg"));
            GameManager.game.AddObject(this);
        }
    }
}
