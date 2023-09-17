using RapChieuPhim.Dao;
using RapChieuPhim.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace RapChieuPhim.Controllers
{
    public class HomeController : Controller
    {
        private PhimDao daoP = new PhimDao();
        Data dbl = new Data();
        public ActionResult Index()
        {
            List<Phim> list = dbl.Phims.AsQueryable().Where(g => g.TinhTrang == true).ToList();
            ViewData["TopView"] = list;
            List<Phim> listHD = dbl.Phims.AsQueryable().Where(g => g.TinhTrang == false).ToList();
            ViewData["HanhDong"] = listHD;
            ViewData["loai"] = dbl.LoaiPhims.ToList();
            Data db = new Data();
            List<Ghe> lisssst = db.Ghes.ToList();
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}