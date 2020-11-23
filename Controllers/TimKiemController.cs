using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ecommerce.Models;
using PagedList;

namespace ecommerce.Controllers
{
    public class TimKiemController : Controller
    {
        // GET: TimKiem
        DbecommerceEntities db = new DbecommerceEntities();
        [HttpGet]
        public ActionResult KetQuaTimKiem(string sTuKhoa, int? page)
        {
            if (Request.HttpMethod != "GET")
            {
                page = 1;
            }
            //Thực hiện chức năng phân trang
            // Tao bien so san pham tren trang
            int Pagesize = 2;
            //Tao bien so 2 so trang hien tai
            int PageNumber = (page ?? 1);
            //tim kiem theo ten san pham
            var lstSP = db.SanPham.Where(n => n.TenSP.Contains(sTuKhoa));
            ViewBag.TuKhoa = sTuKhoa;
            return View(lstSP.OrderBy(n => n.TenSP).ToPagedList(PageNumber, Pagesize));
        }
        [HttpPost]
        public ActionResult LayTuKetQuaTimKiem(string sTuKhoa)
        {
            return RedirectToAction("KetQuaTimKiem",new {sTuKhoa=sTuKhoa });
        }
    }
}