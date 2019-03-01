using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

namespace WebApplication.Controllers
{
    public class SendSmsController : Controller
    {
        public SendSmsController(IOptions<AppSettings> settings)
        {
            _settings = settings.Value;
        }

        private readonly AppSettings _settings;

        public async Task<IActionResult> Index()
        {
            TwilioClient.Init(_settings.AccountSid, _settings.AuthToken);

            try
            {
                var message = await MessageResource.CreateAsync(
                    body: "Hi! It looks like your phone number was born in {{ Poland }}",
                    from: new PhoneNumber("+48799449055"),
                    to: new PhoneNumber("+447481360673")
                );
                Debug.WriteLine(message.Sid);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }

            return Ok();
        }
    }
}