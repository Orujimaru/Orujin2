using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using Microsoft.Xna.Framework;
using Orujin.Core.Renderer;
using Orujin.Framework;

namespace MyGame.MyGameObjects
{
    public class CameraObject : GameObject
    {
        public const bool UnlockWhenPlayerMovesLeft = true;
        public const bool UnlockWhenPlayerMovesRight = false;
        private bool unlockDirection;
        private float width;
        private float height;

        public CameraObject(float height, float width, Vector2 position, bool direction, string name)
            : base(position, name, "CameraObject")
        {
            this.width = width;
            this.height = height;
            this.unlockDirection = direction;
            Body body = BodyFactory.CreateRectangle(GameManager.game.world, width / Camera.PixelsPerMeter, height / Camera.PixelsPerMeter, 1f, position / Camera.PixelsPerMeter);
            body.IsStatic = true;
            body.IsSensor = true;
            this.AddBody(body);
            GameManager.game.AddObject(this);
        }

        public override void OnCollisionExit(Fixture fixtureA, Fixture fixtureB)
        {
            //Is the CameraObject for the X or Y axis?
            if (this.height > this.width) // It is for the X axis
            {
                //Check which side the player is leaving
                if (fixtureB.Body.Position.X < fixtureA.Body.Position.X && fixtureB.Body.LinearVelocity.X < 0) //Player is moving to the left
                {
                    if (this.unlockDirection == UnlockWhenPlayerMovesLeft)
                    {
                        this.UnlockCameraHorizontally();
                    }
                    else
                    {
                        this.LockCameraHorizontally();
                    }
                }
                else if (fixtureB.Body.Position.X > fixtureA.Body.Position.X && fixtureB.Body.LinearVelocity.X > 0) //Player is moving to the right
                {
                    if (this.unlockDirection == UnlockWhenPlayerMovesRight)
                    {
                        this.UnlockCameraHorizontally();
                    }
                    else
                    {
                        this.LockCameraHorizontally();
                    }
                }
            }
            else //It is for the Y axis
            {
            }
        }

        private void UnlockCameraHorizontally()
        {
            GameManager.game.RemotelyTriggerEvent(GameEventCondition.UnlockCameraHorizontallyId);
        }

        private void LockCameraHorizontally()
        {
            GameManager.game.RemotelyTriggerEvent(GameEventCondition.LockCameraHorizontallyId);
        }

        private void UnlockCameraVertically()
        {
        }

        private void LockCameraVertically()
        {
        }
    }
}
