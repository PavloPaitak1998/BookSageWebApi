using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

using Server_WEB_Programming.Lab2.Dal.Entities;
using Server_WEB_Programming.Lab2.Dal.UoW.Interfaces;
using Server_WEB_Programming.Lab3.ViewModels;

using WebGrease.Css.Extensions;

namespace Server_WEB_Programming.Lab3.Controllers
{
    public class SagesController : ApiController
    {
        private readonly IUnitOfWork _uow;

        public SagesController(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<IHttpActionResult> Get()
        {
            var res = await _uow.SageRepository.GetAllAsync(null, null, q => q.Include(x => x.Books));
            return Ok(res);
        }

        // GET api/values/5
        public async Task<IHttpActionResult> Get(int id)
        {
            var res = await _uow.SageRepository.GetFirstOrDefaultAsync(
                          x => x.IdSage == id,
                          null,
                          q => q.Include(x => x.Books));
            return Ok(res);
        }

        // POST api/values
        public async Task<IHttpActionResult> Post([FromBody]SageCreateViewModel sageViewModel)
        {
            var selectedBooks = new HashSet<int>(sageViewModel.SelectedBooks);

            var books = await _uow.BookRepository.GetAllAsync(filter: x => selectedBooks.Contains(x.IdBook), disableTracking: false);

            sageViewModel.Sage.Books = books.ToList();

            await _uow.SageRepository.CreateAsync(sageViewModel.Sage);

            if (await _uow.SaveAsync())
            {
                return Ok();
            }

            return BadRequest();
        }

        // PUT api/values/5
        public async Task<IHttpActionResult> Put(int id, [FromBody]SageUpdateViewModel sageViewModel)
        {
            try
            {
                var sageToUpdate = await _uow.SageRepository.GetFirstOrDefaultAsync(
                                       x => x.IdSage == id,
                                       null,
                                       q => q.Include(x => x.Books),
                                       disableTracking: false);

                if (sageToUpdate == null)
                {
                    return NotFound();
                }

                sageToUpdate.Age = sageViewModel.Sage.Age;
                sageToUpdate.Name = sageViewModel.Sage.Name;
                sageToUpdate.City = sageViewModel.Sage.City;
                sageToUpdate.Photo = sageViewModel.Sage.Photo;

                var selectedBooks = new HashSet<int>(sageViewModel.SelectedBooks);

                sageToUpdate.Books
                    .Where(x => !selectedBooks.Contains(x.IdBook))
                    .ToList()
                    .ForEach(item => sageToUpdate.Books.Remove(item));

                var existingBooks = new HashSet<int>(sageToUpdate.Books.Select(c => c.IdBook));


                var books = await _uow.BookRepository.GetAllAsync(x => selectedBooks.Except(existingBooks).Contains(x.IdBook), disableTracking: false);

                books.ForEach(book => sageToUpdate.Books.Add(book));

                await _uow.SageRepository.UpdateAsync(sageToUpdate);

                if (await _uow.SaveAsync())
                {
                    return Ok();
                }

                return BadRequest();

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        // DELETE api/values/5
        public async Task<IHttpActionResult> Delete(int id)
        {
            await _uow.SageRepository.DeleteAsync(id);

            if (await _uow.SaveAsync())
            {
                return Ok();
            }

            return BadRequest();
        }
    }
}
