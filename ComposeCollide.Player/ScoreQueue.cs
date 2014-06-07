using System.IO;
using System.Net;
using ComposeCollide.Shared;
using Newtonsoft.Json;

namespace ComposeCollide.Player
{
    public class ScoreQueue
    {
        public ScoreDetail GetNextScoreToPlay()
        {
            var request = WebRequest.Create("http://composecollide.co.uk/Home/GetNext");

            using (var response = request.GetResponse())
            using (var stream = response.GetResponseStream())
            using (var reader = new StreamReader(stream))
            {
                return JsonConvert.DeserializeObject<ScoreDetail>(reader.ReadToEnd());
            }
        }
    }
}