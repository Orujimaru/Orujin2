#region Using Statements
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Orujin;
using Orujin.Framework;
#endregion

namespace MyGame
{
    /// <summary>
    /// The main class.
    /// </summary>
    public static class Program
    {
        public static Orujin.Orujin orujin;
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            MyGameClass.Initialize();          
            orujin = new Orujin.Orujin();
            orujin.Run();                   
        }
    }
}
