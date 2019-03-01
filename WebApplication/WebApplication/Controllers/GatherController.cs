using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Twilio.AspNet.Core;
using Twilio.Http;
using Twilio.TwiML;
using Twilio.TwiML.Voice;

namespace WebApplication.Controllers
{
    [Route("gather")]
    public class GatherController : TwilioController
    {
        public GatherController(IOptions<AppSettings> settings)
        {
            _settings = settings.Value;
        }

        private readonly AppSettings _settings;

        [HttpGet]
        public IActionResult Index()
        {
            var response = new VoiceResponse();

            var gather = new Gather(numDigits: 1, action: new Uri($"{_settings.Path}/gather/1"), method: HttpMethod.Post);
            gather.Say("Press 1 or 2.");
            response = response.Append(gather);
            response.Redirect(new Uri($"{_settings.Path}/gather"), HttpMethod.Get);

            return TwiML(response);
        }

        [HttpPost]
        [Route("1")]
        public IActionResult Index1(string digits)
        {
            var response = new VoiceResponse();

            if (!string.IsNullOrEmpty(digits))
            {
                switch (digits)
                {
                    case "1":
                        response.Say("Good for you!");
                        break;
                    case "2":
                        response.Say("Excellent!");
                        break;
                    default:
                        response.Say("Sorry, I don't understand that choice.").Pause();
                        response.Redirect(new Uri($"{_settings.Path}/gather"), HttpMethod.Get);
                        break;
                }
            }
            else
            {
                response.Redirect(new Uri($"{_settings.Path}/gather"), HttpMethod.Get);
            }

            return TwiML(response);
        }
    }
}