using System;
using System.Configuration;
using System.Threading;

namespace ComposeCollide.Player
{
    public class Program
    {
        private static ScoreQueue queue;
        private static Playback playback;
        private static ControlMessages controlMessages;

        public static void Main(string[] args)
        {
            queue = new ScoreQueue();
            playback = new Playback();
            controlMessages = new ControlMessages();

            controlMessages.Initialise();
            PerformPlayback();
        }

        private static void PerformPlayback()
        {
            while (true)
            {
                try
                {
                    if (controlMessages.IsPlaybackEnabled())
                    {
                        var score = queue.GetNextScoreToPlay();
                        if (score != null) playback.Play(score);
                    }

                    Thread.Sleep(Convert.ToInt32(ConfigurationManager.AppSettings["TimeBetweenScores"]) * 1000);
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
