using RapChieuPhim.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RapChieuPhim.Dao
{
    public class ThongTinDao
    {
        private Data db = new Data();
        public ThongTin SaveTT(ThongTin thongTin)
        {
            ThongTin tt = db.ThongTins.Add(thongTin);
            db.SaveChanges();
            return tt;
        }
    }
}