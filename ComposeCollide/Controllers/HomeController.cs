using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using ComposeCollide.Models;
using ComposeCollide.Shared;
using WebGrease.Css.Extensions;

namespace ComposeCollide.Controllers
{
    public class HomeController : Controller
    {
        private readonly ComposeCollideContext db = new ComposeCollideContext();

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost, ActionName("Index")]
        public ActionResult Create(ScoreDetail scoreDetail)
        {
            scoreDetail.Created = DateTime.Now;

            if (ContainsBadWords(scoreDetail.Creator)) scoreDetail.Played = DateTime.Now;

            if (ModelState.IsValid)
            {
                db.ScoreDetails.Add(scoreDetail);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(scoreDetail);
        }

        public JsonResult GetNext()
        {
            var unplayed = db.ScoreDetails.Where(s => s.Played == null).OrderBy(s => s.Created);
            var collaborationsAvailable = unplayed.Count(s => s.IsCollaboration) >= 2;

            if (collaborationsAvailable)
            {
                var collaborations = unplayed.Where(s => s.IsCollaboration).Take(2);
                var result = Json(CombineCollaborations(collaborations.ToList()), JsonRequestBehavior.AllowGet);
                MarkAsPlayed(collaborations);
                return result;
            }

            if (unplayed.Any(s => !s.IsCollaboration))
            {
                var solo = unplayed.FirstOrDefault(s => !s.IsCollaboration);
                MarkAsPlayed(new[] { solo });
                return Json(solo, JsonRequestBehavior.AllowGet);
            }

            return null;
        }

        private ScoreDetail CombineCollaborations(IList<ScoreDetail> collaborations)
        {
            var first = collaborations.First();
            var second = collaborations.Last();

            return new ScoreDetail
            {
                Creator = first.Creator.Trim() + " & " + second.Creator.Trim(),
                ScoreInfo = first.ScoreInfo.Substring(0, 68) + second.ScoreInfo.Substring(0, 68)
            };
        }

        private void MarkAsPlayed(IEnumerable<ScoreDetail> scores)
        {
            scores.ForEach(s => s.Played = DateTime.Now);
            db.SaveChanges();
        }

        private bool ContainsBadWords(string name)
        {
            var dataFile = Server.MapPath("~/App_Data/swearwords.txt");

            if (!System.IO.File.Exists(dataFile)) return false;
            
            var swearWords = System.IO.File.ReadAllLines(dataFile);

            foreach (var part in name.Split(' '))
            {
                var rgx = new Regex("[^a-zA-Z0-9 -]");
                var cleanPart = rgx.Replace(part, "");

                if (swearWords.Contains(cleanPart.ToLower())) return true;
            }

            return false;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}