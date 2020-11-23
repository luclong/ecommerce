using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ecommerce.Models;
using CaptchaMvc.HtmlHelpers;
using CaptchaMvc;
using System.Web.Security;

namespace ecommerce.Controllers
{

    public class HomeController : Controller
    {
        // GET: Home
        DbecommerceEntities db = new DbecommerceEntities();
        public ActionResult MenuPartial()
        {

            var lstSP = db.SanPham;
            return PartialView(lstSP);
        }
        public ActionResult Index()
        {
            //Lan Luot Tao Ra Cac ViewBag de lay list san pham tu co so du lieu
            //List Dien Thoai 
            var lstDT = db.SanPham.Where(n => n.MaLoaiSP == 1);
            //Gan vao ViewBag
            ViewBag.ListDT = lstDT;
            //List May Tinh Bang
            var lstMTB = db.SanPham.Where(n => n.MaLoaiSP == 2 );
            //Gan vao ViewBag
            ViewBag.ListMTB = lstMTB;
            //List Laptop
            var lstLT = db.SanPham.Where(n => n.MaLoaiSP == 3);
            //Gan vao ViewBag
            ViewBag.ListLT = lstLT;
            return View();
        }
        [HttpGet]
        public ActionResult DangKy1()
        {
            return View();
        }
        [HttpPost]
        public ActionResult DangKy1(ThanhVien t)
        {
            return View();
        }
        [HttpGet]
        public ActionResult DangKy()
        {
            ViewBag.CauHoi = new SelectList(LoadCauHoi());
            return View();
        }
        [HttpPost]
        public ActionResult DangKy(ThanhVien tv)
        {
            ViewBag.CauHoi = new SelectList(LoadCauHoi());
            // Kiem Tra Captcha hop le
            ThanhVien kt = db.ThanhVien.SingleOrDefault(n => n.TaiKhoan == tv.TaiKhoan);
            if (kt != null)
            {
                ViewBag.TaiKhoan = "Tài khoản có người dùng";
                return View();
            }
            if (this.IsCaptchaValid("Captcha is not valid"))
            {
                if (ModelState.IsValid)
                {
                    ViewBag.ThongBao = "Thêm thành công";
                    // Them Khach Hang Vao CSDL
                    tv.MaLoaiTV = 3;
                    db.ThanhVien.Add(tv);
                    db.SaveChanges();
                }
                return View();
            }
            ViewBag.ThongBao = "Sai mã Captcha";
            return View();

        }
        
        public List<string> LoadCauHoi()
        {
            List<string> lstCauHoi = new List<string>();
            lstCauHoi.Add("Con vật mà bạn yêu thích?");
            lstCauHoi.Add("Ca sĩ mà bạn yêu thich?");
            lstCauHoi.Add("Hiện tại bạn đang làm gì?");
            return lstCauHoi;
        }
        [HttpGet]
        public int DangNhap(string username,string password)
        {
            //string staikhoan = f["txttaikhoan"].tostring();
            //string smatkhau = f["txtmatkhau"].tostring();
            //thanhvien tv = db.thanhvien.singleordefault(n => n.taikhoan == staikhoan && n.matkhau == smatkhau);
            //if (tv != null)
            //{
            //    session["taikhoan"] = tv;
            //    return content("<script>window.location.reload();</script>");
            //}
            //return content("tài khoản hoặc mật khẩu không đúng");
            string taikhoan = username;
            string matkhau = password;
            //Try van kiem tra dang nhap lay thong tin thanh vien
            ThanhVien tv = db.ThanhVien.SingleOrDefault(n => n.TaiKhoan == taikhoan && n.MatKhau == matkhau);
                if (tv != null)
                {
                    //Lay list quyen cua thanh vien tuong ung loaithanhvien
                    var lstQuyen = db.LoaiThanhVien_Quyen.Where(n => n.MaLoaiTV == tv.MaLoaiTV);
                    //Duyet list quyen
                    string Quyen = "";
                    foreach(var item in lstQuyen)
                    {
                        Quyen += item.Quyen.MaQuyen+",";
                    }
                    Quyen = Quyen.Substring(0, Quyen.Length - 1); //Cat dau ","
                    PhanQuyen(tv.TaiKhoan.ToString(), Quyen);
                    Session["TaiKhoan"] = tv;
                return 1;
                }
            return 0;
        }
        public void PhanQuyen (string TaiKhoan,string Quyen)
        {
            FormsAuthentication.Initialize();
            var ticket = new FormsAuthenticationTicket(1,
                                                     TaiKhoan,//user
                                                     DateTime.Now,//begin
                                                     DateTime.Now.AddHours(3),//timeout
                                                     false,
                                                     Quyen,//permission.. "admin" or more than one "DangKy,QuanLySanPham,QuanLyDonHang")
                                                     FormsAuthentication.FormsCookiePath);
            var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, FormsAuthentication.Encrypt(ticket));
            if (ticket.IsPersistent) cookie.Expires = ticket.Expiration;
            Response.Cookies.Add(cookie);
        }
        public ActionResult LoiPhanQuyen()
        {
           return View();
        }
        
        public ActionResult DangXuat()
        {
            Session["TaiKhoan"] = null;
            FormsAuthentication.SignOut();
            return RedirectToAction("Index");
        }
        public ActionResult GioiThieu()
        {
            return View();
        }
        public ActionResult TinTuc()
        {
            return View();
        }
        public ActionResult LienHe()
        {
            return View();
        }

    }
}





   