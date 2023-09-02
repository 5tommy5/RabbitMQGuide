namespace Producer.Api.Interfaces
{
    public interface IRabbitService
    {
        void SendMessage<T> (T message);
    }
}
