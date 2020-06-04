using System;

namespace Asteroids
{
#if WINDOWS || LINUX
    /// <summary>
    /// The main class.
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            using (GameEngine game = new GameEngine())
                game.Run();
        }
    }
#endif
}
