namespace HU.Inno.Teachers.Messaging;

public interface IPublisher<T>
{
    void Publish(T message);
}