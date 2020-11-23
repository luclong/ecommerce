using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ecommerce.Models;

namespace ecommerce.Controllers
{

    public class GioHangController : Controller
    {
        DbecommerceEntities db = new DbecommerceEntities();
        //Lay Gio Hang
        public List<ItemGioHang> LayGioHang()
        {
            List<ItemGioHang> lstGioHang = Session["GioHang"] as List<ItemGioHang>;
            if (lstGioHang == null)
            {
                lstGioHang = new List<ItemGioHang>();
                Session["GioHang"] = lstGioHang;
            }
            return lstGioHang;
        }
        //Them gio hang thong thuong (load lai trang)
        public ActionResult ThemGioHang(int MaSP, string strURL)
        {
            //Kiem Tra San Pham Co Ton Tai Trong CSDL hay khong
            SanPham sp = db.SanPham.SingleOrDefault(n => n.MaSP == MaSP);
            if (sp == null)
            {
                //Trang Dan Khong Hop Le
                Response.StatusCode = 404;
                return null;
            }
            //Lay Gio Hang 
            List<ItemGioHang> lstGioHang = LayGioHang();
            //Truong Hop 1 neu san Pham Ton Tai Trong GIo Hang
            ItemGioHang spCheck = lstGioHang.SingleOrDefault(n => n.MaSP == MaSP);
            if (spCheck != null)
            {
                // Kiem Tra Khach Dat Truoc Khi Cho Khach Mua Hang
                if (sp.SoLuongTon < spCheck.SoLuong)
                {
                    return View("ThongBao");
                }
                spCheck.SoLuong++;
                spCheck.ThanhTien = spCheck.SoLuong * spCheck.DonGia;
                return Redirect(strURL);
            }
            ItemGioHang itemGH = new ItemGioHang(MaSP);
            if (sp.SoLuongTon < itemGH.SoLuong)
            {
                return View("ThongBao");
            }
            
            lstGioHang.Add(itemGH);
            return Redirect(strURL);

        }
        public decimal TinhTongSoLuong()
        {
            //Lay Gio Hang
            List<ItemGioHang> lstGioHang = Session["GioHang"] as List<ItemGioHang>;
            if (lstGioHang == null)
            {
                return 0;
            }
            return lstGioHang.Sum(n => n.SoLuong);
        }
        public decimal TongTien()
        {
            //Lay Gio Hang
            List<ItemGioHang> lstGioHang = Session["GioHang"] as List<ItemGioHang>;
            if (lstGioHang == null)
            {
                return 0;
            }
            return lstGioHang.Sum(n => n.ThanhTien);
        }
        public ActionResult GioHangPartial()
        {
            if (TongTien() == 0)
            {
                ViewBag.TongSoLuong = 0;
                ViewBag.TongTien = 0;
                return PartialView();
            }
            ViewBag.TongSoLuong = TinhTongSoLuong();
            ViewBag.TongTien = TongTien();

            return PartialView();
        }
        // GET: GioHang
        public ActionResult XemGioHang()
        {
            // Lay Gio Hang
            
            List<ItemGioHang> lstGioHang = LayGioHang();
            if (TongTien() == 0)
            {
                ViewBag.TongTien = 0;
            }
            ViewBag.TongTien = TongTien();
            return View(lstGioHang);
        }
        [HttpGet]
        public ActionResult SuaGioHang(int MaSP)
        {
            //Kiem tra session gio hang ton tai chua
            if (Session["GioHang"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            //Kiem Tra San Pham Co Ton Tai Trong CSDL hay khong
            SanPham sp = db.SanPham.SingleOrDefault(n => n.MaSP == MaSP);
            if (sp == null)
            {
                //Trang Dan Khong Hop Le
                Response.StatusCode = 404;
                return null;
            }
            //Lay list gio hang session
            List<ItemGioHang> lstGioHang = LayGioHang();
            //Kiem Tra xem co ton tai trong gio hang hay khong
            ItemGioHang spCheck = lstGioHang.SingleOrDefault(n => n.MaSP == MaSP);
            if (spCheck == null)
            {
                return RedirectToAction("Index", "Home");
            }
            //Lay list gio hang tao giao dien
            ViewBag.GioHang = lstGioHang;
            //Neu ton tai roi
            return View(spCheck);
        }
        [HttpPost]
        public ActionResult CapNhatGioHang(ItemGioHang itemGH)
        {
            //Kiem TRa So Luong Ton
            SanPham spCheck = db.SanPham.Single(n => n.MaSP == itemGH.MaSP);
            if (spCheck.SoLuongTon < itemGH.SoLuong)
            {
                return View("ThongBao");
            }
            // Cap Nhat So Luong trong session gio hang
            // Buoc1: Lay List<GioHang> tu session ["GioHang"] 
            List<ItemGioHang> lstGH = LayGioHang();
            // Buoc2: Lay San Pham Cap Nhat Tu Trong List <GioHang> ra
            ItemGioHang itemGHUpdate = lstGH.Find(n => n.MaSP == itemGH.MaSP);
            //Buoc3: Tien Hanh Cap Nhat So Luong Cung Thanh Tien
            itemGHUpdate.SoLuong = itemGH.SoLuong;
            itemGHUpdate.ThanhTien = itemGHUpdate.SoLuong * itemGHUpdate.DonGia;
            return RedirectToAction("XemGioHang");
        }
        public ActionResult XoaGioHang(int MaSP)
        {
            //Kiem tra session gio hang ton tai chua
            if (Session["GioHang"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            //Kiem Tra San Pham Co Ton Tai Trong CSDL hay khong
            SanPham sp = db.SanPham.SingleOrDefault(n => n.MaSP == MaSP);
            if (sp == null)
            {
                //Trang Dan Khong Hop Le
                Response.StatusCode = 404;
                return null;
            }
            //Lay list gio hang session
            List<ItemGioHang> lstGioHang = LayGioHang();
            //Kiem Tra xem co ton tai trong gio hang hay khong
            ItemGioHang spCheck = lstGioHang.SingleOrDefault(n => n.MaSP == MaSP);
            if (spCheck == null)
            {
                return RedirectToAction("Index", "Home");
            }
            lstGioHang.Remove(spCheck);
            return RedirectToAction("XemGioHang");
        }
        [HttpPost]
        public ActionResult DatHang(KhachHang kh)
        {
            //Kiem tra session gio hang ton tai chua
            if (Session["GioHang"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            KhachHang khang = new KhachHang();
            if (Session["TaiKhoan"] == null)
            {
                // Them Kh vao bang khach hang doi voi khach hang vang lai (kh ko tao tk)
                khang = kh;
                db.KhachHang.Add(khang);
                db.SaveChanges();
            }
            else
            {
                ThanhVien tv = Session["TaiKhoan"] as ThanhVien;
                khang.TenKH = tv.HoTen;
                khang.DiaChi = tv.DiaChi;
                khang.Email = tv.Email;
                khang.SoDienThoai = tv.SoDienThoai;
                khang.MaTV = tv.MaTV;
                db.KhachHang.Add(khang);
                db.SaveChanges();
            }
            // Thêm Đơn Hàng
            DonDatHang ddh = new DonDatHang();
            ddh.MaKH = khang.MaKH;
            ddh.NgayDat = DateTime.Now;
            ddh.TinhTrangGiaoHang = false;
            ddh.TinhTrangGiaoHang = false;


            ddh.DaThanhToan = false;
            ddh.UuDai = 0;
            ddh.DaHuy = false;
            ddh.DaXoa = false;
            db.DonDatHang.Add(ddh);
            db.SaveChanges();
            //Them Chi Tiet Don Hang
            List<ItemGioHang> lstGH = LayGioHang();
            foreach (var item in lstGH)
            {
                ChiTietDonDatHang ctdh = new ChiTietDonDatHang();
                ctdh.MaDDH = ddh.MaDDH;
                ctdh.MaSP = item.MaSP;
                ctdh.TenSP = item.TenSP;
                ctdh.SoLuong = item.SoLuong;
                ctdh.DonGia = item.DonGia;
                db.ChiTietDonDatHang.Add(ctdh);
            }
            db.SaveChanges();
            // Cap Nhat So Luong Ton San Pham
            List<ItemGioHang> lstGH1 = LayGioHang();
            foreach (var item1 in lstGH1)
            {
                var sp = db.SanPham.SingleOrDefault(n => n.MaSP == item1.MaSP);
                if (sp!=null)
                {
                    sp.SoLuongTon = sp.SoLuongTon - item1.SoLuong;
                    sp.TenSP = sp.TenSP;
                    sp.SoLanMua = sp.SoLanMua + item1.SoLuong;
                    db.SanPham.Add(sp);
                }
            }
            db.SaveChanges();
            Session["GioHang"] = null;
            return RedirectToAction("XemGioHang");
        }
        
    }
}