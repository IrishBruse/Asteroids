namespace Asteroids
{
    public static class Program
    {
        static void Main()
        {
            using (GameEngine game = new GameEngine())
                game.Run();
        }
    }
}
