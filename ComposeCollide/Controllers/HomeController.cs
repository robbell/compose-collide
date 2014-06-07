using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using ComposeCollide.Models;
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
            var unplayed = db.ScoreDetails.Where(s => s.Played == null).OrderBy(s => s.Created).ToList();
            var collaborationsAvailable = unplayed.Count(s => s.IsCollaboration) >= 2;

            if (collaborationsAvailable)
            {
                var collaborations = unplayed.Where(s => s.IsCollaboration).Take(2);
                var result = Json(CombineCollaborations(collaborations), JsonRequestBehavior.AllowGet);
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
                ScoreInfo = first.ScoreInfo.Substring(0, 68) + second.ScoreInfo.Substring(68, 68)
            };
        }

        private void MarkAsPlayed(IEnumerable<ScoreDetail> scores)
        {
            scores.ForEach(s => s.Played = DateTime.Now);
            db.SaveChanges();
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