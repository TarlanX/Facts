using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Threading.Tasks;
using MediatR;
using Teagle.Facts.Web.Mediatr;
using Teagle.Facts.Web.ViewModels;

namespace Teagle.Facts.Web.Controllers
{
    public class SiteController : Controller
    {
        private readonly IMediator _mediator;

        public SiteController(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<IActionResult> Privacy()
        {
           await _mediator.Publish(new ErrorNotification("Privacy test for notification"), HttpContext.RequestAborted);

            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
