using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Orujin.Core.Logic;
using Orujin.Core.Renderer;
using Orujin.Pipeline;

namespace Orujin.Framework
{
    public class OrujinGame
    {
        internal static Orujin orujin;
        public World world {get; internal set;}
        public string activeState {get; private set;}
        public string backgroundState {get; private set;}
        public bool runBackgroundState {get; private set;}
        
        public string name;
        
        public OrujinGame(string name, Vector2 gravity)
        {
            this.world = new World(gravity);
            this.name = name;
            this.backgroundState = null;
            this.runBackgroundState = false;
        }

        internal void Initialize(Orujin o)
        {
            orujin = o;
            if(activeState == null)
            {
                activeState = "Default"
                orujin.AddGameState("Default");
            }
        }

        public virtual void Start()
        {
        }

        public virtual void Update(float elapsedTime)
        {
        }

        public void SetActiveState(string name)
        {
            if(this.backgroundState.Equals(name, StringComarision.OrdinalIgnoreCase))
            {
                this.backgroundState = null;
                this.runBackgroundState = false;
            }
            this.activeState = name;
        }
        
        public bool SetBackgroundState(string name, bool run)
        {
            if(this.activeState.Equals(name, StringComparison.OrdinalIgnoreCase))
            {
                //Can't set the active state to background state unless a new active state is given
                return false;
            }
            this.backgroundState = name;
            this.runBackgroundState = run;
        }
        
        /*Attempts to add the GameObject to the game and returns true if it was successful and there are no duplicates*/
        public bool AddObject(GameObject gameObject)
        {
            return orujin.gameObjectManager.Add(gameObject, this.activeState);
        } 
        
        public bool AddObject(GameObject gameObject, string gameState)
        {
            return orujin.gameObjectManager.Add(gameObject, gameState);
        } 

        /*Attempts to remove the GameObject from the game and returns true if it was successful, returns false if the GameObject wasn't found*/
        public bool RemoveObject(GameObject gameObject)
        {
            return orujin.gameObjectManager.Remove(gameObject, this.activeState);
        }
        
        public bool RemoveObject(GameObject gameObject, string gameState)
        {
            return orujin.gameObjectManager.Remove(gameObject, gameState);
        }

        /*Attempts to find a GameObject with the specific name, returns null if no GameObject with the name was found*/
        public GameObject FindObjectWithName(string name)
        {
            return orujin.gameObjectManager.GetByName(name, this.activeState);
        }
        
        public GameObject FindObjectWithName(string name, string gameState)
        {
            return orujin.gameObjectManager.GetByName(name, gameState);
        }

        internal CameraManager GetCameraManager()
        {
            return orujin.cameraManager;
        }

        /*Attempts to find one or more GameObjects with a specific tag, returns an empty list if no GameObjects with the tag were found.*/
        public List<GameObject> FindObjectsWithTag(string tag)
        {
            return orujin.gameObjectManager.GetByTag(tag, this.activeState);
        }
        
        public List<GameObject> FindObjectsWithTag(string tag, string gameState)
        {
            return orujin.gameObjectManager.GetByTag(tag, gameState);
        }

        /*Adds a custom input command to the game. THIS SHOULD INCLUDE GAME STATE*/
        public void AddInputCommand(string objectName, string methodName, object[] parameters, Keys key, Buttons button)
        {
            orujin.inputManager.AddCommand(objectName, methodName, parameters, key, button);
        }

        public void AddEventCondition(GameEventCondition condition)
        {
            orujin.conditionManager.AddCondition(condition);
        }

        public void RemoveEventCondition(int id)
        {
            orujin.conditionManager.RemoveCondition(id);
        }

        public static Texture2D GetTexture2DByName(String name)
        {
            return orujin.GetTexture2DByName(name);
        }

        public static SpriteAnimation GetSpriteAnimationByName(String name)
        {
            return orujin.GetSpriteAnimationByName(name);
        }

        public static ModularAnimation GetModularAnimationByName(String name)
        {
            return orujin.GetModularAnimationByName(name);
        }

        protected void LoadLevel(string fileName, ObjectProcessor op)
        {
            orujin.LoadLevel(fileName, op);
        }

        public void RemotelyTriggerEvent(int i)
        {
            orujin.conditionManager.RemotelyFulfillCondition(i);
        }

        public void PauseLogic(int data)
        {
            orujin.updateLogic = false;
        }

        public void ResumeLogic(int data)
        {
            orujin.updateLogic = true;
        }
    }
}
