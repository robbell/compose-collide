using System;
using System.Configuration;
using System.Net;
using Bespoke.Common.Osc;
using System.Linq;

namespace ComposeCollide.Player
{
    public class ControlMessages
    {
        private volatile bool enabled;

        public void Initialise()
        {
            var server = new OscServer(TransportType.Udp, IPAddress.Loopback, Convert.ToInt32(ConfigurationManager.AppSettings["ListenOnPort"]));
            server.RegisterMethod("/");
            server.MessageReceived += MessageReceived;
            server.Start();
        }

        private void MessageReceived(object sender, OscMessageReceivedEventArgs e)
        {
            enabled = e.Message.Data.First().ToString().ToUpper() == "PLAY";
        }

        public bool IsPlaybackEnabled()
        {
            return enabled;
        }
    }
}