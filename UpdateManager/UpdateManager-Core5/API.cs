using System.Net.Http;
using System.Threading.Tasks;

namespace UpdateManager_Core
{
    public class API
    {
        public async Task<string> Send(string url)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Add("User-Agent", "request");
                    return await client.GetStringAsync(url);
                }
            }
            catch { return null; }
        }

        public async Task<T> Request<T>(string url)
        {
            var responseText = await Send(url);
            if (responseText == null) return default;

            return responseText.Deserialize<T>();
        }
    }
}
