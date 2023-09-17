using RapChieuPhim.Dao;
using RapChieuPhim.Models;
using System;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RapChieuPhim.Controllers
{
    public class TaiKhoanController : Controller
    {
        private Data db = new Data();
        // GET: TaiKhoan
        [HttpPost]
        public bool Login(string username, string password)
        {
            var dao = new TaiKhoanDao();
            Data Data = new Data();
            TaiKhoan taiKhoan = dao.checkTaikhoan(username, password);
            if (taiKhoan != null)
            {
                var newCookie = new HttpCookie("myCookie", taiKhoan.Id.ToString());
                var idtt = taiKhoan.id_ThongTin;
                ThongTin m = Data.ThongTins.FirstOrDefault(x => x.ThongTin_id == idtt);
                newCookie.Expires = DateTime.Now.AddDays(10);
                Response.AppendCookie(newCookie);
                Session.Add("USERSESSIO", taiKhoan);
                Session["HoTen"] = m.TenNguoiDung;
                Session["PQ"] = taiKhoan.PhanQuyen;
                Session["nguoi"] = Data.ThongTins.FirstOrDefault(g => g.ThongTin_id == taiKhoan.id_ThongTin).TenNguoiDung;
                return true;
            }
            return false;
        }
        [HttpPost]
        public ActionResult Register(string username, string email, string password, string diachi, string gioitinh, string ngaysinh)
        {

            bool check = true;
            TaiKhoan taiKhoan = new TaiKhoan();
            ThongTin thongTin = new ThongTin();
            thongTin.TenNguoiDung = username;
            thongTin.NgaySinh = Convert.ToDateTime(ngaysinh);
            thongTin.Email = email;
            thongTin.DiaChi = diachi;
            thongTin.GioiTinh = gioitinh;
            db.ThongTins.Add(thongTin);
            db.SaveChanges();
            taiKhoan.TenDangNhap = email;
            taiKhoan.MatKhau = password;
            taiKhoan.TinhTrang = true;
            taiKhoan.NgayDangKy = DateTime.Now;
            taiKhoan.PhanQuyen = "5";
            taiKhoan.id_ThongTin = thongTin.ThongTin_id;
            db.TaiKhoans.Add(taiKhoan);
            db.SaveChanges();
            return RedirectToAction("Index", "Home");

        }
        public ActionResult SuaThongTin()
        {
            var tk = Convert.ToInt32(Request.Cookies["myCookie"].Value);
            var taikhoan = db.TaiKhoans.FirstOrDefault(g => g.Id == tk);
            var nguoi = db.ThongTins.FirstOrDefault(g => g.ThongTin_id == taikhoan.id_ThongTin);
            ViewBag.HoTen = nguoi.TenNguoiDung;
            ViewBag.DC = nguoi.DiaChi;
            ViewBag.GT = nguoi.GioiTinh;
            ViewBag.NS = nguoi.NgaySinh.Value.ToString("dd-MM-yyyy");
            ViewBag.Email = nguoi.Email;
            return View(taikhoan);
        }
        [HttpPost]
        public ActionResult SuaThongTin([Bind(Include = "Id,TenDangNhap,MatKhau,id_ThongTin")] TaiKhoan tk)
        {
            if (ModelState.IsValid)
            {
                tk.Id = Convert.ToInt32(Request["Id"]);
                tk.id_ThongTin = Convert.ToInt32(Request["id_ThongTin"]);
                tk.TenDangNhap = Request["TenDangNhap"];
                tk.NgayDangKy = Convert.ToDateTime(Request["NgayDangKy"]);
                tk.PhanQuyen = Request["PhanQuyen"];
                tk.TinhTrang = true;
                db.TaiKhoans.AddOrUpdate(tk);
                db.SaveChanges();
                ThongTin t = db.ThongTins.Find(tk.id_ThongTin);
                t.NgaySinh = Convert.ToDateTime(Request["NgaySinh"]);
                t.GioiTinh = Request["GioiTinh"];
                t.TenNguoiDung = Request["Ten"];
                t.DiaChi = Request["DiaChi"];
                t.Email = Request["Email"];
                db.ThongTins.AddOrUpdate(t);
                db.SaveChanges();
                return RedirectToAction("Index", "Home");
            }
            ViewData["loai"] = db.LoaiGhes.ToList();
            ViewData["phong"] = db.Phongs.ToList();
            return View(tk);
        }
        public ActionResult Logout(string username, string email, string password)
        {

            Session.Clear();
            if (Request.Cookies["myCookie"] != null)
            {
                //Fetch the Cookie using its Key.
                HttpCookie nameCookie = Request.Cookies["myCookie"];

                //Set the Expiry date to past date.
                nameCookie.Expires = DateTime.Now.AddDays(-1);

                //Update the Cookie in Browser.
                Response.Cookies.Add(nameCookie);

                //Set Message in TempData.
                TempData["Message"] = "Cookie deleted.";
            }
            return Redirect("/Home");

        }

    }
}