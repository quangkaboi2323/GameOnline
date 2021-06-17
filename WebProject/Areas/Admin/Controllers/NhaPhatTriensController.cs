using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebProject.Models;

namespace WebProject.Areas.Admin.Controllers
{
    public class NhaPhatTriensController : Controller
    {
        private GameEntities1 db = new GameEntities1();

        // GET: Admin/NhaPhatTriens
        public ActionResult Index()
        {
            return View(db.NhaPhatTriens.ToList());
        }

        // GET: Admin/NhaPhatTriens/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NhaPhatTrien nhaPhatTrien = db.NhaPhatTriens.Find(id);
            if (nhaPhatTrien == null)
            {
                return HttpNotFound();
            }
            return View(nhaPhatTrien);
        }
        String getMaNPT()
        {
            var maMax = db.Games.ToList().Select(n => n.MaGame).Max();
            int npt = int.Parse(maMax.Substring(2)) + 1;
            string p = String.Concat("0", npt.ToString());
            return "NPT" + p.Substring(npt.ToString().Length - 1);
        }
        // GET: Admin/NhaPhatTriens/Create
        public ActionResult Create()
        {
            ViewBag.maNPT = getMaNPT();
            return View();
        }

        // POST: Admin/NhaPhatTriens/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MaNPT,TenNPT,LogoNPT")] NhaPhatTrien nhaPhatTrien)
        {
            var ImgNPT = Request.Files["Picture"];
            string postedFileName = System.IO.Path.GetFileName(ImgNPT.FileName);
            var path = Server.MapPath("/Assets/img/" + postedFileName);
            ImgNPT.SaveAs(path);
            if (ModelState.IsValid)
            {
                nhaPhatTrien.MaNPT = getMaNPT();
                nhaPhatTrien.LogoNPT = postedFileName;
                db.NhaPhatTriens.Add(nhaPhatTrien);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(nhaPhatTrien);
        }

        // GET: Admin/NhaPhatTriens/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NhaPhatTrien nhaPhatTrien = db.NhaPhatTriens.Find(id);
            if (nhaPhatTrien == null)
            {
                return HttpNotFound();
            }
            return View(nhaPhatTrien);
        }

        // POST: Admin/NhaPhatTriens/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MaNPT,TenNPT,LogoNPT")] NhaPhatTrien nhaPhatTrien)
        {
            if (ModelState.IsValid)
            {
                db.Entry(nhaPhatTrien).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(nhaPhatTrien);
        }

        // GET: Admin/NhaPhatTriens/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NhaPhatTrien nhaPhatTrien = db.NhaPhatTriens.Find(id);
            if (nhaPhatTrien == null)
            {
                return HttpNotFound();
            }
            return View(nhaPhatTrien);
        }

        // POST: Admin/NhaPhatTriens/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            NhaPhatTrien nhaPhatTrien = db.NhaPhatTriens.Find(id);
            db.NhaPhatTriens.Remove(nhaPhatTrien);
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
