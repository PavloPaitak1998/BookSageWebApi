using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web.Mvc;
using Server_WEB_Programming.Lab2.Dal.DataBase;
using Server_WEB_Programming.Lab2.Dal.Entities;
using System.Web;

using Server_WEB_Programming.Lab2.ApiServices;
using Server_WEB_Programming.Lab2.Dal.UoW.Interfaces;
using Server_WEB_Programming.Lab2.ViewModels;

namespace Server_WEB_Programming.Lab2.Controllers
{
    public class SagesController : Controller
    {
        private readonly IBookApiService _bookApiService;
        private readonly ISageApiService _sageApiService;

        public SagesController(IBookApiService bookApiService, ISageApiService sageApiService)
        {
            _bookApiService = bookApiService;
            _sageApiService = sageApiService;
        }

        // GET: Sages
        public async Task<ActionResult> Index()
        {
            var sages = await this._sageApiService.GetAsync();

            return View(sages);
        }

        // GET: Sages/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            // Book book = await db.Books.FindAsync(id);
            var sage = await this._sageApiService.GetAsync(id.Value);

            if (sage == null)
            {
                return HttpNotFound();
            }

            return View(sage);
        }

        [Authorize(Roles = "admin")]
        // GET: Sages/Create
        public async Task<ActionResult> Create()
        {
            var sageViewModel = new SageViewModel();

            var books = await _bookApiService.GetAsync();

            sageViewModel.AllBooks = books
                .Select(x => new SelectListItem
                {
                    Value = x.IdBook.ToString(),
                    Text = x.Name
                })
                .ToList();

            return View(sageViewModel);
        }

        [Authorize(Roles = "admin")]

        // POST: Sages/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(SageViewModel sageViewModel, HttpPostedFileBase photo1)
        {
            if (ModelState.IsValid)
            {
                if (photo1 != null)
                {
                    sageViewModel.Sage.Photo = new byte[photo1.ContentLength];
                    photo1.InputStream.Read(sageViewModel.Sage.Photo, 0, photo1.ContentLength);
                }
                var sage = new SageCreateViewModel 
                {
                   Sage = sageViewModel.Sage,
                   SelectedBooks = sageViewModel.SelectedBooks
                };

                await this._sageApiService.PostAsync(sage);
                return RedirectToAction("Index");
            }

            return View(sageViewModel);
        }

        [Authorize(Roles = "admin")]
        // GET: Sages/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            // Book book = await db.Books.FindAsync(id);
            var sageViewModel = new SageViewModel()
            {
                Sage = await _sageApiService.GetAsync(id.Value)
            };

            if (sageViewModel.Sage == null)
            {
                return HttpNotFound();
            }

            var books = await _bookApiService.GetAsync();

            sageViewModel.AllBooks = books
                .Select(x => new SelectListItem
                             {
                                 Value = x.IdBook.ToString(),
                                 Text = x.Name
                             })
                .ToList();

            return View(sageViewModel);
        }

        [Authorize(Roles = "admin")]

        // POST: Sages/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(SageViewModel sageViewModel, HttpPostedFileBase photo1)
        {
            if (ModelState.IsValid)
            {
                if (photo1 != null)
                {
                    sageViewModel.Sage.Photo = new byte[photo1.ContentLength];
                    photo1.InputStream.Read(sageViewModel.Sage.Photo, 0, photo1.ContentLength);
                }

                var sage = new SageUpdateViewModel
                {
                   Sage = sageViewModel.Sage,
                   SelectedBooks = sageViewModel.SelectedBooks
                };

                await _sageApiService.PutAsync(sage.Sage.IdSage, sage);

                return RedirectToAction("Index");
            }

            return View(sageViewModel);
        }

        [Authorize(Roles = "admin")]

        // GET: Sages/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Sage sage = await _sageApiService.GetAsync(id.Value);

            if (sage == null)
            {
                return HttpNotFound();
            }

            return View(sage);
        }

        [Authorize(Roles = "admin")]

        // POST: Sages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            await _sageApiService.DeleteAsync(id);

            return RedirectToAction("Index");
        }
    }
}
