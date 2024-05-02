using GraphAPI_WebApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Graph;
using System.Diagnostics;
using User = GraphAPI_WebApp.Models.User;

namespace GraphAPI_WebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly GraphServiceClient _graphServiceClient;

        public HomeController(ILogger<HomeController> logger, GraphServiceClient graphServiceClient)
        {
            _logger = logger;
            _graphServiceClient = graphServiceClient;
        }

        public IActionResult Index()
        {
            var result = _graphServiceClient.Users.Request().Select(x =>
            new
            {
                x.Id,
                x.DisplayName,
                x.Mail,
            }
            ).GetAsync().Result;

            List<User> users = new List<User>();
            foreach (var user in result)
            {
                users.Add(new User(user.Id, user.DisplayName, user.Mail));
            }
            return View(users);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
