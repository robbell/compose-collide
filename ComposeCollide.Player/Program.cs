using System;

namespace ComposeCollide.Player
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var controlMessages = new ControlMessages(new ScoreQueue(), new Playback());
        }

        private void ProcessLoop()
        {
            while (true)
            {
                try
                {
                    // poll queue
                    // playback
                }
                catch (Exception ex)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write(ex);
                    Console.ResetColor();
                }
            }
        }
    }
}
