using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FarseerPhysics.Common;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using Microsoft.Xna.Framework;
using Orujin.Core.Renderer;
using Orujin.Framework;

namespace MyGame.MyGameObjects
{
    public class Player : GameObject
    {
        private const float MaximumOwnVelocity = 4.0f;
        public bool isOnGround = false;
        private bool decelerate = false;

        private float zeroVerticalVelocityFor = 0;
        private float resetJumpAfterZeroVerticalVelocityFor = 100;

        public Player(Vector2 position, string name)
            : base(position, name, "Player")
        {
            Body body = BodyFactory.CreateCapsule(GameManager.game.world, 100 / Camera.PixelsPerMeter, 20 / Camera.PixelsPerMeter, 20, 20 / Camera.PixelsPerMeter, 20, 1.0f, position / Camera.PixelsPerMeter);
            body.BodyType = BodyType.Dynamic;
            this.AddBody(body);
            this.checkForInput = true;
            GameManager.game.AddObject(this);
            GameManager.importantObjectA = this;
            Camera.SetParent(this);
        }

        public void Jump(float strength)
        {
            if (this.isOnGround)
            {
                this.ApplyLinearImpulse(new Vector2(0, -strength), 2.0f);
                this.isOnGround = false;
            }
        }

        public void MovePlayer(Vector2 direction, float magnitude)
        {
            if (this.body.LinearVelocity.X < MaximumOwnVelocity && direction.X > 0)
            {
                this.body.ApplyForce(direction * magnitude);
            }
            else if (this.body.LinearVelocity.X > -MaximumOwnVelocity && direction.X < 0)
            {
                this.body.ApplyForce(direction * magnitude);
            }
            this.decelerate = false;
        }

        public override void Update(float elapsedTime)
        {
            this.FailSafeResetJump(elapsedTime);
            this.body.FixedRotation = true;
            base.Update(elapsedTime);
            this.HandleDeceleration(elapsedTime);
        }

        private void FailSafeResetJump(float elapsedTime)
        {
            if (this.body.LinearVelocity.Y == 0)
            {
                this.zeroVerticalVelocityFor += elapsedTime;
                if (this.zeroVerticalVelocityFor >= this.resetJumpAfterZeroVerticalVelocityFor)
                {
                    this.isOnGround = true;
                }
            }
            else
            {
                this.zeroVerticalVelocityFor = 0;
            }
        }

        public void HandleDeceleration(float elapsedTime)
        {
            //The user did not move the player character last loop, therefore we will make it decelerate a bit faster than the friction
            if (this.decelerate && this.isOnGround)
            {
                this.body.LinearVelocity = new Vector2(this.body.LinearVelocity.X * 0.9f, this.body.LinearVelocity.Y);
            }
            this.decelerate = true;
        }

        public override bool OnCollisionEnter(Fixture fixtureA, Fixture fixtureB, FarseerPhysics.Dynamics.Contacts.Contact contact)
        {
            if (!fixtureB.IsSensor)
            {
                Vector2 norm;
                FixedArray2<Vector2> pts;
                contact.GetWorldManifold(out norm, out pts);

                //If player is falling and the collision is downwards
                if (norm.Y < 0 && this.body.LinearVelocity.Y >= 0)
                {
                    this.isOnGround = true;
                }

                return true;
            }
            return true;
            
        }

        public override void OnCollisionExit(Fixture fixtureA, Fixture fixtureB)
        {
            //If it isn't a slope and the player is falling
            if (!fixtureB.Body.parent.identity.tag.Equals("SLOPE", StringComparison.OrdinalIgnoreCase)) 
            {
                if (this.body.LinearVelocity.Y > 0)
                {
                    this.isOnGround = false;
                }
            }
        }

        /***TEMP FOR TESTING***/
        public void Kill(int data)
        {
            bool dead = true;
        }

        /***TEMP FOR TESTING***/
        public void KillAgain(int data)
        {
            bool superDead = true;
        }
    }
}
