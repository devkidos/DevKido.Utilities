using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevKido.Utilities.HtttpServices
{
    public interface IHttpClientService
    {
        T Get<T>(string apiUrl, string token);
        T Put<T>(string apiUrl, T model, string token);
        T Post<T>(string apiUrl, ExpandoObject dynamicObject, string token);
        T PostFormUrlEncoded<T>(string apiUrl, Dictionary<string, string> httpHeaderParams);
        T GetUserAccessToken<T>(string apiUrl, string username, string password); 

        Task<T> GetAsync<T>(string apiUrl, string token);
        Task<T> PutAsync<T>(string apiUrl, T model, string token);
        Task<T> PostAsync<T>(string apiUrl, ExpandoObject dynamicObject, string token);
        Task<T> PostFormUrlEncodedAsync<T>(string apiUrl, Dictionary<string, string> httpHeaderParams);
        Task<T> GetUserAccessTokenAsync<T>(string apiUrl, string username, string password);

    }
}
