namespace UserDataHub.Consumer.Core.Settings;
public class RabbitMQSettings
{
    public string RouteKey {get; set;}
    public List<string> QueueList { get; set; }
}