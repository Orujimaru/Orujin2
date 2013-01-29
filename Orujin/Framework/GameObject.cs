using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FarseerPhysics.Common;
using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Orujin.Core.Renderer;

namespace Orujin.Framework
{
    public struct Identity
    {
        public string name;
        public string tag;

        public bool Equals(Identity other)
        {
            if (name.Equals(other.name) && tag.Equals(other.tag))
            {
                return true;
            }
            return false;
        }

        public bool PartlyEquals(Identity other)
        {
            if (name.Equals(other.name) || tag.Equals(other.tag))
            {
                return true;
            }
            return false;
        }
    }

    public class GameObject
    {
        /***Farseer Physics body of the game object, use addBody() to initialize it***/
        private Body physicsBody = null;

        /***Public accessor for the Farseer Physics Body ***/
        public Body body { get { return physicsBody; } private set { return; } }

        /***RendererComponent contains all the attributes the Renderer needs to render the object***/
        public RendererComponents rendererComponents { get; private set; }
        
        private Vector2 previousPosition;
        private Vector2 position;

        private Vector2 initialPosition;
        private bool constant;

        public Vector2 distanceMoved
        {
            get
            {
                return position - previousPosition;
            }
            private set { return; }
        }

        public Vector2 origin {
            get
            {
                if (prioritizeFarseerPhysics)
                {
                    return this.physicsBody.Position * Camera.PixelsPerMeter;
                }
                else
                {
                    return position + Camera.adjustedPosition;
                }
            }
            private set
            {
                return;
            }
        }
        public Vector2 nextVelocity { get; private set; }
        public float velocityModifier { get; private set; }

        public Identity identity { get; private set; }

        public bool checkForInput = false;
        public bool checkForPixelCollision = false;

        public bool prioritizeFarseerPhysics = false;

        private static int idGen = 0;
        public int id { get; private set; }

        public GameObject(Vector2 position, string name, string tag)
        {
            this.initialPosition = position;
            this.position = position;
            this.previousPosition = position;
            this.nextVelocity = Vector2.Zero;
            this.velocityModifier = 0.2f;
            this.rendererComponents = new RendererComponents(this);
           
            Identity identity = new Identity();
            identity.name = name;
            identity.tag = tag;
            this.identity = identity;
            this.constant = false;
            idGen += 1;
            this.id = idGen;
        }

        protected void SetConstant()
        {
            this.constant = true;
        }

        public virtual void Update(float elapsedTime)
        {
            if (this.constant)
            {
                this.position = initialPosition + Camera.adjustedPosition;
                if (this.physicsBody != null)
                {
                    this.physicsBody.SetTransform( this.position / Camera.PixelsPerMeter, this.physicsBody.Rotation);
                }
            }
            else
            {
                this.previousPosition = position;
                this.HandlePhysicsBody();
                this.HandleMove(elapsedTime);
            }          
            this.rendererComponents.Update(elapsedTime);            
        }

        private void HandleMove(float elapsedTime)
        {
            if (!this.prioritizeFarseerPhysics)
            {
                Vector2 velocity = this.nextVelocity;
                this.nextVelocity = Vector2.Zero;
                this.position += velocity;
                if (this.physicsBody != null)
                {
                    this.physicsBody.SetTransform(new Vector2(this.physicsBody.Position.X + (velocity.X / Camera.PixelsPerMeter), this.physicsBody.Position.Y + (velocity.Y / Camera.PixelsPerMeter)), this.physicsBody.Rotation);
                }
            }
        }

        private void HandlePhysicsBody()
        {
            if (this.physicsBody != null)
            {
                Transform transform;
                this.physicsBody.GetTransform(out transform);
                this.position = this.physicsBody.Position * Camera.PixelsPerMeter;
            }
        }

        public void Move(Vector2 direction, float magnitude)
        {
            this.nextVelocity += direction * magnitude;
        }

        public void AddForce(Vector2 direction, float magnitude)
        {
            if (this.physicsBody != null)
            {
                this.physicsBody.ApplyForce((direction * magnitude));
                this.HandlePhysicsBody();
            }
        }

        public void SetVelocity(Vector2 direction, float magnitude)
        {
            if (this.physicsBody != null)
            {
                this.physicsBody.LinearVelocity = (direction * magnitude);
                this.HandlePhysicsBody();
            }
        }

        public void ApplyLinearImpulse(Vector2 direction, float magnitude)
        {
            if (this.physicsBody != null)
            {
                this.physicsBody.ApplyLinearImpulse(direction * magnitude);
            }
        }

        public RendererComponents GetRendererComponents()
        {
            return this.rendererComponents;
        }

        public void AddBody(Body body)
        {
            this.physicsBody = body;
            this.prioritizeFarseerPhysics = true;
            this.body.OnCollision += OnCollisionEnter;
            this.body.OnSeparation += OnCollisionExit;
            this.physicsBody.SetParent(this);
            this.physicsBody.Position = this.position / Camera.PixelsPerMeter;
        }
 
        public void ShowOrigin(Texture2D originTexture)
        {
            this.rendererComponents.Debug(originTexture);
        }

        public void HideOrigin()
        {
            this.rendererComponents.StopDebugging();
        }

        public virtual bool OnCollisionEnter(Fixture fixtureA, Fixture fixtureB, FarseerPhysics.Dynamics.Contacts.Contact contact)
        {
            //Allow the collision
            return true;
        }

        public virtual void OnCollisionExit(Fixture fixtureA, Fixture fixtureB)
        {
        }
    }
}
