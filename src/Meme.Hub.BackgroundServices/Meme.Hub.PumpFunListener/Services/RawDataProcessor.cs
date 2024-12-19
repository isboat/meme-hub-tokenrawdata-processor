namespace Meme.Hub.TokenRawDataProcessor.Services
{
    public interface IRawDataProcessor
    {
        Task ProcessTokenAsync(string tokenMessage);
    }

    public class RawDataProcessor : IRawDataProcessor
    {
        public Task ProcessTokenAsync(string tokenMessage)
        {
            throw new NotImplementedException();
        }
    }
}
