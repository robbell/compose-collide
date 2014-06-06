using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using ComposeCollide.Models;

namespace ComposeCollide.Controllers
{
    public class ScoreDetailsController : Controller
    {
        private ComposeCollideContext db = new ComposeCollideContext();

        // GET: ScoreDetails
        public ActionResult Index()
        {
            return View(db.ScoreDetails.ToList());
        }

        // GET: ScoreDetails/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ScoreDetail scoreDetail = db.ScoreDetails.Find(id);
            if (scoreDetail == null)
            {
                return HttpNotFound();
            }
            return View(scoreDetail);
        }

        // GET: ScoreDetails/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ScoreDetails/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Creator,Created,Played,IsCollaboration,ScoreInfo")] ScoreDetail scoreDetail)
        {
            if (ModelState.IsValid)
            {
                db.ScoreDetails.Add(scoreDetail);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(scoreDetail);
        }

        // GET: ScoreDetails/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ScoreDetail scoreDetail = db.ScoreDetails.Find(id);
            if (scoreDetail == null)
            {
                return HttpNotFound();
            }
            return View(scoreDetail);
        }

        // POST: ScoreDetails/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Creator,Created,Played,IsCollaboration")] ScoreDetail scoreDetail)
        {
            if (ModelState.IsValid)
            {
                db.Entry(scoreDetail).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(scoreDetail);
        }

        // GET: ScoreDetails/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ScoreDetail scoreDetail = db.ScoreDetails.Find(id);
            if (scoreDetail == null)
            {
                return HttpNotFound();
            }
            return View(scoreDetail);
        }

        // POST: ScoreDetails/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ScoreDetail scoreDetail = db.ScoreDetails.Find(id);
            db.ScoreDetails.Remove(scoreDetail);
            db.SaveChanges();
            return RedirectToAction("Index");
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
