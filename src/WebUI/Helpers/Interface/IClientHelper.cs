using Application.ViewModels;

namespace WebUI.Helpers.Interface
{
    public interface IClientHelper
    {
        Task<ResponseVM> PostAsync(string BaseUrl, string url, object model);
        Task<ResponseVM> PutAsync(string BaseUrl, string url, string id, object model);
        Task<ResponseVM> GetAsync(string BaseUrl, string url, string id);
        Task<ResponseVM> DeleteAsync(string BaseUrl, string url, string id);
    }
}
