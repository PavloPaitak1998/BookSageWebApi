using System.Threading.Tasks;
using System.Web.Mvc;
using Server_WEB_Programming.Lab2.ApiServices;

namespace Server_WEB_Programming.Lab2.Controllers
{
    [Authorize(Roles = "admin")]
    public class BookOrdersController : Controller
    {
        private readonly IBookOrderApiService _bookOrderApiService;

        public BookOrdersController(IBookOrderApiService bookOrderApiService)
        {
            _bookOrderApiService = bookOrderApiService;
        }

        // GET: BookOrders
        public async Task<ActionResult> Index()
        {
            return View(await _bookOrderApiService.GetAsync());
        }
    }
}