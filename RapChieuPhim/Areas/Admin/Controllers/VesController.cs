﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using RapChieuPhim.Models;

namespace RapChieuPhim.Areas.Admin.Controllers
{
    public class VesController : BaseController
    {
        private Data db = new Data();

        // GET: Admin/Ves
        public ActionResult Index()
        {
            ViewData["ghe"] = db.Ghes.AsQueryable().ToList();
            ViewData["tt"]=db.ThongTins.AsQueryable().ToList();
            ViewData["tk"] = db.TaiKhoans.AsQueryable().ToList();
            return View(db.Ves.AsQueryable().ToList());
        }

        // GET: Admin/Ves/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ve ve = db.Ves.Find(id);
            if (ve == null)
            {
                return HttpNotFound();
            }
            return View(ve);
        }

        // GET: Admin/Ves/Create
        public ActionResult Create()
        {
            ViewBag.Id_LichChieu = new SelectList(db.LichChieux, "Id", "GioBatDau");
            ViewBag.IdRap = new SelectList(db.RapPhims, "Id", "TenRapChieu");
            ViewBag.IdTaiKhoan = new SelectList(db.TaiKhoans, "Id", "TenDangNhap");
            return View();
        }

        // POST: Admin/Ves/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,IdTaiKhoan,IdPhong,NgayDat,GiaVe,Soluong")] Ve ve)
        {
            if (ModelState.IsValid)
            {
                db.Ves.Add(ve);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            var ves = db.LichChieux.ToList();
            var rap=db.RapPhims.ToList();
            ViewBag.Id_LichChieu = new SelectList(db.LichChieux, "Id", "GioBatDau", ves);
            ViewBag.IdRap = new SelectList(db.RapPhims, "Id", "TenRapChieu", rap);
            ViewBag.IdTaiKhoan = new SelectList(db.TaiKhoans, "Id", "TenDangNhap", ve.IdTaiKhoan);
            return View(ve);
        }

        // GET: Admin/Ves/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ve ve = db.Ves.Find(id);
            if (ve == null)
            {
                return HttpNotFound();
            }
            var ves = db.LichChieux.ToList();
            var rap = db.RapPhims.ToList();
            ViewBag.Id_LichChieu = new SelectList(db.LichChieux, "Id", "GioBatDau", ves);
            ViewBag.IdRap = new SelectList(db.RapPhims, "Id", "TenRapChieu", rap);
            ViewBag.IdTaiKhoan = new SelectList(db.TaiKhoans, "Id", "TenDangNhap", ve.IdTaiKhoan);
            return View(ve);
        }

        // POST: Admin/Ves/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,IdTaiKhoan,IdPhong,NgayDat,GiaVe,Soluong")] Ve ve)
        {
            if (ModelState.IsValid)
            {
                db.Entry(ve).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            var ves = db.LichChieux.ToList();
            var rap = db.RapPhims.ToList();
            ViewBag.Id_LichChieu = new SelectList(db.LichChieux, "Id", "GioBatDau", ves);
            ViewBag.IdRap = new SelectList(db.RapPhims, "Id", "TenRapChieu", rap);
            ViewBag.IdTaiKhoan = new SelectList(db.TaiKhoans, "Id", "TenDangNhap", ve.IdTaiKhoan);
            return View(ve);
        }

        // GET: Admin/Ves/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ve ve = db.Ves.Find(id);
            if (ve == null)
            {
                return HttpNotFound();
            }
            return View(ve);
        }

        // POST: Admin/Ves/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Ve ve = db.Ves.Find(id);
            db.Ves.Remove(ve);
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
