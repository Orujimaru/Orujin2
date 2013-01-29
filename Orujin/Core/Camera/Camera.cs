using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Orujin.Framework;

namespace Orujin.Core.Renderer
{
    public class Camera
    {
        public static GameObject parent { get; private set; }
        public static bool followParentHorizontally { get; internal set; }
        public static bool followParentVertically { get; internal set; }

        public static Nullable<Vector2> destination { get; internal set; }
        public static float duration { get; internal set; }

        public const float PixelsPerMeter = 64.0f;

        private static Vector2 position;
        public static Vector2 adjustedPosition
        {
            get
            {
                return -position;
            }
            private set
            {
                return;
            }
        }
        public static Vector2 screenCenter { get; private set; }
        public static Vector2 scale { get; private set; }

        public static Matrix matrix { 
            get 
            {
                return Matrix.CreateTranslation(new Vector3(position - screenCenter, 0f))
                    * Matrix.CreateScale(new Vector3(scale, 1)) 
                    * Matrix.CreateTranslation(new Vector3(screenCenter, 0f)) ;
            }
            private set { return; }
        }

        public static Matrix farseerMatrix
        {
            get
            {
                return Matrix.CreateTranslation(new Vector3((position - screenCenter) / PixelsPerMeter, 0f))
                    * Matrix.CreateScale(new Vector3(scale, 1))
                    * Matrix.CreateTranslation(new Vector3(screenCenter / PixelsPerMeter, 0f));
            }
            private set { return; }
        }

        internal static void Initialize(float frameWidth, float frameHeight)
        {
            screenCenter = new Vector2(frameWidth / 2, frameHeight / 2);
            position = Vector2.Zero;
            scale = new Vector2(1, 1);       
        }

        internal static void Move(Vector2 moveVector)
        {
            position -= moveVector;
        }

        internal static void SetPosition(Vector2 newPosition)
        {
            position = -newPosition;
        }

        internal static void Scale(Vector2 scaleVector)
        {
            scale += scaleVector;
            if (scale.X < 0.01f)
            {
                scale = new Vector2(0.01f, scale.Y);
            }
            if (scale.Y < 0.01f)
            {
                scale = new Vector2(scale.X, 0.01f);
            }
        }

        public static void SetParent(GameObject newParent)
        {
            parent = newParent;
        }

        internal static void MoveTo(Vector2 newPosition, float newDuration)
        {
            if (screenCenter != newPosition)
            {
                destination = newPosition;
                duration = newDuration;
            }
        }

        internal static void MoveBy(Vector2 newPosition, float newDuration)
        {
            newPosition = adjustedPosition + screenCenter + newPosition;
            if (screenCenter != (newPosition - screenCenter))
            {
                destination = newPosition;
                duration = newDuration;
            }
        }
    }
}
