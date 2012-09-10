using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;

namespace W8_Meetup_App.Common
{
    class HttpGet
    {
        string Url;

        public HttpGet(string Url)
        {
            this.Url = Url;
        }

        public async Task<string> Fetch()
        {
            try
            {
                HttpClient HC = new HttpClient();
                HttpRequestMessage HRM = new HttpRequestMessage();
                HRM.Method = new HttpMethod("GET");
                HRM.RequestUri = new Uri(this.Url);
                var response = await HC.SendAsync(HRM);
                var RespText = await response.Content.ReadAsStringAsync();
                return RespText;
            }
            catch (Exception Err)
            {
                throw Err;
            }
        }
    }
}
