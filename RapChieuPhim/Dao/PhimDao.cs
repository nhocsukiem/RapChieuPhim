using PagedList;
using RapChieuPhim.Models;
using RapChieuPhim.ViewModel;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace RapChieuPhim.Dao
{
    public class PhimDao

    {
        private Data db = new Data();



        public List<PhimViewModel> GetListPhims()
        {

            List<PhimViewModel> list = new List<PhimViewModel>();
            //foreach (var phim in phims)
            //{
            //    PhimViewModel phimView = new PhimViewModel();
            //    phimView.Id = phim.Id;
            //    phimView.TenPhim = phim.TenPhim;
            //    phimView.AnhPhim = phim.AnhPhim;
            //    phimView.DaoDien = phim.DaoDien;
            //    phimView.DienVien = phim.DienVien;
            //    phimView.ChatLuong = phim.ChatLuong;
            //    phimView.NgayCongChieu = phim.NgayCongChieu;
            //    phimView.NamPhatHanh = phim.NamPhatHanh;
            //    phimView.NgayKetThuc = phim.NgayKetThuc;
            //    phimView.TenLoai = phim.LoaiPhim.TenLoai;
            //    phimView.ThoiLuong = phim.ThoiLuong;
            //    phimView.TinhTrang = phim.TinhTrang;
            //    phimView.MoTa = phim.MoTa;
            //    list.Add(phimView);
            //}
            return list;

        }

        public PhimViewModel getPhimFindbyId(int? id)
        {
            Phim phim = db.Phims.Find(id);
            PhimViewModel phimView = new PhimViewModel();
            phimView.Id = phim.Id;
            phimView.TenPhim = phim.TenPhim;
            phimView.AnhPhim = phim.AnhPhim;
            phimView.DaoDien = phim.DaoDien;
            phimView.DienVien = phim.DienVien;
            phimView.ChatLuong = phim.ChatLuong;
            phimView.NgayCongChieu = phim.NgayCongChieu;
            phimView.NamPhatHanh = phim.NamPhatHanh;
            phimView.NgayKetThuc = phim.NgayKetThuc;

            phimView.ThoiLuong = phim.ThoiLuong;
            phimView.TinhTrang = phim.TinhTrang;
            phimView.MoTa = phim.MoTa;
            return phimView;

        }

        public List<PhimViewModel> GetListPhimsByHanhDong()
        {

            List<PhimViewModel> list = new List<PhimViewModel>();

            return list;

        }
        public List<PhimViewModel> GetListPhimsByDC()
        {

            List<PhimViewModel> list = new List<PhimViewModel>();

            return list;

        }
        public List<PhimViewModel> GetListPhimsByHoatHinh()
        {

            List<PhimViewModel> list = new List<PhimViewModel>();

            return list;

        }
        public List<PhimViewModel> GetListPhimsByKinhDi()
        {

            List<PhimViewModel> list = new List<PhimViewModel>();

            return list;

        }
        public List<PhimViewModel> GetListPhimsByHanhDongSC()
        {

            List<PhimViewModel> list = new List<PhimViewModel>();

            return list;

        }
        public List<PhimViewModel> GetListPhimsByDCSC()
        {

            List<PhimViewModel> list = new List<PhimViewModel>();

            return list;

        }
        public List<PhimViewModel> GetListPhimsByHoatHinhSC()
        {

            List<PhimViewModel> list = new List<PhimViewModel>();

            return list;

        }
        public List<PhimViewModel> GetListPhimsByKinhDiSC()
        {

            List<PhimViewModel> list = new List<PhimViewModel>();

            return list;

        }
        public IEnumerable<Phim> GetListPageListPhims(int page, int pageSize)
        {

            return db.Phims.OrderByDescending(p => p.Id).ToPagedList(page, pageSize);

        }
        public IEnumerable<Phim> GetPhims(int? Id)
        {
            if (Id != null)
            {
                return db.Phims.Where(g => g.IdLoaiPhim.Contains(Id.ToString()) && g.TinhTrang == true).ToList();
            }
            else
            {
                return db.Phims.Where(g => g.TinhTrang == true).ToList();
            }

        }
    }
}