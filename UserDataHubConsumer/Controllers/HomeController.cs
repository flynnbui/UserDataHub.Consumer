using Microsoft.AspNetCore.Mvc;
using UserDataHub.Consumer.Core.Services;

namespace UserDataHubConsumer.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class HomeController : Controller
    {
        private readonly ConsumerHostedService consumerHostedService;

        public HomeController(ConsumerHostedService consumerHostedService)
        {
            this.consumerHostedService = consumerHostedService;
        }

        [HttpGet]
        public List<string> Index()
        {
            return consumerHostedService.getAllMessage();   
        }
    }
}
