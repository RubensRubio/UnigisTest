using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using UnigisTest.Models;

namespace UnigisTest
{
    public class UnigisService
    {
        #region "Singleton"

        private static UnigisService instance = null;

        private static object mutex = new object();
        private UnigisService()
        {
        }
        public static UnigisService GetInstance()
        {

            if (instance == null)
            {
                lock ((mutex))
                {
                    instance = new UnigisService();
                }
            }

            return instance;

        }

        #endregion
        public async Task<ResponseAPI> Get(string endPoint)
        {

            using (HttpClient client = new HttpClient())
            {
                ResponseAPI? responseAPI = null;

                try
                {
                    string serializedResponse = "";

                    string restURL = $"https://dog.ceo/api/breed/hound/{endPoint}";
                    client.BaseAddress = new Uri(restURL);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    HttpResponseMessage response = await client.GetAsync(restURL);

                    if (response.IsSuccessStatusCode)
                    {
                        serializedResponse = await response.Content.ReadAsStringAsync();
                        responseAPI = JsonConvert.DeserializeObject<ResponseAPI>(serializedResponse);
                    }
                    else
                    {
                        UnigisLog.GetInstance().Log("Error","Get", response.ToString());
                        UnigisLog.GetInstance().Log("Error","Get", response.RequestMessage!.ToString());
                    }
                }
                catch (Exception ex)
                {
                    UnigisLog.GetInstance().Log("Error","Get", ex.Message);
                }

                return responseAPI!;
            }
        }
    }
}
