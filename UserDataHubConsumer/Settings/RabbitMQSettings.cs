using System.ComponentModel.DataAnnotations;

namespace UserDataHub.WebAPI.Settings;
public class RabbitMQSettings
{
    [Required]
    public string HostName { get; set; } = "localhost";
    [Required]
    public int Port { get; set; } = 5672;
    [Required]
    public string UserName { get; set; } = "guest";
    [Required]
    public string Password { get; set; } = "guest";
    [Required]
    public string VirtualHost { get; set; } = "/";
}