using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ecommerce.Models;
using PagedList;

namespace ecommerce.Controllers
{
    public class SanPhamController : Controller
    {
        // GET: SanPham
        DbecommerceEntities db = new DbecommerceEntities();
        // Tao 2 partial view san pham de hien thi 2 style khac nhau
        [ChildActionOnly]
        public ActionResult SanPhamStyle1Partial()
        {
            return PartialView();
        }
        public ActionResult SanPhamStyle2Partial()
        {
            return PartialView();
        }
        // Xây dựng sản phẩm chi tiết
        
        public ActionResult XemChiTiet(int? id, string tensp)
        {
            //Kiem tra truyen tham so rong
            if (id == null) {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SanPham sp = db.SanPham.SingleOrDefault(n => n.MaSP == id && n.DaXoa==false);
            //Neu khong thi truy xuat csdl lay ra san pham tuong ung
            if (sp == null) {
                // Thong bao neu san pham khong co san pham do
                return HttpNotFound();
            }
            return View(sp);
        }
        // Xay Dung 1 action  load san pham theo ma loai san pham va nha san xuat
        public ActionResult SanPham(int? MaLoaiSP, int? MaNSX,int? page) {
            if (MaNSX == null || MaLoaiSP == null) {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //Load dua 2 tieu chi tieu chi la MaLSP va MaNSX(2 truong trong bang san pham)
            var lstSP = db.SanPham.Where(n => n.MaLoaiSP == MaLoaiSP && n.MaNSX == MaNSX);
            if (lstSP == null) {
                return HttpNotFound();
            }
            //Thực hiện chức năng phân trang
            // Tao bien so san pham tren trang
            int Pagesize = 4;
            //Tao bien so 2 so trang hien tai
            int PageNumber = (page ?? 1);
            ViewBag.MaLoaiSP = MaLoaiSP;
            ViewBag.MaNSX = MaNSX;
            return View(lstSP.OrderBy(n=>n.MaSP).ToPagedList(PageNumber, Pagesize));
        }
        public ActionResult SanPhamStyle3Partial()
        {
            return PartialView();
        }
        public ActionResult ListSanPham()
        {
            //lay du lieu nap bao model (du lieu la dt moi)
            var lstSanPhamDT = db.SanPham.Where(n => n.MaLoaiSP == 1);
            ViewBag.ListSPDT = lstSanPhamDT;
            var lstSanPhamLT = db.SanPham.Where(n =>n.MaLoaiSP == 3);
            ViewBag.ListSPLT = lstSanPhamLT;
            var lstSanPhamMTB = db.SanPham.Where(n => n.MaLoaiSP == 2);
            ViewBag.ListSPMTB = lstSanPhamMTB;
            return View();
        }
    }
}