using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ecommerce.Models;

namespace ecommerce.Controllers
{
    [Authorize(Roles ="QuanTri,QuanLySanPham")]
    public class QuanLySanPhamController : Controller
    {
        // GET: QuanLySanPham
        DbecommerceEntities db = new DbecommerceEntities();
        public ActionResult Index()
        {
            return View(db.SanPham.Where(n => n.DaXoa == false).OrderByDescending(n => n.MaSP));
        }
        [HttpGet]
        public ActionResult TaoMoi() {
            //load dropdownlist nha cung cap
            ViewBag.MaNCC = new SelectList(db.NhaCungCap.OrderBy(n => n.TenNCC), "MaNCC", "TenNCC");
            ViewBag.MaLoaiSP = new SelectList(db.LoaiSanPham.OrderBy(n => n.MaLoaiSP), "MaLoaiSP", "TenLoai");
            ViewBag.MaNSX = new SelectList(db.NhaSanXuat.OrderBy(n => n.MaNSX), "MaNSX", "TenNSX");
            return View();
        }
        [ValidateInput(false)]
        [HttpPost]
        public ActionResult TaoMoi(SanPham sp, HttpPostedFileBase[] HinhAnh)
        {
            ViewBag.MaNCC = new SelectList(db.NhaCungCap.OrderBy(n => n.TenNCC), "MaNCC", "TenNCC");
            ViewBag.MaLoaiSP = new SelectList(db.LoaiSanPham.OrderBy(n => n.MaLoaiSP), "MaLoaiSP", "TenLoai");
            ViewBag.MaNSX = new SelectList(db.NhaSanXuat.OrderBy(n => n.MaNSX), "MaNSX", "TenNSX");
            int loi = 0;
            for (int i = 0; i < HinhAnh.Count(); i++)
            {
                if (HinhAnh[i] != null)
                {
                    //Kiem Tra noi dung hinh anh
                    if (HinhAnh[i].ContentLength > 0)
                    {
                        if (HinhAnh[i].ContentType != "image/jpeg" && HinhAnh[i].ContentType != "image/png" && HinhAnh[i].ContentType != "image/gif" && HinhAnh[i].ContentType != "image/tiff" && HinhAnh[i].ContentType != "image/BMP" && HinhAnh[i].ContentType != "image/jpg")
                        {
                            ViewBag.upload += "Hình Ảnh" + i + "Không hợp lê <br />";
                            loi++;
                        }
                        else
                        {
                            //Lay hinh anh
                            var fileName = Path.GetFileName(HinhAnh[i].FileName);
                            //Lấy hình ảnh chuyển vào thư mục hình ảnh
                            var path = Path.Combine(Server.MapPath("~/Content/img-sp/SanPham/"), fileName);
                            //Nếu thư mục chứa hình ảnh đó rồi sẽ xuất ra thông báo 
                            if (System.IO.File.Exists(path))
                            {
                                ViewBag.upload = "Hình " + i + " đã tồn tại";
                                loi++;
                            }
                            else
                            {
                                //Lấy Hình Ảnh đưa vào thư muc HinhAnhSP
                                //HinhAnh[i].SaveAs(path);

                            }
                        }
                    }
                }
            }
            if (loi > 0)
            {
                return View(sp);
            }
            else
            {
                
                var fileName = Path.GetFileName(HinhAnh[0].FileName);
                var fileName1 = Path.GetFileName(HinhAnh[1].FileName);
                var fileName2 = Path.GetFileName(HinhAnh[2].FileName);
                var fileName3 = Path.GetFileName(HinhAnh[3].FileName);
                var fileName4 = Path.GetFileName(HinhAnh[4].FileName);
                var path = Path.Combine(Server.MapPath("~/Content/img-sp/SanPham/"), fileName);
                var path1 = Path.Combine(Server.MapPath("~/Content/img-sp/SanPham/"), fileName1);
                var path2 = Path.Combine(Server.MapPath("~/Content/img-sp/SanPham/"), fileName2);
                var path3 = Path.Combine(Server.MapPath("~/Content/img-sp/SanPham/"), fileName3);
                var path4 = Path.Combine(Server.MapPath("~/Content/img-sp/SanPham/"), fileName4);
                HinhAnh[0].SaveAs(path);
                sp.HinhAnh = fileName;
                HinhAnh[1].SaveAs(path1);
                sp.HinhAnh1 = fileName1;
                HinhAnh[2].SaveAs(path2);
                sp.HinhAnh2 = fileName2;
                HinhAnh[3].SaveAs(path3);
                sp.HinhAnh3 = fileName3;
                HinhAnh[4].SaveAs(path4);
                sp.HinhAnh4 = fileName4;
            }
            //Kiem Tra hình tồn tại trong csdl chưa
            //if (HinhAnh[0].ContentLength > 0)
            //{
                //Lay hinh anh
            //   var fileName = Path.GetFileName(HinhAnh[0].FileName);
                //Lấy hình ảnh chuyển vào thư mục hình ảnh
            //    var path = Path.Combine(Server.MapPath("~/Content/img-sp/SanPham/"), fileName);
                //Nếu thư mục chứa hình ảnh đó rồi sẽ xuất ra thông báo 
            //    if (System.IO.File.Exists(path))
            //    {
            //        ViewBag.upload = "Hình đã tồn tại";
            //        return View();
            //    }
            //    else
            //    {
                    //Lấy Hình Ảnh đưa vào thư muc HinhAnhSP
            //        HinhAnh[0].SaveAs(path);
            //        sp.HinhAnh = fileName;
             //   }
            //}*@
            db.SanPham.Add(sp);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        [HttpGet]
        public ActionResult ChinhSua(int? id)
        {
            if (id == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            SanPham sp = db.SanPham.SingleOrDefault(n => n.MaSP == id);
            if (sp == null)
            {
                return HttpNotFound();
            }
            //load dropdownlist nha cung cap
            ViewBag.MaNCC = new SelectList(db.NhaCungCap.OrderBy(n => n.TenNCC), "MaNCC", "TenNCC", sp.MaNCC);
            ViewBag.MaLoaiSP = new SelectList(db.LoaiSanPham.OrderBy(n => n.MaLoaiSP), "MaLoaiSP", "TenLoai", sp.MaLoaiSP);
            ViewBag.MaNSX = new SelectList(db.NhaSanXuat.OrderBy(n => n.MaNSX), "MaNSX", "TenNSX", sp.MaNSX);
            return View(sp);
        }
        [ValidateInput(false)]
        [HttpPost]
        public ActionResult ChinhSua(SanPham model)
        {
            ViewBag.MaNCC = new SelectList(db.NhaCungCap.OrderBy(n => n.TenNCC), "MaNCC", "TenNCC", model.MaNCC);
            ViewBag.MaLoaiSP = new SelectList(db.LoaiSanPham.OrderBy(n => n.MaLoaiSP), "MaLoaiSP", "TenLoai", model.MaLoaiSP);
            ViewBag.MaNSX = new SelectList(db.NhaSanXuat.OrderBy(n => n.MaNSX), "MaNSX", "TenNSX", model.MaNSX);
            db.Entry(model).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            // Neu du lieu dau vao chac chan
            //if (ModelState.IsValid)
            //{
            //    db.Entry(model).State = System.Data.Entity.EntityState.Modified;
            //    db.SaveChanges();
            //    return RedirectToAction("Index");
            //}
            return RedirectToAction("Index");
        }
        [HttpGet]
        public ActionResult Xoa(int? id)
        {
            if (id == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            SanPham sp = db.SanPham.SingleOrDefault(n => n.MaSP == id);
            if (sp == null)
            {
                return HttpNotFound();
            }
            //load dropdownlist nha cung cap
            ViewBag.MaNCC = new SelectList(db.NhaCungCap.OrderBy(n => n.TenNCC), "MaNCC", "TenNCC", sp.MaNCC);
            ViewBag.MaLoaiSP = new SelectList(db.LoaiSanPham.OrderBy(n => n.MaLoaiSP), "MaLoaiSP", "TenLoai", sp.MaLoaiSP);
            ViewBag.MaNSX = new SelectList(db.NhaSanXuat.OrderBy(n => n.MaNSX), "MaNSX", "TenNSX", sp.MaNSX);
            return View(sp);
        }
        [HttpPost]
        public ActionResult Xoa(int id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SanPham sp = db.SanPham.SingleOrDefault(n => n.MaSP==id);
            if (sp == null)
            {
                return HttpNotFound();
            }
            db.SanPham.Remove(sp);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}