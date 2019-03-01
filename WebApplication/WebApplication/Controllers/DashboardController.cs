using System;
using System.Diagnostics;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Twilio;
using Twilio.Rest.Api.V2010.Account.Usage;

namespace WebApplication.Controllers
{
    public class DashboardController : Controller
    {
        public DashboardController(IOptions<AppSettings> settings)
        {
            _settings = settings.Value;
        }

        private readonly AppSettings _settings;

        [HttpGet]
        public IActionResult Index()
        {
            TwilioClient.Init(_settings.AccountSid, _settings.AuthToken);

            var records = RecordResource.Read();

            var price = records.Sum(r => r.Price);
            var count = records.Sum(r => Convert.ToInt32(r.Count));

            Debug.WriteLine(price);
            Debug.WriteLine(count);

            return Ok();
        }
    }
}