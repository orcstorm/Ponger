using System;

namespace Pong
{
    public static class Program
    {
        [STAThread]
        static void Main()
        {
            using (var game = new PongGame())
                game.Run();
        }
    }
}
