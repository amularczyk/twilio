using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Twilio.AspNet.Core;
using Twilio.TwiML;

namespace WebApplication.Controllers
{
    public static class Database
    {
        public static List<string> ToDos = new List<string>();
    }

    public class SmsController : TwilioController
    {
        [HttpGet]
        public IActionResult Index(string body)
        {
            var method = body.Split(' ')[0];
            switch (method.ToLower())
            {
                case "add":
                    Database.ToDos.Add(body.Replace("add ", "").Replace("Add ", ""));
                    break;

                case "remove":
                    Database.ToDos.Remove(body.Replace("remove ", "").Replace("Remove ", ""));
                    break;

                case "list":
                    var sb = new StringBuilder();
                    for (var i = 1; i <= Database.ToDos.Count; i++)
                    {
                        sb.AppendLine($"{i} {Database.ToDos[i - 1]}");
                    }

                    var messagingResponse = new MessagingResponse();
                    messagingResponse.Message(
                        sb.ToString(),
                        statusCallback: new Uri("http://3f52b400.ngrok.io/status"));
                    return TwiML(messagingResponse);
            }

            return Ok();
        }

        [HttpPost]
        public IActionResult Index()
        {
            var fromCountry = Request.Form["FromCountry"];
            var messagingResponse = new MessagingResponse();
            messagingResponse.Message($"Hi! It looks like your phone number was born in {fromCountry}");

            return TwiML(messagingResponse);
        }
    }
}