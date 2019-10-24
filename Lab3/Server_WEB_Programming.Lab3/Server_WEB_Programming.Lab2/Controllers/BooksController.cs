using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web.Mvc;

using Refit;

using Server_WEB_Programming.Lab2.ApiServices;
using Server_WEB_Programming.Lab2.Dal.DataBase;
using Server_WEB_Programming.Lab2.Dal.Entities;
using Server_WEB_Programming.Lab2.ViewModels;
using Server_WEB_Programming.Lab2.Dal.UoW.Interfaces;

namespace Server_WEB_Programming.Lab2.Controllers
{
    public class BooksController : Controller
    {
        private readonly IBookApiService _bookApiService;
        private readonly ISageApiService _sageApiService;

        public BooksController(IBookApiService bookApiService, ISageApiService sageApiService)
        {
            _bookApiService = bookApiService;
            _sageApiService = sageApiService;
        }

        // GET: Books
        public async Task<ActionResult> Index()
        {
            var books = await _bookApiService.GetAsync();

            return View(books);
        }

        // GET: Books/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            // Book book = await db.Books.FindAsync(id);
            var book = await _bookApiService.GetAsync(id.Value);

            if (book == null)
            {
                return HttpNotFound();
            }

            return View(book);
        }

        [System.Web.Mvc.Authorize(Roles = "admin")]
        // GET: Books/Create
        public async Task<ActionResult> Create()
        {
            var bookViewModel = new BookViewModel();

            var sages = await _sageApiService.GetAsync();

            bookViewModel.AllSages = sages
                .Select(x => new SelectListItem
                {
                    Value = x.IdSage.ToString(),
                    Text = x.Name
                })
                .ToList();

            return View(bookViewModel);
        }

        [System.Web.Mvc.Authorize(Roles = "admin")]

        // POST: Books/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(BookViewModel bookViewModel)
        {
            if (ModelState.IsValid)
            {
                var book = new BookCreateViewModel
                {
                    Book = bookViewModel.Book,
                    SelectedSages = bookViewModel.SelectedSages
                };

                await _bookApiService.PostAsync(book, "Bearer " + Session["ApiAccessToken"]);
                return RedirectToAction("Index");
            }

            return View(bookViewModel);
        }

        [System.Web.Mvc.Authorize(Roles = "admin")]

        // GET: Books/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            // Book book = await db.Books.FindAsync(id);
            var bookViewModel = new BookViewModel
            {
                Book = await _bookApiService.GetAsync(id.Value)
            };

            if (bookViewModel.Book == null)
            {
                return HttpNotFound();
            }

            var sages = await _sageApiService.GetAsync();

            bookViewModel.AllSages = sages
                .Select(x => new SelectListItem
                             {
                                Value = x.IdSage.ToString(),
                                Text = x.Name
                             })
                .ToList();

            return View(bookViewModel);
        }

        [System.Web.Mvc.Authorize(Roles = "admin")]

        // POST: Books/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(BookViewModel bookViewModel)
        {
            if (ModelState.IsValid)
            {
                var book = new BookUpdateViewModel
                {
                   Book = bookViewModel.Book,
                   SelectedSages = bookViewModel.SelectedSages
                };

                await _bookApiService.PutAsync(book.Book.IdBook, book, "Bearer " + Session["ApiAccessToken"]);

                return RedirectToAction("Index");
            }

            return View(bookViewModel);
        }

        [System.Web.Mvc.Authorize(Roles = "admin")]

        // GET: Books/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var book = await _bookApiService.GetAsync(id.Value);

            if (book == null)
            {
                return HttpNotFound();
            }

            return View(book);
        }

        [System.Web.Mvc.Authorize(Roles = "admin")]

        // POST: Books/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            await _bookApiService.DeleteAsync(id, "Bearer " + Session["ApiAccessToken"]);

            return RedirectToAction("Index");
        }
    }
}
