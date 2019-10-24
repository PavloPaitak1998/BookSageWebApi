﻿using System;
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

namespace Server_WEB_Programming.Lab3.Controllers
{
    public class BooksController : ApiController
    {
        private readonly IUnitOfWork _uow;

        public BooksController(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<IHttpActionResult> Get()
        {
            var res = await _uow.BookRepository.GetAllAsync(null, null, q => q.Include(x => x.Sages));
            return Ok(res);
        }

        // GET api/values/5
        public async Task<IHttpActionResult> Get(int id)
        {
            var res = await _uow.BookRepository.GetFirstOrDefaultAsync(
                          x => x.IdBook == id,
                          null,
                          q => q.Include(x => x.Sages));
            return Ok(res);
        }

        [Authorize]
        // POST api/values
        public async Task<IHttpActionResult> Post([FromBody]BookCreateViewModel bookViewModel)
        {
            var selectedSages = new HashSet<int>(bookViewModel.SelectedSages);

            var sages = await _uow.SageRepository.GetAllAsync(filter: x => selectedSages.Contains(x.IdSage), disableTracking: false);

            bookViewModel.Book.Sages = sages.ToList();

            await _uow.BookRepository.CreateAsync(bookViewModel.Book);

            if (await _uow.SaveAsync())
            {
                return Ok();
            }

            return BadRequest();
        }

        // PUT api/values/5
        public async Task<IHttpActionResult> Put(int id, [FromBody]BookUpdateViewModel book)
        {
            var bookToUpdate = await _uow.BookRepository
                                   .GetFirstOrDefaultAsync(
                                       x => x.IdBook.Equals(id),
                                       null,
                                       q => q.Include(x => x.Sages),
                                       disableTracking: false);

            if (bookToUpdate == null)
            {
                return NotFound();
            }

            bookToUpdate.Name = book.Book.Name;
            bookToUpdate.Description = book.Book.Description;

            var selectedSages = new HashSet<int>(book.SelectedSages);
            var bookSages = new HashSet<int>(bookToUpdate.Sages.Select(c => c.IdSage));

            var sages = await _uow.SageRepository.GetAllAsync(disableTracking: false);

            foreach (var sage in sages)
            {
                if (selectedSages.Contains(sage.IdSage))
                {
                    if (!bookSages.Contains(sage.IdSage))
                    {
                        bookToUpdate.Sages.Add(sage);
                    }
                }
                else
                {
                    if (bookSages.Contains(sage.IdSage))
                    {
                        bookToUpdate.Sages.Remove(sage);
                    }
                }
            }

            await _uow.BookRepository.UpdateAsync(bookToUpdate);

            if (await _uow.SaveAsync())
            {
                return Ok();
            }

            return BadRequest();
        }

        // DELETE api/values/5
        public async Task<IHttpActionResult> Delete(int id)
        {
            await _uow.BookRepository.DeleteAsync(id);

            if (await _uow.SaveAsync())
            {
                return Ok();
            }

            return BadRequest();
        }
    }
}
