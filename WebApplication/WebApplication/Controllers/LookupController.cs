using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Twilio;
using Twilio.Rest.Lookups.V1;
using Twilio.Types;

namespace WebApplication.Controllers
{
    [Route("lookup")]
    public class LookupController : Controller
    {
        private string[] phoneNumbers_1 =
        {
            "+551155256325",
            "+ 493019449",
            "+4915735982887",
            "+496979550",
            "+493022610",
            "+498969931333",
            "+443432221234",
            "+4916793929939",
            "+17189237300",
            "+4915735997026​"
        };

        private readonly string[] phoneNumbers_2 =
        {
            "+13123133187",
            "+12092104311",
            "+16513219277",
            "+14255288365",
            "+61481073056",
            "+17864755228"
        };

        public LookupController(IOptions<AppSettings> settings)
        {
            _settings = settings.Value;
        }

        private readonly AppSettings _settings;

        [HttpGet]
        public IActionResult Index()
        {
            TwilioClient.Init(_settings.AccountSid, _settings.AuthToken);

            try
            {
                var type = new List<string>
                {
                    "carrier"
                };


                foreach (var phoneNumber in phoneNumbers_2)
                {
                    var phoneNumberResource = PhoneNumberResource.Fetch(
                        type: type,
                        pathPhoneNumber: new PhoneNumber(phoneNumber)
                    );
                    Debug.WriteLine(phoneNumber);
                    foreach (var carrier in phoneNumberResource.Carrier)
                    {
                        Debug.WriteLine(carrier.Key);
                        Debug.WriteLine(carrier.Value);
                    }
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }

            return Ok();
        }

        [HttpGet]
        [Route("addons")]
        public async Task<IActionResult> AddOns()
        {
            TwilioClient.Init(_settings.AccountSid, _settings.AuthToken);

            var addOns = new List<string>
            {
                "payfone_tcpa_compliance"
            };

            var addOnsData = new Dictionary<string, object>
            {
                { "payfone_tcpa_compliance.right_party_contacted_date", 20160101 }
            };

            var phoneNumber = await PhoneNumberResource.FetchAsync(
                addOns: addOns,
                addOnsData: addOnsData,
                pathPhoneNumber: new PhoneNumber(_settings.MyNumber)
            );

            Debug.WriteLine(phoneNumber.CallerName);

            return Ok();
        }
    }
}