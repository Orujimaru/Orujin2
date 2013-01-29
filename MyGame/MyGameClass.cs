using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MyGame.MyGameObjects;
using Orujin.Core.GameHelp;
using Orujin.Core.Renderer;
using Orujin.Framework;

namespace MyGame
{
    public class MyGameClass : OrujinGame
    {
        public MyGameClass()
            : base("MyGame", new Vector2(0, 9.82f))
        {
        }

        /***Add objects here***/
        public override void Start()
        {
            base.AddInputCommand("PlayerOne", "Jump", DynamicArray.ObjectArray(2.0f), Keys.Up, Buttons.A);
            base.AddInputCommand("PlayerOne", "MovePlayer", DynamicArray.ObjectArray(new Vector2(-10.0f, 0)), Keys.Left, Buttons.LeftThumbstickLeft);
            base.AddInputCommand("PlayerOne", "MovePlayer", DynamicArray.ObjectArray(new Vector2(10.0f, 0)), Keys.Right, Buttons.LeftThumbstickRight);

            base.AddInputCommand("Camera", "Scale", DynamicArray.ObjectArray(new Vector2(0.1f, 0.1f)), Keys.W, Buttons.X);
            base.AddInputCommand("Camera", "Scale", DynamicArray.ObjectArray(new Vector2(-0.1f, -0.1f)), Keys.S, Buttons.X);
            base.AddInputCommand("Camera", "MoveBy", DynamicArray.ObjectArray(new Vector2(-1280.0f, 0), 1000), Keys.A, Buttons.X);

            base.AddInputCommand("Camera", "CenterParentContiniously", DynamicArray.ObjectArray(500), Keys.V, Buttons.X);

            base.AddInputCommand("Camera", "SetPosition", DynamicArray.ObjectArray(new Vector2(100, 100)), Keys.B, Buttons.X);

            //Load level once the game and the events have been set up.
            this.LoadLevel("Content/Levels/TheMeadowPart1FixedGrass.xml", new MyObjectProcessor());

            this.CreateCameraEventsAndBorders(GameManager.importantObjectA.identity);   

            base.Start();
        }

        private void CreateCameraEventsAndBorders(Identity identity)
        {
            //Right side
            List<GameEventCommand> rightBorderEventCommands = new List<GameEventCommand>();
            rightBorderEventCommands.Add(new GameEventCommand("Game", null, "PauseLogic", DynamicArray.ObjectArray(1), 0));
            rightBorderEventCommands.Add(new GameEventCommand("Camera", null, "MoveBy", DynamicArray.ObjectArray(new Vector2(1280.0f, 0), 2000), 0));
            rightBorderEventCommands.Add(new GameEventCommand("Game", null, "ResumeLogic", DynamicArray.ObjectArray(1), 2000));
            GameEvent rightBorderEvent = new GameEvent(rightBorderEventCommands);

            Trigger rightCameraBorder = new Trigger(new Vector2(1330, 400), new Rectangle(0, 0, 50, 2000), "Right Camera Object", GameEventCondition.RightCameraBorderId, identity, this.world, 0, true);

            GameEventCondition rightCameraBorderCondition = new GameEventCondition(GameEventCondition.RightCameraBorderId, rightBorderEvent);
            base.AddEventCondition(rightCameraBorderCondition);

            //Left side
            List<GameEventCommand> leftBorderEventCommands = new List<GameEventCommand>();
            leftBorderEventCommands.Add(new GameEventCommand("Game", null, "PauseLogic", DynamicArray.ObjectArray(1), 0));
            leftBorderEventCommands.Add(new GameEventCommand("Camera", null, "MoveBy", DynamicArray.ObjectArray(new Vector2(-1280.0f, 0), 2000), 0));
            leftBorderEventCommands.Add(new GameEventCommand("Game", null, "ResumeLogic", DynamicArray.ObjectArray(1), 2000));
            GameEvent leftBorderEvent = new GameEvent(leftBorderEventCommands);

            Trigger leftCameraBorder = new Trigger(new Vector2(-50, 400), new Rectangle(0, 0, 50, 2000), "Left Camera Object", GameEventCondition.LeftCameraBorderId, identity, this.world, 0, true);

            GameEventCondition leftCameraBorderCondition = new GameEventCondition(GameEventCondition.LeftCameraBorderId, leftBorderEvent);
            base.AddEventCondition(leftCameraBorderCondition);

            //Unlock Horizontally
            GameEvent unlockCameraHorizontallyEvent = new GameEvent(new GameEventCommand("Camera", null, "UnlockHorizontally", DynamicArray.ObjectArray(50), 0));
            GameEventCondition unlockCameraHorizontallyCondition = new GameEventCondition(GameEventCondition.UnlockCameraHorizontallyId, unlockCameraHorizontallyEvent);
            base.AddEventCondition(unlockCameraHorizontallyCondition);

            //Lock Horizontally
            GameEvent lockCameraHorizontallyEvent = new GameEvent(new GameEventCommand("Camera", null, "LockHorizontally", DynamicArray.ObjectArray(1), 0));
            GameEventCondition lockCameraHorizontallyCondition = new GameEventCondition(GameEventCondition.LockCameraHorizontallyId, lockCameraHorizontallyEvent);
            base.AddEventCondition(lockCameraHorizontallyCondition);

            //Unlock Vertically
            GameEvent unlockCameraVerticallyEvent = new GameEvent(new GameEventCommand("Camera", null, "UnlockVertically", DynamicArray.ObjectArray(50), 0));
            GameEventCondition unlockCameraVerticallyCondition = new GameEventCondition(GameEventCondition.UnlockCameraVerticallyId, unlockCameraVerticallyEvent);
            base.AddEventCondition(unlockCameraVerticallyCondition);

            //Lock Vertically
            GameEvent lockCameraVerticallyEvent = new GameEvent(new GameEventCommand("Camera", null, "LockVertically", DynamicArray.ObjectArray(1), 0));
            GameEventCondition lockCameraVerticallyCondition = new GameEventCondition(GameEventCondition.LockCameraVerticallyId, lockCameraVerticallyEvent);
            base.AddEventCondition(lockCameraVerticallyCondition);

            //Test unlocker
            CameraObject co = new CameraObject(1280, 2, new Vector2(1640, 300), CameraObject.UnlockWhenPlayerMovesLeft, "TestCameraObject");
        }

        /***Add game logic here***/
        public override void Update(float elapsedTime)
        {
            base.Update(elapsedTime);
        }

        public static void Initialize()
        {
            GameManager.game = new MyGameClass();
        }
    }

}
