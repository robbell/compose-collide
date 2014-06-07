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
            var unplayed = db.ScoreDetails.Where(s => s.Played == null).OrderBy(s => s.Created);
            var collaborationsAvailable = unplayed.Count(s => s.IsCollaboration) >= 2;

            if (collaborationsAvailable)
            {
                var collaborations = unplayed.Where(s => s.IsCollaboration).Take(2);
                return Json(collaborations);
            }

            if (unplayed.Any(s => !s.IsCollaboration))
            {
                var solo = unplayed.FirstOrDefault(s => !s.IsCollaboration);
                return Json(solo);
            }

            return null;
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