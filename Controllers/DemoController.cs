using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace Demo.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DemoController : ControllerBase
    {
        /*
         * An endpoint that enforces a required scope
         */
        [HttpGet("data")]
        [Authorize(Policy = "scope")]
        public IActionResult MediumSensitivityData()
        {
            return Ok(new { data = "Some data for developers", user = GetSubject(), title = GetClaim("title") });
        }

        /*
         * A high security endpoint that also requires a low risk score
         */
        [HttpGet("sensitivedata")]
        [Authorize(Policy = "scope")]
        [Authorize(Policy = "low_risk")]
        public IActionResult HighSensitivityData()
        {
            return Ok(new { data = "Your risk score is low", user = GetSubject(), risk = GetClaim("risk") });
        }

        private String GetSubject()
        {
            return GetClaim(ClaimTypes.NameIdentifier);
        }

        private String GetClaim(String type)
        {
            Claim c = User.Claims.FirstOrDefault(c => c.Type == type);
            return c?.Value;
        }
    }
}
