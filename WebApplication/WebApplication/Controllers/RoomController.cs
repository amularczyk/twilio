using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Twilio;
using Twilio.Rest.Video.V1;

namespace WebApplication.Controllers
{
    public class RoomController : Controller
    {
        public RoomController(IOptions<AppSettings> settings)
        {
            _settings = settings.Value;
        }

        private readonly AppSettings _settings;

        public async Task<IActionResult> Index()
        {
            TwilioClient.Init(_settings.AccountSid, _settings.AuthToken);

            try
            {
                var room1 = await RoomResource.CreateAsync(new CreateRoomOptions { UniqueName = "DailyStandup1" });
                Debug.WriteLine(room1.UniqueName);
                var room2 = await RoomResource.CreateAsync(new CreateRoomOptions { UniqueName = "DailyStandup2" });
                Debug.WriteLine(room2.UniqueName);

                var rooms = await RoomResource.FetchAsync(room1.Sid);
                Debug.WriteLine(rooms.UniqueName);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }

            return Ok();
        }
    }
}