using DeThi.Models;
using Microsoft.AspNetCore.Mvc;

namespace DeThi.Controllers
{
    public class CongNhanController : Controller
    {
        public IActionResult QuanLyDiemCachLy()
        {
            DataContext context = HttpContext.RequestServices.GetService(typeof(DataContext)) as DataContext;
            return View(context.sqlListDiemCachLy());
        }
        public IActionResult LietKeSoTC()
        {
            return View();
        }
        public IActionResult ListCongNhanByTC(int SoTrieuChung)
        {
            DataContext context = HttpContext.RequestServices.GetService(typeof(DataContext)) as DataContext;
            return View(context.sqlListBySoTC(SoTrieuChung));
        }
        public IActionResult ListSelectedOfCongNhan(string MaDiemCachLy)
        {
            DataContext context = HttpContext.RequestServices.GetService(typeof(DataContext)) as DataContext;
            return View(context.sqlListCongNhanCachLy(MaDiemCachLy));
        }
        public IActionResult DeleteCongNhan(string Id)
        {
            DataContext context = HttpContext.RequestServices.GetService(typeof(DataContext)) as DataContext;
            if (context.DeleteCongNhan(Id))
                ViewData["KetQua"] = "Xóa thành công";
            else ViewData["KetQua"] = "Xóa không thành công";
            return View();
        }
        public IActionResult ViewCongNhan(string Id)
        {
            DataContext context = HttpContext.RequestServices.GetService(typeof(DataContext)) as DataContext;
            var congNhan = context.GetCongNhan(Id);
            ViewData.Model = congNhan;
            return View();
        }
    }
}
