using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FarseerPhysics.Collision.Shapes;
using FarseerPhysics.Common;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using Microsoft.Xna.Framework;
using Orujin.Core.Renderer;
using Orujin.Framework;

namespace MyGame.MyGameObjects
{
    public class Platform : GameObject
    {
        public Platform(float width, float height, Vector2 position, string name)
            : base(position, name, "Platform")
        {
            Body body = BodyFactory.CreateRectangle(GameManager.game.world, width / Camera.PixelsPerMeter, height / Camera.PixelsPerMeter, 1f, position / Camera.PixelsPerMeter);
            body.IsStatic = true;
            body.Restitution = 0f;
            body.Friction = 0.5f;
            this.AddBody(body);
            GameManager.game.AddObject(this);
        }

        public Platform(float width, float height, Vector2 position, string name, bool slopeDirection)
            : base(position, name, "Slope")
        {            
            Vector2 bottomLeft = new Vector2(-width/2, height/2) / Camera.PixelsPerMeter;
            Vector2 bottomRight = new Vector2(width / 2, height / 2) / Camera.PixelsPerMeter;
            Vector2 top = Vector2.Zero;

            if (slopeDirection) //SLOPE //Remove top left vertex
            {
                top = new Vector2(width / 2, -height / 2) / Camera.PixelsPerMeter;
            }
            else //INVERSLOPE //Remove top right vertex
            {
                top = new Vector2(-width / 2, -height / 2) / Camera.PixelsPerMeter;
            }

            Vertices vertices = new Vertices(3);
            vertices.Add(top);
            vertices.Add(bottomRight);
            vertices.Add(bottomLeft);           
            

            Body body = BodyFactory.CreatePolygon(GameManager.game.world, vertices, 1f, position/ Camera.PixelsPerMeter);
            body.IsStatic = true;
            body.Restitution = 0f;
            body.Friction = 0.5f;
            this.AddBody(body);
        }
    }
}
