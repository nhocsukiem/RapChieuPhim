using RapChieuPhim.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RapChieuPhim.Dao
{
    public class LoaiPhimDao
    {
        private Data db = new Data();
        public LoaiPhim GetLoaiPhimFindByID(int id)
        {
            return db.LoaiPhims.Find(id);
        }
    }
}