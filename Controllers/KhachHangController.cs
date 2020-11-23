using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ecommerce.Models;

namespace ecommerce.Controllers
{
    public class KhachHangController : Controller
    {
        // GET: KhachHang
        DbecommerceEntities db = new DbecommerceEntities();
        [Authorize(Roles ="QuanLyDonHang")]
        public ActionResult Index()
        {
            /*var lstKH = from KH in db.KhachHang select KH;
            return View(lstKH);*/
            var listKH = db.KhachHang;
            return View(listKH);
        }
        public ActionResult Index1()
        {
            var lstKH = from KH in db.KhachHang select KH;
            return View(lstKH);
        }
        public ActionResult TruyVanDoiTuong()
        {
            // B1 Truy Van 1 Doi Tuong
            var lst = from kh in db.KhachHang where kh.MaKH==1 select kh;
            // B2
            //KhachHang khach = lst.FirstOrDefault();
            KhachHang khang = db.KhachHang.SingleOrDefault(n => n.MaKH == 3);
            return View(khang);
        }
        public ActionResult SortDuLieu()
        {
            List<KhachHang> lstKh = db.KhachHang.OrderBy(n => n.TenKH).ToList();
            return View(lstKh);

        }
        public ActionResult GroupDuLieu()
        {
            List<ThanhVien> lstKh = db.ThanhVien.OrderBy(n => n.TaiKhoan).ToList();
            return View(lstKh);
            //52
        }
    }
}