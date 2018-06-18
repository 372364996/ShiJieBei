using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ShiJieBeiComponents.Domains;
using ShiJieBeiComponents.Repositories.EF;

namespace ShiJieBei.Areas.Mgr.Controllers
{
    public class GamesController : MgrController
    {
        private ShiJieBeiDbContext db = new ShiJieBeiDbContext();

        // GET: Mgr/Games
        public ActionResult Index()
        {
            return View(db.Games.ToList());
        }

        // GET: Mgr/Games/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Games games = db.Games.Find(id);
            if (games == null)
            {
                return HttpNotFound();
            }
            return View(games);
        }

        // GET: Mgr/Games/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Mgr/Games/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,ZhuChang,KeChang,ZhuChangSuoXie,KeChangSuoXie,ZhuChangScore,KeChangScore,Status,StartTime")] Games games)
        {
            if (ModelState.IsValid)
            {
                games.CreateTime = DateTime.Now;
                db.Games.Add(games);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(games);
        }

        // GET: Mgr/Games/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Games games = db.Games.Find(id);
            if (games == null)
            {
                return HttpNotFound();
            }
            return View(games);
        }

        // POST: Mgr/Games/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,ZhuChang,KeChang,ZhuChangSuoXie,KeChangSuoXie,ZhuChangScore,KeChangScore,Status,StartTime")] Games games)
        {
            if (ModelState.IsValid)
            {
                games.CreateTime = DateTime.Now;
                db.Entry(games).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(games);
        }

        // GET: Mgr/Games/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Games games = db.Games.Find(id);
            if (games == null)
            {
                return HttpNotFound();
            }
            return View(games);
        }

        // POST: Mgr/Games/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Games games = db.Games.Find(id);
            db.Games.Remove(games);
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
