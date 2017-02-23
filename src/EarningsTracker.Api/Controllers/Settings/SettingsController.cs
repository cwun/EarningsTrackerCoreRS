using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using EarningsTracker.Services.Settings;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace EarningsTracker.Api.Controllers.Settings
{
    [Route("api/[controller]/{officeId:int}")]
    public class SettingsController : Controller
    {
        private readonly ISettingService _service;

        public SettingsController(ISettingService service)
        {
            _service = service;
        }

        // GET: api/settings/1
        [HttpGet]
        public async Task<IActionResult> GetIncomeByOfficeId(int officeId)
        {
            var result = await _service.GetIncomePerOfficeAsync(officeId);
            return Ok(result);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateIncomeByOfficeId(int officeId, [FromBody]Setting item)
        {
            var result = await _service.UpdateIncomePerOfficeAsync(item);

            if (result == 0)
            {
                throw new Exception("Item cannot be updated.");
            }
            return Ok(result);
        }
    }
}
