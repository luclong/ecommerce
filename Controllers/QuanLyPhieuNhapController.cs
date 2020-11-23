using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ecommerce.Models;

namespace ecommerce.Controllers
{
    [Authorize(Roles ="QuanTri")]
    public class QuanLyPhieuNhapController : Controller
    {
        DbecommerceEntities db = new DbecommerceEntities();
        // GET: QuanLyPhieuNhap
        [HttpGet]
        public ActionResult NhapHang()
        {
            ViewBag.MaNCC = db.NhaCungCap;
            ViewBag.ListSanPham = db.SanPham;
            return View();
        }
        [HttpPost]
        public ActionResult NhapHang(PhieuNhap model,IEnumerable<ChiTietPhieuNhap> ModelList)
        {
            ViewBag.MaNCC = db.NhaCungCap;
            ViewBag.ListSanPham = db.SanPham;
            //Sau khi kiem tra dau vao
            //Gan Xoa false
            model.DaXoa = false;
            db.PhieuNhap.Add(model);
            db.SaveChanges();
            //SaveChanges de lay duoc phieu nhap gan cho lstChiTiétnaPham
            SanPham sp;
            foreach(var item in ModelList)
            {
                // CapNhat So Luong Ton Cua San Pham
                sp = db.SanPham.Single(n => n.MaSP == item.MaSP);
                sp.SoLuongTon =sp.SoLuongTon + item.SoLuongNhap;
                // Gan ma phieu nhap cho chi tiet phieu nhap
                item.MaSP = model.MaPN;
            }
            db.ChiTietPhieuNhap.AddRange(ModelList);
            db.SaveChanges();
            return View();
        }
        
    }
}