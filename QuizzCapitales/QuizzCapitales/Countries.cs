using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace QuizzCapitales
{
    internal class Countries
    {
        private const string apiUrl = "https://restcountries.com/v3.1/all";

        public async Task<(string[], string[])> GetCountriesAndCapitals()
        {
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    string response = await client.GetStringAsync(apiUrl);

                    if (!string.IsNullOrEmpty(response))
                    {
                        var countries = JsonSerializer.Deserialize<List<Country>>(response);

                        if (countries != null && countries.Count > 0)
                        {
                            List<string> countryNames = new List<string>();
                            List<string> capitals = new List<string>();

                            foreach (var country in countries)
                            {
                                if (country.Name != null && country.Capital != null && country.Capital.Count > 0)
                                {
                                    string countryName = country.Name.Common;
                                    string capital = country.Capital[0];

                                    if (!string.IsNullOrEmpty(countryName) && !string.IsNullOrEmpty(capital))
                                    {
                                        countryNames.Add(countryName);
                                        capitals.Add(capital);
                                    }
                                }
                            }

                            return (countryNames.ToArray(), capitals.ToArray());
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Une erreur s'est produite lors de la récupération des données : " + ex.Message);
                }
            }

            return (new string[0], new string[0]);
        }
    }
    public class Country
    {
        public CommonName Name { get; set; }
        public List<string> Capital { get; set; }
    }

    public class CommonName
    {
        public string Common { get; set; }
    }
}
