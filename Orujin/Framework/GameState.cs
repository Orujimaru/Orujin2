using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Orujin.Framework;

namespace Orujin.Core.Logic
{
    public class GameState
    {
        /***Unique identifier***/
        public string name { get; private set; }
        internal List<GameObject> gameObjects;

        public GameState(string name)
        {
            this.name = name;
            gameObjects = new List<GameObject>();
        }
    }
}
