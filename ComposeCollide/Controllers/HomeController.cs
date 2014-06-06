using System;
using System.Web.Mvc;
using ComposeCollide.Models;

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