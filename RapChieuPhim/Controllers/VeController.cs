using RapChieuPhim.Areas.Admin.Controllers;
using RapChieuPhim.Dao;
using RapChieuPhim.Models;
using RapChieuPhim.ViewModel;
using System;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;

namespace RapChieuPhim.Controllers
{
    public class VeController : BaseController
    {
        // GET: Ve
        Data db = new Data();
        public ActionResult Index(int id)

        {
            var dao = new PhimDao();
            Session["id"] = id;
            PhimViewModel phimmodel = dao.getPhimFindbyId(id);
            Session.Add("PHIM", phimmodel);
            ViewData["listChair"] = db.Ghes.ToList();
            ViewData["Showtimes"] = db.LichChieux.ToList();
            ViewData["phimmodel"] = phimmodel;
            var LichChieu = db.LichChieux.Where(g => g.IdPhim == id).Select(g => g.NgayChieu).Distinct().ToList();
            ViewData["lc"] = LichChieu;
            ViewBag.Id = id;
            return View();
        }
        public ActionResult Phong(string Ngay)
        {
            DateTime dt = DateTime.ParseExact(Ngay, "dd-MM-yyyy", CultureInfo.InvariantCulture);
            var rc = db.LichChieux.Where(g => g.NgayChieu == dt).Select(g => g.IdPhong).ToList();
            var list = db.Phongs.Where(g => rc.Contains(g.Id)).Select(g => new { g.Id, g.TenPhong }).ToList();
            return Json(list, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GioXem(string Ngay = "", int IdPhong = 0)
        {
            DateTime dt = DateTime.ParseExact(Ngay, "dd-MM-yyyy", CultureInfo.InvariantCulture);
            var rc = db.LichChieux.Where(g => g.NgayChieu == dt && g.IdPhong == IdPhong).Select(g => new { GIOBD = g.GioBD.ToString().Replace(".0000000", ""), GIOKT = g.GioKT.ToString().Replace(".0000000", ""), g.Id }).ToList();
            Session["ngay"] = dt;
            return Json(rc, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Ghe(int? Id, string IdGhe = "")
        {
            var dao = new PhimDao();
            var id = Convert.ToInt16(Session["id"]);
            PhimViewModel phimmodel = dao.getPhimFindbyId(id);
            Session.Add("PHIM", phimmodel);
            ViewData["phimmodel"] = phimmodel;
            var IdPhong = db.LichChieux.FirstOrDefault(g => g.Id == Id).IdPhong;
            ViewBag.SoGheTheoHang = db.Phongs.FirstOrDefault(g => g.Id == IdPhong).Soluongghemoihang;
            var ghe = db.Ghes.Where(g => g.Id_phong == IdPhong).OrderByDescending(g => g.Loai_id).ToList();
            ViewBag.Id = Id;
            ViewData["ghe"] = ghe;
            ViewBag.IdGhe = IdGhe;
            ViewData["ghedat123"] = db.Ves.Where(g => g.IdLC == Id).Select(g => g.IdGhe).ToList();
            return View();
        }
        [HttpPost]
        public ActionResult Ghe(int Id = 0, string IdGhe = "")
        {
            if (Id > 0 && !string.IsNullOrEmpty(IdGhe))
            {
                var lstIdGhe = IdGhe.Split(',').Select(int.Parse).ToList();
                lstIdGhe.Remove(0);
                int IdGheint = 0;
                string IdVeStr = "";
                var lc = db.LichChieux.FirstOrDefault(g => g.Id == Id);
                for (int i = 0; i < lstIdGhe.Count; i++)
                {
                    IdGheint = lstIdGhe[i];
                    Ve v = new Ve();
                    var r = Convert.ToInt16(Request.Cookies["myCookie"].Value);
                    v.IdTaiKhoan = r;
                    v.IdPhim = lc.IdPhim;
                    v.NgayDat = DateTime.Now;
                    var LG = db.Ghes.FirstOrDefault(g => g.ghe_id == IdGheint).Loai_id;
                    v.GiaVe = Convert.ToDecimal(db.LoaiGhes.FirstOrDefault(p => p.Id == LG).GiaGhe.ToString());
                    v.IdGhe = IdGheint;
                    v.IdLC = Id;
                    db.Ves.Add(v);
                    db.SaveChanges();
                    IdVeStr += "," + v.Id;
                }
                Session["IdVe"] = IdVeStr.Trim(',');
            }
            return RedirectToAction("ThanhToan", "Ve");
        }

        public ActionResult ThanhToan()
        {
            var dao = new PhimDao();
            var Id = Convert.ToInt16(Session["id"]);
            var r = Convert.ToInt16(Request.Cookies["myCookie"].Value);
            var nguoi = db.TaiKhoans.FirstOrDefault(g => g.Id == r).id_ThongTin;
            var tt = db.ThongTins.Where(g => g.ThongTin_id == nguoi).ToList();
            if (tt.Count > 0)
            {
                ViewBag.HoTen = db.ThongTins.FirstOrDefault(g => g.ThongTin_id == nguoi).TenNguoiDung;
                ViewBag.Email= db.ThongTins.FirstOrDefault(g => g.ThongTin_id == nguoi).Email;
            }
            PhimViewModel phimmodel = dao.getPhimFindbyId(Id);
            Session.Add("PHIM", phimmodel);
            ViewData["phimmodel"] = phimmodel;
            var LichChieu = db.LichChieux.Where(g => g.IdPhim == Id).ToList();
            ViewData["lc"] = LichChieu;
            var IdVe = Session["IdVe"].ToString().Split(',').Select(int.Parse).ToList();
            var Ve_LC_Phim = db.Ve_LC_Phim.AsQueryable().Where(p => IdVe.Contains(p.Id)).ToList();
            ViewData["Ve_LC_Phim"] = Ve_LC_Phim;
            return View();
        }
        public ActionResult ThanhToanSau(string Id = "", string tien = "", string Ten = "", string SDT = "", string Email = "")
        {
            var dao = new PhimDao();
            var Idm = Convert.ToInt16(Session["id"]);
            PhimViewModel phimmodel = dao.getPhimFindbyId(Idm);
            Session.Add("PHIM", phimmodel);
            ViewData["phimmodel"] = phimmodel;
            var IdVe = Id.Split(',').Select(int.Parse).ToList();
            for (int i = 0; i < IdVe.Count; i++)
            {
                var id = Convert.ToInt16(IdVe[i]);
                Ve v = db.Ves.FirstOrDefault(g => g.Id == id);
                v.TinhTrang = 2;
                db.Ves.AddOrUpdate(v);
            }
            DatVe dv = new DatVe();
            var idve = Convert.ToInt16(IdVe[0]);
            var ve = db.Ves.FirstOrDefault(g => g.Id == idve);
            dv.IDTK = Convert.ToInt32(ve.IdTaiKhoan);
            dv.IDve = Id;
            dv.NgayDat = Convert.ToDateTime(ve.NgayDat);
            dv.TongTien = Convert.ToInt32(tien.ToLower().Trim());
            dv.SDT = SDT;
            dv.HoTen = Ten;
            dv.Email = Email;
            db.DatVes.Add(dv);
            db.SaveChanges();
            Session["IdVe"] = "";
            return View();
        }

    }
}