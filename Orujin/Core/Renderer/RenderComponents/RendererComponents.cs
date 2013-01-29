using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Orujin.Framework;

namespace Orujin.Core.Renderer
{
    public class RendererComponents
    {
        private List<Sprite> children;
        private bool debugging = false;
        private Texture2D debugTexture = null;
        private GameObject parent = null;

        public RendererComponents(GameObject parent)
        {
            this.parent = parent;
            this.children = new List<Sprite>();
        }

        public List<RendererPackage> GetRendererPackages()
        {
            List<RendererPackage> rendererPackages = new List<RendererPackage>();

            List<Sprite> tempChildren = this.GetChildren();

            foreach (Sprite sprite in tempChildren)
            {
                rendererPackages.Add(sprite.rendererPackage);
            }

            //Create a RendererPackage for the origin if debugging is true
            if (this.debugging)
            {
                RendererPackage debugPackage = new RendererPackage();
                debugPackage.texture = this.debugTexture;
                debugPackage.destination = new Rectangle((int)(this.parent.origin.X - 5), (int)(this.parent.origin.Y - 5), 10, 10);
                debugPackage.overloadIndex = 1;
                debugPackage.layer = 100;
                debugPackage.color = Color.Black;
                rendererPackages.Add(debugPackage);
            }
            return rendererPackages;
        }

        public List<Sprite> GetChildren()
        {
            foreach (Sprite sprite in this.children)
            {
                sprite.PrepareRendererPackage();
                float widthOffset = sprite.rendererPackage.destination.Width / 2;
                float heightOffset = sprite.rendererPackage.destination.Height / 2;

                Vector2 parentOffset = sprite.rendererPackage.parentOffset;

                Vector2 originOffset = new Vector2(widthOffset, heightOffset);
                if (sprite.rendererPackage.overloadIndex >= 5)
                {
                    originOffset = originOffset - sprite.rendererPackage.origin;             
                }

                sprite.rendererPackage.position = this.parent.origin - originOffset + parentOffset + sprite.rendererPackage.positionOffset;
                sprite.rendererPackage.destination = new Rectangle((int)sprite.rendererPackage.position.X, (int)sprite.rendererPackage.position.Y, sprite.rendererPackage.destination.Width, sprite.rendererPackage.destination.Height);                         
            }
            return this.children;
        }

        public void Update(float elapsedTime)
        {
            foreach (IRendererInterface iri in this.children)
            {
                iri.Update(elapsedTime);
            }
        }

        public void AddLight(Texture2D texture, Vector2 offset, Nullable<Vector2> origin, Color color, string name)
        {
            this.children.Add(Sprite.CreateLight(texture, offset, origin, null, null, color, name));
        }

        public void AddSprite(Texture2D texture, Vector2 offset, Nullable<Vector2> origin, Color color, string name)
        {
            this.children.Add(Sprite.CreateSprite(texture, offset, origin, null, null, color, name));
        }

        public void AddSprite(Texture2D texture, Vector2 offset, Vector2 scale, float rotation, bool flipH, bool flipV, Nullable<Vector2> origin, Color color, Vector2 scrollSpeed, string name)
        {
            //this.children.Add(Sprite.CreateSprite(texture, offset, origin, null, null, color, name));
            this.children.Add(Sprite.CreateTile(texture, offset, scale, rotation, flipH, flipV, scrollSpeed, name));
        }

        public void AddSprite(Sprite sprite)
        {
            this.children.Add(sprite);
        }

        public int AddAnimationToComponent(string componentName, SpriteAnimation animation)
        {
            for (int x = 0; x < this.children.Count(); x++)
            {
                if (this.children[x].name == componentName)
                {
                    this.children[x].AddAnimation(animation);
                    return 1;
                }
            }
            return 0;
        }

        public int AddModularAnimationToComponent(string componentName, ModularAnimation animation)
        {
            for (int x = 0; x < this.children.Count(); x++)
            {
                if (this.children[x].name == componentName)
                {
                    this.children[x].AddModularAnimation(animation);
                    return 1;
                }
            }
            return 0;
        }

        public void AdjustBrightness(float newBrightness)
        {
            for (int x = 0; x < this.children.Count(); x++)
            {
                this.children[x].AdjustBrightness(newBrightness);
            }
        }

        public void Debug(Texture2D texture)
        {
            this.debugging = true;
            this.debugTexture = texture;            
        }
        public void StopDebugging()
        {
            this.debugging = false;
            this.debugTexture = null;
        }

    }
}
