using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Teagle.Facts.Web.ViewModels;

namespace Teagle.Facts.Web.Controllers
{
    public class SiteController : Controller
    {

        public IActionResult Index(int? pageIndex, string tag, string search)
        {
            ViewData["Index"]= pageIndex;
            ViewData["Tag"]= tag;
            ViewData["Search"] = search;
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
