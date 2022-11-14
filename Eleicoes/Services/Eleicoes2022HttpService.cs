using System.Text.Json;
using Eleicoes.Constants;
using Lib.Abstractions;
using Library.Utils.Extensions;

namespace Eleicoes.Services
{
    public class Eleicoes2022HttpService : BaseHttpService<JsonElement>
    {
        public Eleicoes2022HttpService(HttpClient client) : base(client)
        {
            BasePath = ConfigConstants.CRAWL_CITY_URL;
        }

        public override async Task<IEnumerable<JsonElement>> FindAll()
        {
            var response = await _client.GetAsync(BasePath);

            return new List<JsonElement>{ await response.ReadContentAs<JsonElement>() };
        }
    }
}