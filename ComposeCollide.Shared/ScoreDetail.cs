using System;
using System.ComponentModel.DataAnnotations;

namespace ComposeCollide.Shared
{
    public class ScoreDetail
    {
        [Key]
        public int Id { get; set; }
        public string Creator { get; set; }
        public DateTime Created { get; set; }
        public DateTime? Played { get; set; }
        public bool IsCollaboration { get; set; }
        public int[][] Score { get; set; }

        public string ScoreInfo
        {
            get
            {
                var output = string.Empty;

                foreach (var track in Score)
                {
                    foreach (var beat in track)
                    {
                        output += beat;
                    }

                    output += ",";
                }

                return output;
            }
            set
            {
                if (value == null) return;

                var trackCount = 0;
                var beatCount = 0;

                foreach (var beat in value)
                {
                    if (beat == ',')
                    {
                        beatCount = 0;
                        trackCount++;
                        continue;
                    }

                    Score[trackCount][beatCount] = (int)char.GetNumericValue(beat);
                    beatCount++;
                }
            }
        }

        public ScoreDetail()
        {
            Score = new int[8][];

            for (var trackCount = 0; trackCount < Score.Length; trackCount++)
            {
                Score[trackCount] = new int[16];
            }
        }
    }
}
