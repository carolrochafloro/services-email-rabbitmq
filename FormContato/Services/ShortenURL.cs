using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text;

namespace FormContato.Services;

// testar em prod (coragem)
public class ShortenURL
{

    public string Endpoint = "v4/shorten";

    public string URL = "https://api-ssl.bitly.com/";

    public async Task<string> GetShortUrl(string longUrl)
    {
        var client = new HttpClient();
        client.BaseAddress = new Uri(URL);
        client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue
                                                      ("Bearer", Environment.GetEnvironmentVariable("BITLY_TOKEN"));
        var body = new 
        {
            long_url = longUrl,
        };

        var jsonBody = JsonConvert.SerializeObject(body);
        var httpContent = new StringContent(jsonBody, Encoding.UTF8, "application/json");
        Console.WriteLine($"HTTP Content: {httpContent}");

        try
        {
            var response = await client.PostAsync(Endpoint, httpContent);
            var content = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"Content: {content}");

            if (response.IsSuccessStatusCode)
            {
                var responseObject = JObject.Parse(content);

                string bitlink = responseObject["deeplinks"][0]["bitlink"].ToString();

                return bitlink;

            }

            return Environment.GetEnvironmentVariable("BITLY_FAIL_RESPONSE");

        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());   
            return Environment.GetEnvironmentVariable("BITLY_FAIL_RESPONSE"); ;
        }
    }
}
