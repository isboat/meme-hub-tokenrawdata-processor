using Microsoft.AspNetCore.OpenApi;
using Microsoft.AspNetCore.Http.HttpResults;
namespace Meme.Hub.TokenRawDataProcessor.Interfaces
{
    public interface ICacheService
    {
        Task AddItemToList(string item, TimeSpan expiration);

        Task<List<string>> GetItemsFromList();

        Task RemoveExpiredItemsAsync();

        T? GetData<T>(string key);

        /// <summary>
        /// Set Data with Value and Expiration Time of Key
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="expirationTime"></param>
        /// <returns></returns>
        bool SetData<T>(string key, T value, DateTimeOffset expirationTime);

        /// <summary>
        /// Remove Data
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        bool RemoveData(string key);
    }
}