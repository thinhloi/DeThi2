using DeThi.Models;
using Microsoft.AspNetCore.Mvc;

namespace DeThi.Controllers
{
    public class DiemCachLyController : Controller
    {
        public IActionResult ThemDiemCachLy()
        {
            return View();
        }

        public IActionResult AddDiemCachLy(DiemCachLyModels diemCachLy)
        {
            DataContext context = HttpContext.RequestServices.GetService(typeof(DataContext)) as DataContext;

            if (context.InsertDiemCachLy(diemCachLy))
                ViewData["KetQua"] = "Thêm thành công";
            else
                ViewData["KetQua"] = "Thêm thất bại";

            return View();
        }
    }
}
