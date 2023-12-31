﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using RapChieuPhim.Models;
using RapChieuPhim.ViewModel;

namespace RapChieuPhim.Dao
{
    public class TaiKhoanDao
    {
        private Data db = new Data();
        public TaiKhoan checkTaikhoan(string username, string password)
        {
           

           
           TaiKhoan checkTK=  db.TaiKhoans.Include(t => t.ThongTin).FirstOrDefault(acc => acc.TenDangNhap == username && acc.MatKhau == password);
            return checkTK;

        }

        

        public TaiKhoan SaveTaiKhoan(TaiKhoan taiKhoan)
        {
            db.TaiKhoans.Add(taiKhoan);
            db.SaveChanges();
            return null;
        }
    }
    }