using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Orujin.Framework
{
    public class Menu
    {
        public string name { get; private set; }

        public Menu(string name)
        {
            this.name = name;
        }

        public int Run(float elapsedTime)
        {
            return 0;
        }

    }
}
