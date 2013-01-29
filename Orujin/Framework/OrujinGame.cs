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
        public string name;
        
        public OrujinGame(string name, Vector2 gravity)
        {
            this.world = new World(gravity);
            this.name = name;
        }

        internal void Initialize(Orujin o)
        {
            orujin = o;
        }

        public virtual void Start()
        {
        }

        public virtual void Update(float elapsedTime)
        {
        }

        /*Attempts to add the GameObject to the game and returns true if it was successful and there are no duplicates*/
        public bool AddObject(GameObject gameObject)
        {
            return orujin.gameObjectManager.Add(gameObject);
        } 

        /*Attempts to remove the GameObject from the game and returns true if it was successful, returns false if the GameObject wasn't found*/
        public bool RemoveObject(GameObject gameObject)
        {
            return orujin.gameObjectManager.Remove(gameObject);
        }

        /*Attempts to find a GameObject with the specific name, returns null if no GameObject with the name was found*/
        public GameObject FindObjectWithName(string name)
        {
            return orujin.gameObjectManager.GetByName(name);
        }

        internal CameraManager GetCameraManager()
        {
            return orujin.cameraManager;
        }

        /*Attempts to find one or more GameObjects with a specific tag, returns an empty list if no GameObjects with the tag were found.*/
        public List<GameObject> FindObjectsWithTag(string tag)
        {
            return orujin.gameObjectManager.GetByTag(tag);
        }

        /*Adds a custom input command to the game*/
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
