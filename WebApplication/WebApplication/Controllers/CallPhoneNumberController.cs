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
    public class CallPhoneNumberController : Controller
    {
        public CallPhoneNumberController(IOptions<AppSettings> settings)
        {
            _settings = settings.Value;
        }

        private readonly AppSettings _settings;

        public async Task<IActionResult> Index()
        {
            TwilioClient.Init(_settings.AccountSid, _settings.AuthToken);

            try
            {
                var call = await CallResource.CreateAsync(
                    url: new Uri("http://demo.twilio.com/docs/voice.xml"),
                    from: new PhoneNumber("+48799449055"),
                    to: new PhoneNumber(_settings.MyNumber)
                );
                Debug.WriteLine(call.Sid);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }

            return Ok();
        }
    }
}