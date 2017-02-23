using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using EarningsTracker.Services.Dashboards;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace EarningsTracker.Api.Controllers.Dashboards
{
    [Route("api/[controller]")]
    public class DashboardsController : Controller
    {
        private readonly IDashboardService _service;

        public DashboardsController(IDashboardService service)
        {
            _service = service;
        }

        // GET: api/dashboards
        [HttpGet]
        public async Task<IActionResult> GetDashboardSetting()
        {
            var result = await _service.GetDashboardSettingAsync();
            return Ok(result);
        }
    }
}
