using Microsoft.AspNetCore.Mvc;
using ToDo_List_with_additions.Services;

namespace ToDo_List_with_additions.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class TestController : ControllerBase
    {
        [HttpGet]
        public async void GetData()
        {
            var test = new Mongo();
            test.MainAsync();
        }
    }
}
