#region Using Statements
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.GamerServices;
using Orujin.Core.Renderer;
using Orujin.Framework;
using Orujin.Core.Logic;
using Orujin.Core.Input;
using System.Reflection;
using Orujin.Core.GameHelp;
using Orujin.Debug;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using Orujin.Pipeline;
using Orujin.Framework;
#endregion

namespace Orujin
{
    public class Orujin : Game
    {
        private GraphicsDeviceManager graphics;
        internal RendererManager rendererManager { get; private set; }
        internal GameObjectManager gameObjectManager { get; private set; }
        internal GameEventConditionManager conditionManager { get; private set; }
        internal InputManager inputManager { get; private set; }
        internal CameraManager cameraManager { get; private set; }
        internal DebugManager debugManager { get; private set; }
        internal bool updateLogic = true;

        public Orujin()
            : base()
        {
            graphics = new GraphicsDeviceManager(this);      
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            this.rendererManager = new RendererManager(ref this.graphics, 1280, 720);
            this.rendererManager.SetAmbientLightIntensity(this.graphics, 1f);
            this.gameObjectManager = new GameObjectManager();
            this.conditionManager = new GameEventConditionManager();
            this.inputManager = new InputManager();
            this.cameraManager = new CameraManager(1280, 720);
            this.debugManager = new DebugManager();            
            base.Initialize();
        }

        protected override void LoadContent()
        {     
            this.debugManager.InitiateFarseerDebugView(this.graphics.GraphicsDevice, this.Content, GameManager.game.world);
            GameManager.game.Initialize(this);
            GameManager.game.Start();

            //TEMP ADD CONDITION AND TRIGGER
            //Identity playerIdentity = new Identity();
            //playerIdentity.name = "PlayerOne";
            //playerIdentity.tag = "Player";
            //Trigger t = new Trigger(new Vector2(600, 500), new Rectangle(0, 0, 100, 100), "TempTrigger", 1, playerIdentity, GameManager.game.world);
        }

        protected override void UnloadContent()
        {
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            float elapsedTime = gameTime.ElapsedGameTime.Milliseconds;
            
            this.cameraManager.Update(elapsedTime);
    
            List<InputCommand> input = this.inputManager.Update(elapsedTime);
            
            
            this.conditionManager.Update(elapsedTime, this.gameObjectManager.GetGameObjects());

            if (this.updateLogic)
            {
                this.gameObjectManager.Update(elapsedTime, input);
                GameManager.game.world.Step((float)gameTime.ElapsedGameTime.TotalMilliseconds * 0.001f);
            }
            
            base.Update(gameTime);
        }


        protected override void Draw(GameTime gameTime)
        {
            this.rendererManager.Begin();

            this.rendererManager.Render(this.gameObjectManager);

            this.rendererManager.End(ref this.graphics);

            this.debugManager.RenderFarseerDebugView(this.graphics);

            base.Draw(gameTime);
        }

        public Texture2D GetTexture2DByName(String name)
        {
            return Content.Load<Texture2D>(name);
        }

        public SpriteAnimation GetSpriteAnimationByName(String name)
        {
            return SpriteAnimationLoader.Load(name);
        }

        public ModularAnimation GetModularAnimationByName(String name)
        {
            return ModularAnimationLoader.Load(name);
        }

        public void LoadLevel(string fileName, ObjectProcessor op)
        {
            LevelLoader.FromFile(fileName, this.Content, op);
        }
    }
}
