using Application.ViewModels;
using Domain.Exceptions; 
using Newtonsoft.Json;
using NLog.Fluent;
using System.Net.Http.Headers;
using System.Text;
using WebUI.Helpers.Interface;

namespace WebUI.Helpers
{
    public class ClientHelper : IClientHelper
    {

          
        public async Task<ResponseVM> PostAsync(string BaseUrl, string url, object model)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    string concatUrl = BaseUrl + url;
                    client.BaseAddress = new Uri(BaseUrl);
                    client.DefaultRequestHeaders.Accept.Clear(); 
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client.Timeout = TimeSpan.FromMinutes(5);
                    StringContent content = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");

                    HttpResponseMessage response = await client.PostAsync(concatUrl, content);

                    LoggerMiddleware.LogInfo($"Response Code from url {concatUrl} is {response.StatusCode}");
                    var apiTask = response.Content.ReadAsStringAsync();
                    var responseString = apiTask.Result; 
                    var resp = JsonConvert.DeserializeObject<ResponseVM>(responseString);
                    return resp;

                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
         

        public async Task<ResponseVM> GetAsync(string BaseUrl, string url, string id)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(BaseUrl);
                    client.DefaultRequestHeaders.Accept.Clear(); 
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    string aurl = string.Format(url, id);
                    string concatUrl = BaseUrl + aurl;

                    HttpResponseMessage response = await client.GetAsync(concatUrl);
                    LoggerMiddleware.LogInfo($"Response Code from url {concatUrl} is {response.StatusCode}");
                    var apiTask = response.Content.ReadAsStringAsync();
                    var responseString = apiTask.Result; 
                    var resp = JsonConvert.DeserializeObject<ResponseVM>(responseString);
                    return resp;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }



        public async Task<ResponseVM> PutAsync(string BaseUrl, string url, string id, object model)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(BaseUrl);
                    client.DefaultRequestHeaders.Accept.Clear(); 
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    StringContent content = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");

                    string aurl = string.Format(url, id);
                    string concatUrl = BaseUrl + aurl + id;

                    HttpResponseMessage response = await client.PutAsync(concatUrl, content);
                    LoggerMiddleware.LogInfo($"Response Code from url {concatUrl} is {response.StatusCode}");
                    var apiTask = response.Content.ReadAsStringAsync();
                    var responseString = apiTask.Result; 
                    var resp = JsonConvert.DeserializeObject<ResponseVM>(responseString);
                    return resp;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        

        public async Task<ResponseVM> DeleteAsync(string BaseUrl, string url, string id)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(BaseUrl);
                    client.DefaultRequestHeaders.Accept.Clear(); 
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    string aurl = string.Format(url, id);
                    string concatUrl = BaseUrl + aurl + id;

                    HttpResponseMessage response = await client.DeleteAsync(concatUrl);
                    LoggerMiddleware.LogInfo($"Response Code from url {concatUrl} is {response.StatusCode}");
                    var apiTask = response.Content.ReadAsStringAsync();
                    var responseString = apiTask.Result; 
                    var resp = JsonConvert.DeserializeObject<ResponseVM>(responseString);
                    return resp;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }




    }
}
