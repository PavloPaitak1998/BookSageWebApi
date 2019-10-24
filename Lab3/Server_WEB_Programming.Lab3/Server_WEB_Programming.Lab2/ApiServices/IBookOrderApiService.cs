using System.Collections.Generic;
using System.Threading.Tasks;
using Refit;
using Server_WEB_Programming.Lab2.Dal.Entities;

namespace Server_WEB_Programming.Lab2.ApiServices
{
    public interface IBookOrderApiService
    {
        [Get("/api/BookOrders")]
        Task<IReadOnlyCollection<BookOrder>> GetAsync();

        [Post("/api/BookOrders")]
        Task PostAsync([Body(BodySerializationMethod.Serialized)]IEnumerable<BookOrder> bookOrder);
    }
}
