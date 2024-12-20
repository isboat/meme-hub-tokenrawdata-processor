using Meme.Hub.TokenRawDataProcessor.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Meme.Hub.TokenRawDataProcessor
{
    [ApiController]
    [Route("[controller]")]
    public class CacheController : ControllerBase
    {
        private readonly ICacheService _cacheService;

        public CacheController(ICacheService cacheService)
        {
            _cacheService = cacheService;
        }


        [HttpDelete("purge")]
        public async Task Purge()
        {
            await _cacheService.RemoveExpiredItemsAsync();
        }
    }
}
