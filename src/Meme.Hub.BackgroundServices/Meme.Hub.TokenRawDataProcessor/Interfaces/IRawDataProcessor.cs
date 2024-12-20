namespace Meme.Hub.TokenRawDataProcessor.Interfaces
{
    public interface IRawDataProcessor
    {
        Task ProcessTokenAsync(string rawTokenData);
    }
}
