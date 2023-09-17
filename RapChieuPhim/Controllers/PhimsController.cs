using PagedList;
using RapChieuPhim.Dao;
using RapChieuPhim.Models;
using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using ZXing;

namespace RapChieuPhim.Controllers
{
    public class PhimsController : Controller
    {
        // GET: Phims
        Data db = new Data();
        public ActionResult Index(int? page, int? Id)
        {
            if (page == null)
                page = 1;
            var books = db.Phims.Where(g => g.TinhTrang == true).OrderBy(g => g.Id);
            int pageSize = 6;
            int pageNumber = (page ?? 1);

            if (Id != null)
            {
                ViewBag.Id = Id;
                books = books.Where(g => g.IdLoaiPhim.Contains(Id.ToString())).OrderBy(g => g.Id);
            }
            ViewBag.total = books.Count();
            return View(books.ToPagedList(pageNumber, pageSize));
        }
        public ActionResult PhimSapChieu(int? page, int? id)
        {
            if (page == null)
                page = 1;
            var books = db.Phims.Where(g => g.TinhTrang == false).OrderBy(g => g.Id);
           
            return View(id);
        }
        public ActionResult DetatilPhims(int? id)
        {
            var dao = new PhimDao();
            Session["idPhim"] = id;
            var phims = dao.getPhimFindbyId(id);
            ViewData["Binhluan"] = db.BinhLuan_Phim.Where(g => g.IdPhim == id).AsQueryable().ToList();
            Session.Add("PHIM", phims);
            ViewData["phimview"] = phims;
            return View();
        }
        public ActionResult LoadVe()
        {
            var tk = Convert.ToInt32(Request.Cookies["myCookie"].Value);
            var l = db.Ve_LC_Phim_TK.Where(g => g.Id_tk == tk).OrderBy(g => g.NgayDat).FirstOrDefault();
            var list = db.DatVes.Where(g => g.IDTK == tk).OrderBy(g => g.IDDatVe).FirstOrDefault();
            List<QRCodeModel> m = new List<QRCodeModel>();

            QRCodeModel k = new QRCodeModel();
            k.QRCodeText = list.IDve;
            k.QRCodeImagePath = GenerateQRCode(list.IDve);
            m.Add(k);

            ViewData["listanh"] = m.ToList();
            return View(l);
        }
        private string GenerateQRCode(string qrcodeText)
        {
            string folderPath = "~/Images/";
            string imagePath = "~/Images/QrCode.jpg";
            // If the directory doesn't exist then create it.
            if (!Directory.Exists(Server.MapPath(folderPath)))
            {
                Directory.CreateDirectory(Server.MapPath(folderPath));
            }

            var barcodeWriter = new BarcodeWriter();
            barcodeWriter.Format = BarcodeFormat.QR_CODE;
            var result = barcodeWriter.Write(qrcodeText);

            string barcodePath = Server.MapPath(imagePath);
            var barcodeBitmap = new Bitmap(result);
            using (MemoryStream memory = new MemoryStream())
            {
                using (FileStream fs = new FileStream(barcodePath, FileMode.Create, FileAccess.ReadWrite))
                {
                    barcodeBitmap.Save(memory, ImageFormat.Jpeg);
                    byte[] bytes = memory.ToArray();
                    fs.Write(bytes, 0, bytes.Length);
                }
            }
            return imagePath;
        }
        public ActionResult BinhLuan(string cmt = "")
        {
            var tk = Convert.ToInt32(Request.Cookies["myCookie"].Value);
            var idphim = int.Parse(Session["idPhim"].ToString());
            BinhLuan bl = new BinhLuan();
            bl.IdPhim = idphim;
            bl.NgayDang = System.DateTime.Now;
            bl.TinhTrang = true;
            bl.DanhGia = 1;
            bl.IdTK = tk;
            bl.NoiDung = cmt;
            db.BinhLuans.Add(bl);
            db.SaveChanges();
            var id = Convert.ToInt32(Session["idPhim"]);
            return RedirectToAction("DetatilPhims", "Phims", new { id = id });
        }
        public void XoaLichChieu()
        {
           
                var list = db.LichChieux.AsQueryable().Where(g => (g.NgayChieu < DateTime.Now) || (g.NgayChieu == DateTime.Now && g.GioBD.Value.Hours < DateTime.Now.Hour)).ToList();
                foreach (var l in list)
                {
                    LichChieu v = db.LichChieux.Find(l.Id);
                    var listc = db.Ves.Where(g => g.IdLC == l.Id).ToList();
                    foreach (var c in listc)
                    {
                        Ve m = db.Ves.Find(c.Id);
                        db.Ves.Remove(m);
                        db.SaveChanges();
                    }
                    db.LichChieux.Remove(v);
                    db.SaveChanges();
                }

            }
    }
}