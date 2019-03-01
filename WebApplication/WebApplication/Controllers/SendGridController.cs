using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace WebApplication.Controllers
{
    public class SendGridController : Controller
    {
        public SendGridController(IOptions<AppSettings> settings)
        {
            _settings = settings.Value;
        }

        private readonly AppSettings _settings;

        public async Task<IActionResult> Index()
        {
            var sendGrid = new SendGridClient(_settings.SendGridKey);

            var from = "owl@example.com";
            var to = "owl@example.com";
            var subject = "Email subject";
            var content = "Email content";

            var mail = new SendGridMessage
            {
                From = new EmailAddress(from),
                ReplyTo = new EmailAddress(from),
                Subject = subject,
                PlainTextContent = content,
                Personalizations = new List<Personalization>()
            };
            var personalization = new Personalization { Tos = new List<EmailAddress>() };
            personalization.Tos.Add(new EmailAddress(to));
            mail.Personalizations.Add(personalization);

            var response = await sendGrid.SendEmailAsync(mail);
            var tmp = await response.Body.ReadAsStringAsync();

            return Ok(response);
        }
    }
}