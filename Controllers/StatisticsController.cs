using Microsoft.AspNetCore.Mvc;
using ToDo_List_with_additions.Models;
using ToDo_List_with_additions.Services;

namespace ToDo_List_with_additions.Controllers
{
    public class StatisticsController : Controller
    {
        private readonly ILogger<StatisticsController> _logger;
        private readonly IStatisticsService _statisticsService;
        public StatisticsController(ILogger<StatisticsController> logger, IStatisticsService statisticsService)
        {
            _logger = logger;
            _statisticsService = statisticsService;
        }
        public IActionResult Index()
        {
            if (HttpContext.Session.GetString("userId") == null)
            {
                return RedirectToAction("Index", "User");
            }
            var userId = HttpContext.Session.GetString("userId");
            var statistics = _statisticsService.Get(userId);

            var model = new StatisticsModel()
            {
                Done = statistics.Done,
                NotDone = statistics.NotDone,
                Postponed = statistics.Postponed
            };
            
            return View(model);
        }
    }
}
