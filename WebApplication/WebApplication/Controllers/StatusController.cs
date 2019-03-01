using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication.Controllers
{
    public class StatusController : Controller
    {
        [HttpPost]
        public IActionResult Index()
        {
            Debug.WriteLine($"messageStatus = {Request.Form["MessageStatus"]}");
            Debug.WriteLine($"messageSid = {Request.Form["MessageSid"]}");
            Debug.WriteLine($"signature = {Request.Headers["X-Twilio-Signature"]}");
            return Ok();
        }
    }
}