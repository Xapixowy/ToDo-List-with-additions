using Microsoft.AspNetCore.Mvc;
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
            return View();
        }
    }
}
