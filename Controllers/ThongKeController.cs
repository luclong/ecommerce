using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ecommerce.Models;

namespace ecommerce.Controllers
{
    //[Authorize(Roles ="QuanTri")]
    public class ThongKeController : Controller
    {
        // GET: ThongKe
        DbecommerceEntities db = new DbecommerceEntities();
        public ActionResult StatisticalPartial()
        {
            ViewBag.SoNguoiTruyCap = HttpContext.Application["SoNguoiTruyCap"].ToString();//Số người truy cập Application
            ViewBag.SoNguoiDangOnline = HttpContext.Application["SoNguoiDangOnline"].ToString();
            ViewBag.TongDoanhThu = ThongKeTongDoangThu();
            ViewBag.ThongKeDonHang = ThongKeDonHang();
            ViewBag.ThongKeThanhVien = ThongKeThanhVien();
            return PartialView();
        }
        public ActionResult Index()
        {
            ViewBag.SoNguoiTruyCap = HttpContext.Application["SoNguoiTruyCap"].ToString();//Số người truy cập Application
            ViewBag.SoNguoiDangOnline = HttpContext.Application["SoNguoiDangOnline"].ToString();
            ViewBag.TongDoanhThu = ThongKeTongDoangThu();
            ViewBag.ThongKeDonHang = ThongKeDonHang();
            ViewBag.ThongKeThanhVien = ThongKeThanhVien();
            return View();
        }
        public double ThongKeDonHang()
        {
            double slddh = db.DonDatHang.Count();
            return slddh;
        }
        public double ThongKeThanhVien()
        {
            double sltv = db.ThanhVien.Count();
            return sltv;
        }
        //Tong Doanh Thu
        public decimal ThongKeTongDoangThu()
        {
            decimal TongDoanhThu = db.ChiTietDonDatHang.Sum(n => n.SoLuong * n.DonGia).Value;
            return TongDoanhThu;
        }
        //Doanh thu theo Thang
        public decimal ThongKeTongDoangThuThang(int Thang,int Nam)
        {
            //Thong ke theo thang nam tuong ung
            var lstDDH = db.DonDatHang.Where(n => n.NgayDat.Value.Month == Thang && n.NgayDat.Value.Year == Nam);
            decimal TongTien = 0;
            //Duyet Chi Tiet Don DHH  lay thong tong tien tat ca cac san pham thuoc don hang do
            foreach(var item in lstDDH)
            {
                TongTien += decimal.Parse(item.ChiTietDonDatHang.Sum(n => n.SoLuong * n.DonGia).Value.ToString());

            }
            return TongTien;
        }
        protected override void Dispose(bool disposing)
        {
            if(disposing)
            {
                if (db != null)
                    db.Dispose();
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}