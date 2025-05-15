using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace Demo.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DemoController : ControllerBase
    {
        /*
         * A normal security level endpoint that enforces a required scope
         */
        [HttpGet("data")]
        [Authorize]
        [Authorize(Policy = "has_required_scope")]
        public IActionResult MediumSensitivityData()
        {
            return Ok(new { data = "Some medium sensitivity data", user = GetSubject() });
        }

        /*
         * A high security endpoint that also requires a custom claim with a low risk score
         */
        [HttpGet("highworthdata")]
        [Authorize]
        [Authorize(Policy = "has_required_scope")]
        [Authorize(Policy = "has_low_risk")]
        public IActionResult HighSensitivityData()
        {
            return Ok(new { data = "Some high sensitivity data", user = GetSubject(), risk = GetClaim("risk") });
        }

        private String GetSubject()
        {
            return GetClaim("sub");
        }

        private String GetClaim(String type)
        {
            Claim c = User.Claims.FirstOrDefault(c => c.Type == type);
            return c?.Value;
        }
    }
}
