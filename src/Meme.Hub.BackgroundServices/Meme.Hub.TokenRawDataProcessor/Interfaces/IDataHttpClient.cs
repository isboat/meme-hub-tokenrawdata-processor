namespace Meme.Hub.TokenRawDataProcessor.Interfaces
{
    public interface IDataHttpClient
    {
        Task<T> GetData<T>(string url);
    }
}
