using Microsoft.AspNetCore.Mvc;
using TesiMagistraleLM32.Models;
using System.Diagnostics;
using System.Text.Json;

namespace TesiMagistraleLM32.Controllers
{
    public class UtilityController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private static readonly HttpClient client = new HttpClient();

        public UtilityController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }
        


    }
}