using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebProject.Models;
using System.Net;

namespace WebProject.Controllers
{
    public class GameController : Controller
    {
        // GET: Game
        private GameEntities1 db = new GameEntities1();
        static string maKhachHang;
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(KhachHang khach)
        {
            var user = db.KhachHangs.Where(n => n.Email == khach.Email && n.MatKhau == khach.MatKhau && n.QuyenHan == false).Count();
            var admin = db.KhachHangs.Where(n => n.Email == khach.Email && n.MatKhau == khach.MatKhau && n.QuyenHan == true).Count();            
            maKhachHang = khach.Email;
            TempData["Account"] = khach.Email;
            if (user > 0)
            {                
                return RedirectToAction("Home");
            }
            else if (admin > 0)
            {
                return RedirectToAction("Index", "Admin/Games");
            }
            else
            {
                return View();
            }            
        }
        String createMaKH()
        {
            var maMax = db.KhachHangs.ToList().Select(n => n.MaKH).Max();
            int kh = int.Parse(maMax.Substring(2)) + 1;
            string p = String.Concat("0", kh.ToString());
            return "KH" + p.Substring(kh.ToString().Length - 1);
        }
        public ActionResult Register()
        {
            ViewBag.MaKH = createMaKH();
            return View();
        }

        // POST: Admin/KhachHangAD/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register([Bind(Include = "MaKH,TenKH,NamSinh,Email,ViKhachHang,SoDienThoai,DS_GameMua,Ds_YeuThich,MatKhau,QuyenHan")] KhachHang khachHang)
        {
            if (ModelState.IsValid)
            {
                khachHang.MaKH = createMaKH();
                db.KhachHangs.Add(khachHang);
                db.SaveChanges();
                return RedirectToAction("Login");
            }

            return View(khachHang);
        }
        [ChildActionOnly]
        public ActionResult RenderTheLoai()
        {
            ViewBag.TL = db.TheLoais.SqlQuery("Select * from TheLoai");
            return PartialView("_TheLoai");
        }
        [ChildActionOnly]
        public ActionResult RenderNhaPhatTrien()
        {
            ViewBag.NPT = db.NhaPhatTriens.SqlQuery("Select * from NhaPhatTrien");
            return PartialView("_NhaPhatTrien");
        }
        [ChildActionOnly]
        public ActionResult RenderUser()
        {
            ViewBag.KH = db.KhachHangs.SqlQuery("Select * from KhachHang where Email = '" + maKhachHang + "'");
            return PartialView("_User");
        }
        public ActionResult Home()
        {
            ViewBag.GameLatest = db.Games.SqlQuery("Select * from Game order by NgayRaMat");
            ViewBag.GameFree = db.Games.SqlQuery("Select top 3 * from Game where Gia = 0 order by LuotTai desc");
            ViewBag.GameNotFree = db.Games.SqlQuery("Select top 3 * from Game order by LuotTai desc");
            ViewBag.TaiNhieu = db.Games.SqlQuery("Select top 3 * from Game order by NgayRaMat desc");
            ViewBag.NPT = db.NhaPhatTriens.SqlQuery("Select * from NhaPhatTrien");
            return View();
        }
        public ActionResult TimKiem(string maGame = "", string tenGame = "", string giaMin = "", string giaMax = "", string maTheLoai = "", string maNPT = "", string maDoTuoi = "")
        {
            string min = giaMin, max = giaMax;
            ViewBag.maGame = maGame;
            ViewBag.tenGame = tenGame;
            if (giaMin == "")
            {
                ViewBag.giaMin = "";
                min = "0";
            }
            else
            {
                ViewBag.giaMin = giaMin;
                min = giaMin;
            }
            if (max == "")
            {
                max = Int32.MaxValue.ToString();
                ViewBag.giaMax = "";
            }
            else
            {
                ViewBag.giaMax = giaMax;
                max = giaMax;
            }
            ViewBag.maTheLoai = new SelectList(db.TheLoais, "MaTheLoai", "TenTheLoai");
            ViewBag.maNPT = new SelectList(db.NhaPhatTriens, "MaNPT", "TenNPT");
            ViewBag.maDoTuoi = new SelectList(db.DoTuois, "MaDoTuoi", "Tuoi");
            var games = db.Games.SqlQuery("Game_TimKiem'" + maGame + "','" + tenGame + "','" + min + "','" + max + "','" + maTheLoai + "','" + maNPT + "','" + maDoTuoi + "'");
            if (games.Count() == 0)
                ViewBag.TB = "Không có thông tin tìm kiếm.";
            return View(games.ToList());
        }
        public ActionResult Index(int? page)
        {
            if (page == null) page = 1;
            var games = (from l in db.Games select l)
                .OrderBy(n => n.MaGame);
            int pageSize = 4;
            int pageNumber = (page ?? 1);
            return View(games.ToPagedList(pageNumber, pageSize));
        }
        public ActionResult Details_TheLoai(string id)
        {
            var tl = db.Games.SqlQuery("select * from Game where MaTheLoai = '" + id + "' ").ToList();
            return View(tl);
        }
        public ActionResult Details_NhaPhatTrien(string id)
        {
            var tl = db.Games.SqlQuery("select * from Game where MaNPT = '" + id + "' ").ToList();
            return View(tl);
        }
        // GET: Game/Details/5
        public ActionResult Details(String id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Game games = db.Games.Find(id);
            if (games == null)
            {
                return HttpNotFound();
            }
            ViewBag.gameSame = db.Games.SqlQuery("Select * from Game where MaTheLoai = '" + games.MaTheLoai + "'").ToList();
            return View(games);
        }
        public ActionResult Done()
        {
            return View();
        }
        string LayMaHD()
        {
            var maMax = db.HoaDons.ToList().Select(n => n.MaHD).Max();
            int maNV = int.Parse(maMax.Substring(2)) + 1;
            string NV = String.Concat("0", maNV.ToString());
            return "HD" + NV.Substring(maNV.ToString().Length - 1);
        }
        // GET: Game/Create
        public ActionResult Create(string id)
        {
            TempData["MaGame"] = id;
            ViewBag.MaHD = LayMaHD();
            ViewBag.MaKH = db.KhachHangs.SqlQuery("Select * from KhachHang where Email = '" + maKhachHang + "'");
            ViewBag.game = db.Games.SqlQuery("Select * from Game where MaGame = '" + id + "'"); 
            ViewBag.MaGame = id;
            ViewBag.date = DateTime.Today.ToString("dd/MM/yyyy");
            return View();
        }

        // POST: GiangVien_60130835/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(string id, [Bind(Include = "MaHD,MaGame,MaKH,NgayThanhToan")] HoaDon hoaDon)
        {                        
            if (ModelState.IsValid)
            {
                hoaDon.MaHD = LayMaHD();                
                hoaDon.MaGame = id;                
                hoaDon.NgayThanhToan = DateTime.Today;
                db.HoaDons.Add(hoaDon);
                db.SaveChanges();
                KhachHang khach = db.KhachHangs.Single(p => p.Email == maKhachHang);
                Game game = db.Games.Single(p => p.MaGame == id);
                khach.ViKhachHang = khach.ViKhachHang - game.Gia;
                game.LuotTai++;
                db.SaveChanges();
                return RedirectToAction("Done");
            }            
            return View(hoaDon);
        }
        //public void EditWallet(String maKH, int phi, [Bind(Include = "MaKH,TenKH,NamSinh,Email,ViKhachHang,SoDienThoai,DS_GameMua,Ds_YeuThich,MatKhau,QuyenHan")] KhachHang khachHang)
        //{
        //    khachHang.
        //}
        // GET: Game/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Game/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Game/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Game/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
