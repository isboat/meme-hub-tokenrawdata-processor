using Meme.Hub.TokenRawDataProcessor.Interfaces;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace Meme.Hub.TokenRawDataProcessor.Services
{
    public class RedisCacheService: ICacheService
    {
        private readonly IDatabase _db; 
        private readonly string _sortedSetKey = "mySortedSet";

        public RedisCacheService(IConnectionMultiplexer redis)
        {
            _db = redis.GetDatabase();
        }
        public async Task AddItemToList(string item, TimeSpan expiration) 
        {
            double score = DateTimeOffset.UtcNow.ToUnixTimeSeconds() + expiration.TotalSeconds; 
            await _db.SortedSetAddAsync(_sortedSetKey, item, score);
        }

        public async Task<List<string>> GetItemsFromList() 
        {
            double currentTimestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds(); 
            var items = await _db.SortedSetRangeByScoreAsync(_sortedSetKey, 0, currentTimestamp, Exclude.Stop); 
            List<string> itemList = new List<string>(); 
            foreach (var item in items) { itemList.Add(item.ToString()); }

            return itemList;
        }

        public async Task RemoveExpiredItemsAsync() 
        { 
            double currentTimestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds(); 
            long removedCount = await _db.SortedSetRemoveRangeByScoreAsync(_sortedSetKey, 0, currentTimestamp); 
            Console.WriteLine($"Removed {removedCount} expired items."); 
        }

        public T? GetData<T>(string key)
        {
            var value = _db.StringGet(key);
            if (!string.IsNullOrEmpty(value))
            {
                return JsonConvert.DeserializeObject<T>(value!);
            }
            return default;
        }
        public bool SetData<T>(string key, T value, DateTimeOffset expirationTime)
        {
            var expiryTime = expirationTime.DateTime.Subtract(DateTime.Now);
            var isSet = _db.StringSet(key, JsonConvert.SerializeObject(value), expiryTime);
            return isSet;
        }

        public bool RemoveData(string key)
        {
            var _isKeyExist = _db.KeyExists(key);
            if (_isKeyExist)
            {
                return _db.KeyDelete(key);
            }
            return false;
        }
    }
}
