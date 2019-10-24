using System.Collections.Generic;
using System.Threading.Tasks;
using Refit;
using Server_WEB_Programming.Lab2.Dal.Entities;
using Server_WEB_Programming.Lab2.ViewModels;

namespace Server_WEB_Programming.Lab2.ApiServices
{
    public interface ISageApiService
    {
        [Get("/api/Sages")]
        Task<IReadOnlyCollection<Sage>> GetAsync();

        [Get("/api/Sages/{id}")]
        Task<Sage> GetAsync(int id);

        [Post("/api/Sages")]
        Task PostAsync([Body(BodySerializationMethod.Serialized)]SageCreateViewModel sage);

        [Put("/api/Sages/{id}")]
        Task PutAsync(int id, [Body(BodySerializationMethod.Serialized)]SageUpdateViewModel sage);

        [Delete("/api/Sages/{id}")]
        Task DeleteAsync(int id);
    }
}
