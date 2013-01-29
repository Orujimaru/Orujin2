using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyGame.MyGameObjects;
using Orujin.Framework;
using Orujin.Pipeline;

namespace MyGame
{
    public class MyObjectProcessor : ObjectProcessor
    {
        public override void ProcessTextureObject(ObjectInformation oi)
        {
            //It is not a valid ObjectInformation
            if (oi.texture == null)
            {
                return;
            }
            switch (oi.name.ToUpper())
            {
                case "CAMERAOBJECT":
                    this.CreateCameraObject(oi);
                    return;
            }
           
           //If we have come this far we will create a tile with the texture
           Tile t = new Tile(oi.texture, oi.position, oi.scale, oi.rotation,oi.flipH, oi.flipV, oi.scrollSpeed, oi.name, oi.name);
        }

        public override void ProcessPrimitiveObject(ObjectInformation oi)
        {
            if (oi.customProperties.Count == 0) //No custom properties means a regular platform
            {
                Platform tempPlatform = new Platform(oi.width, oi.height, oi.position, oi.name);
                return;
            }
            else
            {
                switch (oi.customProperties[0].ToUpper())
                {
                    case "SLOPE":
                        new Platform(oi.width, oi.height, oi.position, oi.name, true);
                        return;

                    case "INVERSLOPE":
                        new Platform(oi.width, oi.height, oi.position, oi.name, false);
                        return;

                    case "PLAYER":
                        new Player(oi.position, "PlayerOne");
                        break;
                }
            }
        }

        private void CreateCameraObject(ObjectInformation oi)
        {
            bool unlockDirection = CameraObject.UnlockWhenPlayerMovesLeft;
            if (oi.customProperties[0].Equals("TRUE", StringComparison.OrdinalIgnoreCase))
            {
                unlockDirection = CameraObject.UnlockWhenPlayerMovesRight;
            }
            new CameraObject(2000, 2, oi.position, unlockDirection, oi.name);
        }
    }
}
