﻿using System;
using System.Configuration;
using System.Diagnostics;
using System.Net;
using Bespoke.Common.Osc;
using ComposeCollide.Shared;

namespace ComposeCollide.Player
{
    public class Playback
    {
        private const int numberOfTracks = 8;
        private const int numberOfBeats = 16;
        private int[] positions;
        private readonly IPEndPoint endPoint;
        private int millisecondsBetweenBeats;

        public Playback()
        {
            endPoint = new IPEndPoint(IPAddress.Loopback, Convert.ToInt32(ConfigurationManager.AppSettings["ReceiverPort"]));
        }

        public void Play(ScoreDetail scoreDetail)
        {
            positions = new int[numberOfTracks];

            SetTimeBetweenFrames(250);

            PlayScoreStart(scoreDetail.Creator);

            PlayScore(scoreDetail);

            PlayScoreEnd();
        }

        private void PlayScore(ScoreDetail scoreDetail)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            for (var beatCount = 0; beatCount < Convert.ToInt32(ConfigurationManager.AppSettings["BarsToPlay"]) * 4; beatCount++)
            {
                Wait(stopwatch, millisecondsBetweenBeats);

                PlayNextBeat(scoreDetail);

                stopwatch.Restart();
            }
        }

        private void PlayNextBeat(ScoreDetail scoreDetail)
        {
            var score = scoreDetail.Score;

            SendMessage(m =>
                {
                    for (var trackCount = 0; trackCount < numberOfTracks; trackCount++)
                    {
                        if (score[trackCount][positions[trackCount]] == 2)
                        {
                            positions[trackCount] = 0;
                        }

                        if (score[trackCount][positions[trackCount]] == 1) m.Append(trackCount + 1);

                        positions[trackCount] = positions[trackCount] == numberOfBeats - 1 ? 0 : positions[trackCount] + 1;
                    }
                });
        }

        private void SetTimeBetweenFrames(int tempo)
        {
            millisecondsBetweenBeats = (int)((60d / tempo) * 1000);
        }

        private void Wait(Stopwatch stopwatch, int millisecondsToWait)
        {
            while (stopwatch.ElapsedMilliseconds < millisecondsToWait) { }
        }

        public void PlayScoreStart(string creator)
        {
            SendMessage(m => m.Append(string.Format("BOM {0}", creator)));
        }

        public void PlayScoreEnd()
        {
            SendMessage(m => m.Append("EOM"));
        }

        private void SendMessage(Action<OscMessage> appendAction)
        {
            var message = new OscMessage(endPoint, "/");

            appendAction(message);

            message.Send(endPoint);
        }
    }
}