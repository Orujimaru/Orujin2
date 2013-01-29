using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Orujin.Core.Renderer
{
    public class Sprite : IRendererInterface
    {
        public RendererPackage rendererPackage;
        public SpriteAnimation spriteAnimation { get; private set; }
        public ModularAnimation modularAnimation { get; private set; }

        public string name { get; private set; }

        private Sprite(Texture2D texture, Vector2 parentOffset, Vector2 scale, float rotation, Color color, int layer, string name, int overloadIndex)
        {
            this.rendererPackage.overloadIndex = overloadIndex;

            this.spriteAnimation = null;
            this.rendererPackage.texture = texture;
            this.rendererPackage.destination = new Rectangle(0, 0, texture.Bounds.Width, texture.Bounds.Height);
            this.rendererPackage.positionOffset = Vector2.Zero;
            this.rendererPackage.parentOffset = parentOffset;        
            this.rendererPackage.color = color;
            this.rendererPackage.originalColor = color;           
            this.rendererPackage.layer = layer;
            this.rendererPackage.rotation = rotation;
            this.rendererPackage.scale = scale;
            this.rendererPackage.defaultOrigin = new Vector2(this.rendererPackage.destination.Center.X, this.rendererPackage.destination.Center.Y);
            this.rendererPackage.origin = this.rendererPackage.defaultOrigin;
            this.name = name;           
        }

        public static Sprite CreateLight(Texture2D texture, Vector2 position, Nullable<Vector2> origin, SpriteAnimation spriteAnimation, ModularAnimation ModularAnimation, Color color, string name)
        {
            Sprite s = new Sprite(texture, position, new Vector2(1,1), 0, color, 2, name, 2);
            s.SetOrigin(origin);
            s.AddAnimation(spriteAnimation);
            s.AddModularAnimation(ModularAnimation);
            return s;
        }

        public static Sprite CreateSprite(Texture2D texture, Vector2 position, Nullable<Vector2> origin, SpriteAnimation spriteAnimation, ModularAnimation ModularAnimation, Color color, string name)
        {
            Sprite s = new Sprite(texture, position, new Vector2(1,1), 0, color, 1, name, 2);
            s.SetOrigin(origin);
            s.AddAnimation(spriteAnimation);
            s.AddModularAnimation(ModularAnimation);
            return s;
        }

        public static Sprite CreateTile(Texture2D texture, Vector2 position, Vector2 scale, float rotation, bool flipH, bool flipV, Vector2 scrollOffset, string name)
        {
            Sprite s = new Sprite(texture, position, scale, rotation,  Color.White, 1, name, 7);
            s.SetOrigin(new Vector2(texture.Width/2, texture.Height/2));
            s.rendererPackage.scrollOffset = scrollOffset;
            s.rendererPackage.flipHorizontally = flipH;
            s.rendererPackage.flipVertically = flipV;
            return s;
        }

        public void SetOrigin(Nullable<Vector2> newOrigin)
        {
            if (newOrigin == null)
            {
                this.rendererPackage.origin = this.rendererPackage.defaultOrigin;
            }
            else
            {
                this.rendererPackage.origin = (Vector2)newOrigin;
            }
        }

        public void AddAnimation(SpriteAnimation animation)
        {
            if (animation == null)
            {
                this.spriteAnimation = null;
            }
            else
            {
                this.spriteAnimation = animation;
            }
            this.SetOverloadIndex();
        }

        public void AddModularAnimation(ModularAnimation animation)
        {
            if (animation == null)
            {
                this.spriteAnimation = null;
            }
            else
            {
                this.modularAnimation = animation;
            }
            this.SetOverloadIndex();
        }

        private void SetOverloadIndex()
        {
            if (this.spriteAnimation != null && this.modularAnimation == null)
            {
                this.rendererPackage.overloadIndex = 3;
            }
            else if (this.modularAnimation != null && this.spriteAnimation == null)
            {
                this.rendererPackage.overloadIndex = 7;
            }
            else if (this.modularAnimation != null)
            {
                this.rendererPackage.overloadIndex = 5;
            }    
            else
            {
                this.rendererPackage.overloadIndex = 2;
            }
        }

        public RendererPackage PrepareRendererPackage()
        {
            if (this.spriteAnimation != null)
            {
                Rectangle src = this.spriteAnimation.GetCurrentFrame();
                this.rendererPackage.source = src;
                this.rendererPackage.destination = src;
            }
            else
            {
                this.rendererPackage.source = this.rendererPackage.texture.Bounds;
            }
            if (this.modularAnimation != null)
            {
                this.rendererPackage.rotation = this.modularAnimation.rotation;
                this.rendererPackage.scale = this.modularAnimation.scale;
                this.rendererPackage.positionOffset = this.modularAnimation.positionOffset;
            }
            return this.rendererPackage;
        }
    
        public void Update(float elapsedTime)
        {
            if (this.spriteAnimation != null)
            {
                this.spriteAnimation.Update(elapsedTime);
            }
            if (this.modularAnimation != null)
            {
                this.modularAnimation.Update(elapsedTime);
            }
        }

        public void AdjustBrightness(float newBrightness)
        {
            this.rendererPackage.color = Color.Lerp(Color.Black, this.rendererPackage.originalColor, newBrightness);
        }

        public Color[] GetColorData()
        {
            Rectangle source;
            if (this.spriteAnimation == null)
            {
                source = new Rectangle(0, 0, this.rendererPackage.texture.Bounds.Width, this.rendererPackage.texture.Bounds.Height);
            }
            else
            {
                source = this.rendererPackage.source;
            }

            Color[] colorData = new Color[source.Width * source.Height];
            this.rendererPackage.texture.GetData(0, source, colorData, source.X * source.Y, source.Width * source.Height);
            return colorData;
        }
    }
}
