using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using HttpApi;

namespace WebApplication2.Pages
{
    public class IndexModel : PageModel
    {
        public int Interval { get; set; }
        public int Count { get; set; }

        public async Task OnGetAsync()
        {
            var httpApi = new HttpApiTest();
            var result = await httpApi.FetchDataAsync();

            Interval = result.Item1;
            Count = result.Item2;
        }
    }
}

namespace HttpApi
{
    public class HttpApiTest
    {
        public async Task<(int, int)> FetchDataAsync()
        {
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    const string apiUrl = "https://webmap.lastcountryrp.fr/list.json";

                    HttpResponseMessage response = await client.GetAsync(apiUrl);

                    if (response.IsSuccessStatusCode)
                    {
                        using (JsonDocument doc = await JsonDocument.ParseAsync(await response.Content.ReadAsStreamAsync()))
                        {
                            int interval = (doc.RootElement.GetProperty("interval").GetInt32() != null) ? doc.RootElement.GetProperty("interval").GetInt32() : 0;
                            int count = (doc.RootElement.GetProperty("count").GetInt32() != null) ? doc.RootElement.GetProperty("count").GetInt32() : 0;

                            return (interval, count);
                        }
                    }
                    else
                    {
                        throw new Exception($"Erreur : {response.StatusCode}");
                        
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception($"Erreur : {ex.Message}");
                }
            }
        }
    }
}
