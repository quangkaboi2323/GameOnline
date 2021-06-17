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
    public class GamesController : Controller
    {
        private GameEntities1 db = new GameEntities1();

        // GET: Admin/Games
        public ActionResult Index()
        {
            var games = db.Games.Include(g => g.DoTuoi).Include(g => g.NgonNgu).Include(g => g.NhaPhatTrien).Include(g => g.TheLoai);
            return View(games.ToList());
        }

        // GET: Admin/Games/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Game game = db.Games.Find(id);
            if (game == null)
            {
                return HttpNotFound();
            }
            return View(game);
        }
        String getMaGame()
        {
            var maMax = db.Games.ToList().Select(n => n.MaGame).Max();
            int game = int.Parse(maMax.Substring(2)) + 1;
            string G = String.Concat("00", game.ToString());
            return "G" + G.Substring(game.ToString().Length - 1);
        }
        // GET: Admin/Games/Create
        public ActionResult Create()
        {
            ViewBag.MaGame = getMaGame();
            ViewBag.MaDoTuoi = new SelectList(db.DoTuois, "MaDoTuoi", "Tuoi");
            ViewBag.MaNN = new SelectList(db.NgonNgus, "MaNN", "TenNN");
            ViewBag.MaNPT = new SelectList(db.NhaPhatTriens, "MaNPT", "TenNPT");
            ViewBag.MaTheLoai = new SelectList(db.TheLoais, "MaTheLoai", "TenTheLoai");
            return View();
        }

        // POST: Admin/Games/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MaGame,TenGame,NgayRaMat,MoTa,DanhGia,SoLuong_DG,Gia,AnhMinhHoa,LuotTai,Hinh1,Hinh2,Hinh3,Hinh4,Dungluong,TenFile,OSName,MemoryName,ProcessName,GraphicsName,MaTheLoai,MaNPT,MaDoTuoi,MaNN")] Game game)
        {
            var imgMain = Request.Files["PictureMain"];
            var img1 = Request.Files["Picture1"];
            var img2 = Request.Files["Picture2"];
            var img3 = Request.Files["Picture3"];
            var img4 = Request.Files["Picture4"];
            string postedFileNameMain = System.IO.Path.GetFileName(imgMain.FileName);
            string postedFileName1 = System.IO.Path.GetFileName(img1.FileName);
            string postedFileName2 = System.IO.Path.GetFileName(img2.FileName);
            string postedFileName3 = System.IO.Path.GetFileName(img3.FileName);
            string postedFileName4 = System.IO.Path.GetFileName(img4.FileName);
            //Lưu hình đại diện về Server
            var path = Server.MapPath("/Assets/img/" + postedFileNameMain);
            var path1 = Server.MapPath("/Assets/img/" + postedFileName1);
            var path2 = Server.MapPath("/Assets/img/" + postedFileName2);
            var path3 = Server.MapPath("/Assets/img/" + postedFileName3);
            var path4 = Server.MapPath("/Assets/img/" + postedFileName4);
            imgMain.SaveAs(path);
            img1.SaveAs(path1);
            img2.SaveAs(path2);
            img3.SaveAs(path3);
            img4.SaveAs(path4);
            if (ModelState.IsValid)
            {
                game.MaGame = getMaGame();
                game.AnhMinhHoa = postedFileNameMain;
                game.Hinh1 = postedFileName1;
                game.Hinh2 = postedFileName2;
                game.Hinh3 = postedFileName3;
                game.Hinh4 = postedFileName4;
                db.Games.Add(game);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.MaDoTuoi = new SelectList(db.DoTuois, "MaDoTuoi", "Tuoi", game.MaDoTuoi);
            ViewBag.MaNN = new SelectList(db.NgonNgus, "MaNN", "TenNN", game.MaNN);
            ViewBag.MaNPT = new SelectList(db.NhaPhatTriens, "MaNPT", "TenNPT", game.MaNPT);
            ViewBag.MaTheLoai = new SelectList(db.TheLoais, "MaTheLoai", "TenTheLoai", game.MaTheLoai);
            return View(game);
        }

        // GET: Admin/Games/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Game game = db.Games.Find(id);
            if (game == null)
            {
                return HttpNotFound();
            }
            ViewBag.MaDoTuoi = new SelectList(db.DoTuois, "MaDoTuoi", "Tuoi", game.MaDoTuoi);
            ViewBag.MaNN = new SelectList(db.NgonNgus, "MaNN", "TenNN", game.MaNN);
            ViewBag.MaNPT = new SelectList(db.NhaPhatTriens, "MaNPT", "TenNPT", game.MaNPT);
            ViewBag.MaTheLoai = new SelectList(db.TheLoais, "MaTheLoai", "TenTheLoai", game.MaTheLoai);
            return View(game);
        }

        // POST: Admin/Games/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MaGame,TenGame,NgayRaMat,MoTa,DanhGia,SoLuong_DG,Gia,AnhMinhHoa,LuotTai,Hinh1,Hinh2,Hinh3,Hinh4,Dungluong,TenFile,OSName,MemoryName,ProcessName,GraphicsName,MaTheLoai,MaNPT,MaDoTuoi,MaNN")] Game game)
        {
            var imgMain = Request.Files["PictureMain"];
            var img1 = Request.Files["Picture1"];
            var img2 = Request.Files["Picture2"];
            var img3 = Request.Files["Picture3"];
            var img4 = Request.Files["Picture4"];            
            try
            {
                string postedFileNameMain = System.IO.Path.GetFileName(imgMain.FileName);
                string postedFileName1 = System.IO.Path.GetFileName(img1.FileName);
                string postedFileName2 = System.IO.Path.GetFileName(img2.FileName);
                string postedFileName3 = System.IO.Path.GetFileName(img3.FileName);
                string postedFileName4 = System.IO.Path.GetFileName(img4.FileName);
                //Lưu hình đại diện về Server
                var path = Server.MapPath("/Assets/img/" + postedFileNameMain);
                var path1 = Server.MapPath("/Assets/img/" + postedFileName1);
                var path2 = Server.MapPath("/Assets/img/" + postedFileName2);
                var path3 = Server.MapPath("/Assets/img/" + postedFileName3);
                var path4 = Server.MapPath("/Assets/img/" + postedFileName4);
                imgMain.SaveAs(path);
                img1.SaveAs(path1);
                img2.SaveAs(path2);
                img3.SaveAs(path3);
                img4.SaveAs(path4);                
            }
            catch
            { }
            if (ModelState.IsValid)
            {
                db.Entry(game).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.MaDoTuoi = new SelectList(db.DoTuois, "MaDoTuoi", "Tuoi", game.MaDoTuoi);
            ViewBag.MaNN = new SelectList(db.NgonNgus, "MaNN", "TenNN", game.MaNN);
            ViewBag.MaNPT = new SelectList(db.NhaPhatTriens, "MaNPT", "TenNPT", game.MaNPT);
            ViewBag.MaTheLoai = new SelectList(db.TheLoais, "MaTheLoai", "TenTheLoai", game.MaTheLoai);
            return View(game);
        }

        // GET: Admin/Games/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Game game = db.Games.Find(id);
            if (game == null)
            {
                return HttpNotFound();
            }
            return View(game);
        }

        // POST: Admin/Games/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Game game = db.Games.Find(id);
            db.Games.Remove(game);
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
