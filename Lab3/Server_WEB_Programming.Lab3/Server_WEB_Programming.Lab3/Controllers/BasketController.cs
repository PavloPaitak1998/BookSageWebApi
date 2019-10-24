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
    public class BasketController : ApiController
    {
        private readonly IUnitOfWork _uow;

        public BasketController(IUnitOfWork uow)
        {
            _uow = uow;
        }

        [HttpPost]
        [ActionName("complete-order")]
        public async Task<IHttpActionResult> CompleteOrder(IEnumerable<BookOrder> bookOrders)
        {
            if (bookOrders != null)
            {
                foreach (var item in bookOrders)
                {
                    await _uow.BookOrderRepository.CreateAsync(item);
                }

                await _uow.SaveAsync();
            }

            return Ok();
        }
    }
}
