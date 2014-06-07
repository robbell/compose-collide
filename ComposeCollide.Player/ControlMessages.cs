using System;
using System.Configuration;
using System.Net;
using Bespoke.Common.Osc;
using System.Linq;
using ComposeCollide.Shared;

namespace ComposeCollide.Player
{
    public class ControlMessages
    {
        private readonly Playback playback;
        private readonly ScoreQueue scoreQueue;

        public ControlMessages(ScoreQueue scoreQueue, Playback playback)
        {
            this.scoreQueue = scoreQueue;
            this.playback = playback;
        }

        public void Initialise()
        {
            var server = new OscServer(TransportType.Udp, IPAddress.Loopback, Convert.ToInt32(ConfigurationManager.AppSettings["ListenOnPort"]));
            server.RegisterMethod("/");
            server.MessageReceived += MessageReceived;
            server.Start();
        }

        private void MessageReceived(object sender, OscMessageReceivedEventArgs e)
        {
            var shouldPlay = e.Message.Data.First().ToString().ToUpper() == "PLAY";

            if (shouldPlay)
            {
                var score = scoreQueue.GetNextScoreToPlay();
                if (score != null) playback.Play(score, Convert.ToInt32(e.Message.Data.Last()));
            }
        }
    }

    public class Playback
    {
        public void Play(ScoreDetail score, int toInt32)
        {
            throw new NotImplementedException();
        }
    }
}