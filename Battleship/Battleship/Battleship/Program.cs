using System;

namespace Battleship
{
#if WINDOWS || XBOX
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            using (BattleShipGame game = new BattleShipGame())
            {
                game.Run();
            }
        }
    }
#endif
}

