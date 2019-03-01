using Microsoft.AspNetCore.Mvc;
using Twilio.AspNet.Core;
using Twilio.TwiML;
using Twilio.TwiML.Voice;

namespace WebApplication.Controllers
{
    public class ConferenceController : TwilioController
    {
        private const string _twilioPhone = "+15017250604";

        [HttpPost]
        public IActionResult Index(string from)
        {
            var response = new VoiceResponse();

            if (from == _twilioPhone)
            {
                var dial = new Dial();
                dial.Conference("Room 1",
                    startConferenceOnEnter: true,
                    endConferenceOnExit: true);
                response.Append(dial);
            }
            else
            {
                response.Reject();
            }
            
            return TwiML(response);
        }
    }
}