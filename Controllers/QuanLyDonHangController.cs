using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ecommerce.Models;
using System.Net.Mail;

namespace ecommerce.Controllers
{
    [Authorize(Roles ="QuanTri,QuanLyDonHang")]
    public class QuanLyDonHangController : Controller
    {
        DbecommerceEntities db = new DbecommerceEntities();
        // GET: QuanLyDonHang
        public ActionResult DonHangChuaThanhToan()
        {
            var lst = db.DonDatHang.Where(n => n.DaThanhToan == false).OrderBy(n => n.NgayDat);
            return View(lst);
        }
        public ActionResult ChuaGiao()
        {
            //Lay danh sach cac don hang Chua duyet
            var lstDSDHCG = db.DonDatHang.Where(n => n.TinhTrangGiaoHang == false&&n.DaThanhToan==true).OrderBy(n => n.NgayGiao);
            return View(lstDSDHCG);
        }
        public ActionResult DaGiaoDaThanhToan()
        {
            var lstDSDHCG = db.DonDatHang.Where(n => n.TinhTrangGiaoHang == true && n.DaThanhToan == true);
            return View(lstDSDHCG);
        }
        [HttpGet]
        public ActionResult DuyetDonHang(int? id)
        {
            // Kiem tra id co hop le khong
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DonDatHang dh = db.DonDatHang.SingleOrDefault(n => n.MaDDH == id); 
            if (dh == null)
            {
                return HttpNotFound();
            }
            //Lay Danh Sach chi tiet don hang de hien thi cho nguoi dung thay
            var lstChiTietDonHang = db.ChiTietDonDatHang.Where(n => n.MaDDH == id);
            ViewBag.ListChiTietDonHang = lstChiTietDonHang;
            return View(dh);
        }
        [HttpPost]
        public ActionResult DuyetDonHang(DonDatHang ddh)
        {
            // truy cap lay du lieu 
            DonDatHang ddhUpdate = db.DonDatHang.SingleOrDefault(n => n.MaDDH == ddh.MaDDH);
            ddhUpdate.DaThanhToan = ddh.DaThanhToan;
            ddhUpdate.TinhTrangGiaoHang = ddh.TinhTrangGiaoHang;
            db.SaveChanges();
            //Lay danh sach chi tiet hien don hang de hien thi cho nguoi dung thay
            var lstChiTietDH = db.ChiTietDonDatHang.Where(n => n.MaDDH == ddh.MaDDH);
            ViewBag.ListChiTietDH = lstChiTietDH;
            var a = "NoiDung";
            // Gửi Khách Hàng 1 mail xác nhận thanh Toán
            GuiEmail("Xác nhận đơn hàng của hệ thống newstore", ddhUpdate.KhachHang.Email.ToString(), "testecommercehcmue@gmail.com", "Taogmail.com",a.ToString() );
            return RedirectToAction("ChuaGiao");


        }
        public void GuiEmail(string Title,string ToEmail,string FromEmail, string Password,string Content)
        {
            MailMessage mail = new MailMessage();
            mail.To.Add(ToEmail);// Địa Chỉ Người Nhận 
            mail.From = new MailAddress(ToEmail); // Địa Chỉ Người Gửi
            mail.Subject = Title;// Tieu Đê
            mail.Body = Content;// Noi Dung
            mail.IsBodyHtml = true;
            SmtpClient smtp = new SmtpClient();
            smtp.Host = "smtp.gmail.com";// host gui Email
            smtp.Port = 587;
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = new System.Net.NetworkCredential
            (FromEmail, Password);//Tai Khoan Nguoi Gui
            smtp.EnableSsl = true;// kich hoat giao tiep an toan SSL
            smtp.Send(mail);//Gui mail di

        }
    }
}