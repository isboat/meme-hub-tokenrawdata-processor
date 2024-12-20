using Meme.Hub.TokenRawDataProcessor.Constants;
using Meme.Hub.TokenRawDataProcessor.Interfaces;
using Meme.Hub.TokenRawDataProcessor.Models;
using Newtonsoft.Json;

namespace Meme.Hub.TokenRawDataProcessor.Services
{

    public class RawDataProcessor : IRawDataProcessor
    {
        private readonly IDataHttpClient _httpClient;
        private readonly ICacheService _cacheService;

        public RawDataProcessor(IDataHttpClient httpClient, ICacheService cacheService)
        {
            _httpClient = httpClient;
            _cacheService = cacheService;
        }

        public async Task ProcessTokenAsync(string rawTokenData)
        {
            var rawDataModel = JsonConvert.DeserializeObject<RawTokenDataModel>(rawTokenData);

            if (rawDataModel?.TxType != TokenTransactionType.Create) return;
            
            if (string.IsNullOrEmpty(rawDataModel?.Uri))
            {
                // log error message
                return;
            }

            var tokenData = await _httpClient.GetData<TokenDataModel>(rawDataModel.Uri);
            if (tokenData == null)
            {
                // log error message
                return;
            }

            tokenData.RawData = rawDataModel;
            _cacheService.SetData()
        }
    }
}
