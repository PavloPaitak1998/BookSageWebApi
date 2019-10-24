using System.Collections.Generic;
using System.Threading.Tasks;
using Refit;

using Server_WEB_Programming.Lab2.Dal.Entities;
using Server_WEB_Programming.Lab2.ViewModels;

namespace Server_WEB_Programming.Lab2.ApiServices
{
    public interface IBookApiService
    {
        [Get("/api/Books")]
        Task<IReadOnlyCollection<Book>> GetAsync();

        [Get("/api/Books/{id}")]
        Task<Book> GetAsync(int id);

        [Post("/api/Books")]
        Task PostAsync([Body(BodySerializationMethod.Serialized)]BookCreateViewModel book, [Header("Authorization")] string authorization);

        [Put("/api/Books/{id}")]
        Task PutAsync(int id, [Body(BodySerializationMethod.Serialized)]BookUpdateViewModel book, [Header("Authorization")] string authorization);

        [Delete("/api/Books/{id}")]
        Task DeleteAsync(int id, [Header("Authorization")] string authorization);
    }
}
