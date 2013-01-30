using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Orujin.Core.Physics;
using Orujin.Core.GameHelp;
using Orujin.Core.Input;
using Orujin.Core.Renderer;
using Orujin.Framework;

namespace Orujin.Core.Logic
{
    public class GameObjectManager
    {
        private struct GarbageObject
        {
            public string nameOfState;
            public GameObject gameObject;
        }

        private List<GameState> gameStates = new List<GameState>();
        private List<GarbageObject> garbageList = new List<GarbageObject>();

        public GameObjectManager()
        {

        }

        public void Update(float elapsedTime, List<InputCommand> inputCommands, string nameOfState)
        {
            foreach (GameState gs in this.gameStates)
            {
                if(gs.name.Equals(nameOfState, StringComparison.OrdinalIgnoreCase))
                {
                    for (int x = 0; x < gs.gameObjects.Count(); x++)
                    {
                        if (gs.gameObjects[x].checkForInput)
                        {
                            this.CheckForInput(gs.gameObjects[x], inputCommands);
                        }

                        gs.gameObjects[x].Update(elapsedTime);

                        if (gs.gameObjects[x].checkForPixelCollision)
                        {
                            //Check all objects infront
                            for (int y = x + 1; y < gs.gameObjects.Count(); y++)
                            {
                                if (gs.gameObjects[y].checkForPixelCollision)
                                {
                                    this.CheckForPixelCollision(gs.gameObjects[x], gs.gameObjects[y]);
                                }
                            }
                        }
                    }
                }
                //Remove everything that is in the garbageList
                foreach (GarbageObject garbage in this.garbageList)
                {
                    if(garbage.nameOfState.Equals(gs.name, StringComparison.OrdinalIgnoreCase))
                    {
                        gs.gameObjects.Remove(garbage.gameObject);
                    }
                }
                this.garbageList.Clear();
            }
        }

        public void AddGameState(string name)
        {
            this.gameStates.Add(new GameState(name));
        }

        public void AddGameState(string name, List<GameObject> gameObjects)
        {
            GameState temp = new GameState(name);
            temp.gameObjects = gameObjects;
            this.gameStates.Add(temp);
        }

        private void CheckForPixelCollision(GameObject objectA, GameObject objectB)
        {
            List<Sprite> objectAComponents = objectA.rendererComponents.GetChildren();
            List<Sprite> objectBComponents = objectB.rendererComponents.GetChildren();

            for (int aX = 0; aX < objectAComponents.Count(); aX++)
            {
                for (int bX = 0; bX < objectBComponents.Count(); bX++)
                {
                    if (PixelCollisionManager.Intersects(objectAComponents[aX], objectBComponents[bX]))
                    {
                        bool hej = true;
                    }
                }
            }
        }

        private void CheckForInput(GameObject gameObject, List<InputCommand> inputCommands)
        {
            foreach (InputCommand ic in inputCommands)
            {
                if (ic.isDown)
                {
                    if (gameObject.identity.name.Equals(ic.objectName))
                    {
                        MethodInfo method = gameObject.GetType().GetMethod(ic.methodName);
                        if (ic.thumbstick)
                        {
                            method.Invoke(gameObject, DynamicArray.ObjectArray(ic.parameters, ic.magnitude));
                        }
                        else
                        {
                            method.Invoke(gameObject, ic.parameters);
                        }
                    }
                    else if (ic.objectName.Equals("Camera"))
                    {
                        MethodInfo method = GameManager.game.GetCameraManager().GetType().GetMethod(ic.methodName);
                        method.Invoke(GameManager.game.GetCameraManager(), ic.parameters);
                    }
                    else if (ic.objectName.Equals("Game"))
                    {
                        MethodInfo method = GameManager.game.GetType().GetMethod(ic.methodName);
                        method.Invoke(GameManager.game, ic.parameters);
                    }
                }
            }
        }

        public GameObject GetByName(string name, string nameOfState)
        {
            foreach (GameState gs in this.gameStates)
            {
                if (gs.name.Equals(nameOfState, StringComparison.OrdinalIgnoreCase))
                {
                    foreach (GameObject go in gs.gameObjects)
                    {
                        if (go.identity.name.Equals(name))
                        {
                            return go;
                        }
                    }
                }
            }
            return null;
        }

        public List<GameObject> GetByTag(string tag, string nameOfState)
        {
            foreach (GameState gs in this.gameStates)
            {
                if (gs.name.Equals(nameOfState, StringComparison.OrdinalIgnoreCase))
                {
                    List<GameObject> tempObjects = new List<GameObject>();
                    foreach (GameObject go in gs.gameObjects)
                    {
                        if (go.identity.tag.Equals(tag))
                        {
                            tempObjects.Add(go);
                        }
                    }
                    return tempObjects;
                }
            }
            return new List<GameObject>();         
        }


        public bool Add(GameObject newObject, string nameOfState)
        {
            foreach (GameState gs in this.gameStates)
            {
                if(gs.name.Equals(nameOfState, StringComparison.OrdinalIgnoreCase))
                {
                    if (gs.gameObjects.Contains(newObject))
                    {
                        return false;
                    }

                    gs.gameObjects.Add(newObject);
                    return true;
                }
            }
           return false;
        }

        public bool Remove(GameObject removeObject, string nameOfState)
        {
            foreach (GameState gs in this.gameStates)
            {
                if(gs.name.Equals(nameOfState, StringComparison.OrdinalIgnoreCase))
                {
                    if (gs.gameObjects.Contains(removeObject))
                    {
                        GarbageObject go = new GarbageObject();
                        go.nameOfState = nameOfState;
                        go.gameObject = removeObject;
                        this.garbageList.Add(go);
                        return true;
                    }
                }
            }

            return false;
        }

        public List<GameObject> GetGameObjects(string nameOfState)
        {
            foreach (GameState gs in this.gameStates)
            {
                if(gs.name.Equals(nameOfState, StringComparison.OrdinalIgnoreCase))
                {
                    return gs.gameObjects;
                }
            }
            return new List<GameObject>(); 
        }

    }
}
