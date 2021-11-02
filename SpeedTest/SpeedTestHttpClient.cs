using System;
using System.Globalization;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Xml.Serialization;

namespace SpeedTest
{
    internal class SpeedTestHttpClient : HttpClient
    {
        public int ConnectionLimit { get; set; }

        public SpeedTestHttpClient()
        {
            DefaultRequestHeaders.Add("Accept", "text/html, application/xhtml+xml, */*");
            DefaultRequestHeaders.Add("User-Agent", "(KHTML, like Gecko)");
        }

        public async Task<T> GetConfig<T>(string url)
        {
            var data = await GetStringAsync(AddTimeStamp(new Uri(url)));
            var xmlSerializer = new XmlSerializer(typeof(T));
            using var reader = new StringReader(data);
            return (T)xmlSerializer.Deserialize(reader);
        }

        private static Uri AddTimeStamp(Uri address)
        {
            var uriBuilder = new UriBuilder(address);
            var query = HttpUtility.ParseQueryString(uriBuilder.Query);
            query["x"] = DateTime.Now.ToFileTime().ToString(CultureInfo.InvariantCulture);
            uriBuilder.Query = query.ToString();
            return uriBuilder.Uri;
        }
    }
}
