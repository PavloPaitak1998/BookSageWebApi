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

namespace Server_WEB_Programming.Lab3.Controllers
{
    public class BookOrdersController : ApiController
    {
        private readonly IUnitOfWork _uow;

        public BookOrdersController(IUnitOfWork uow)
        {
            _uow = uow;
        }

        // GET: BookOrders
        public async Task<IHttpActionResult> Get()
        {
            return Ok(await _uow.BookOrderRepository.GetAllAsync(include: q => q.Include(x => x.Book.Sages)));
        }

        // POST api/values
        public async Task<IHttpActionResult> Post([FromBody]IEnumerable<BookOrder> bookOrders)
        {
            foreach (var item in bookOrders)
            {
                await _uow.BookOrderRepository.CreateAsync(item);
            }

            if (await _uow.SaveAsync())
            {
                return Ok();
            }

            return BadRequest();
        }
    }
}
