using System.Text;
using CavistaEventCelebration.Api.Models.EmailService;
using CavistaEventCelebration.Api.Repositories.Interface;
using CavistaEventCelebration.Api.Services.Interface;
using Hangfire;

namespace CavistaEventCelebration.Api.Services.Implementation
{
    public class EventCelebrationService : IEventCelebrationService
    {
        private readonly IEventRepo _eventRepo;
        private readonly IMailService _mailService;
        private readonly IRecurringJobManager _recurringJobManager;
        private readonly IWebHostEnvironment _env;


        public EventCelebrationService(IEventRepo eventRepo, IMailService mailService, IRecurringJobManager recurringJobManager, IWebHostEnvironment env)
        {
            _eventRepo = eventRepo;
            _mailService = mailService;
            _recurringJobManager = recurringJobManager;
            _env = env;
        }

        public async Task NotifyEventAsync(int eventId)
        {
            var employeeEvents = await _eventRepo.GetDailyEvents(eventId);

            foreach (var ev in employeeEvents)
            {
                //Todo : general message and custom message should be added when creating event from the UI
                string generalMessage = "We are excited to celebrate this special occasion with you!";
                string customMessage = ev.EventTitle switch
                {
                    "Birthday" => "Wishing you joy, good health, and success in the year ahead 🎂",
                    "Work Anniversary" => "Thank you for being part of our journey and for your valuable contributions 💼",
                    "Wedding Anniversary" => "May your bond continue to grow stronger with each passing year 💕",
                    _ => "Best wishes on your special day! 🎊"
                };

                var template = LoadTemplate();
                var finalBody = ReplaceTokens(template,
                                          ev.EmployeeFirstName,
                                           ev.EmployeeLastName,
                                            ev.EventTitle,
                                              generalMessage,
                                              customMessage);

                var mailData = new MailData()
                {
                    EmailToId = ev.EmployeeEmailAddress,
                    EmailSubject = $"Happy {ev.EventTitle}! 🎊",
                    EmailBody = finalBody,
                    EmailToName = $"{ev.EmployeeFirstName} {ev.EmployeeLastName}"
                };
                var to = new List<string>() { ev.EmployeeEmailAddress };
                var message = new Message(to, $"Happy {ev.EventTitle}! 🎊", finalBody);
                await _mailService.SendEmailAsync(message);

            }
            if (employeeEvents != null && employeeEvents.Any())
            {
                var teamsChannelEmail = "9c1a5b36.axxess.com@amer.teams.ms";
                string today = DateTime.UtcNow.ToString("MMMM, dd , yyyy");
                string eventType = employeeEvents.FirstOrDefault()?.EventTitle ?? "Event";
                var summaryBody = new StringBuilder();
                summaryBody.Append($"<h3>🎉 {today} {eventType} Celebrations 🎉</h3><ul>");

                foreach (var ev in employeeEvents)
                {
                    summaryBody.Append($"<li><b>{ev.EmployeeFirstName} {ev.EmployeeLastName}</b></li>");
                }

                summaryBody.Append("</ul><p>Let's celebrate together! 🎊</p>");

                var teamsMail = new MailData()
                {
                    EmailToId = teamsChannelEmail,
                    EmailToName = "Celebrations Channel",
                    EmailSubject = $"🎉 {today} {eventType} Celebrations",
                    EmailBody = summaryBody.ToString()
                };

                var to = new List<string>() { teamsChannelEmail };
                var message = new Message(to, $"🎉 {today} {eventType} Celebrations", summaryBody.ToString());

                //this will most likely not work due to teams organisation restriction
                await _mailService.SendEmailAsync(message);
            }
        }


        public async Task RegisterRecurringJobsAsync()
        {
            var events = await _eventRepo.Events();

            foreach (var ev in events)
            {
                _recurringJobManager.AddOrUpdate<IEventCelebrationService>(
                    recurringJobId: $"daily-{ev.Name.ToLower().Replace(" ", "-")}-check",
                    methodCall: s => s.NotifyEventAsync(ev.Id),
                    cronExpression: Cron.Daily(9)
                );
            }
        }

        private string LoadTemplate()
        {
            var path = Path.Combine(_env.ContentRootPath, "EmailTemplates", "EventTemplate.html");
            return File.ReadAllText(path);
        }

        private string ReplaceTokens(string template, string firstName, string lastName, string eventTitle, string generalMessage, string customMessage)
        {
            return template
                .Replace("{{FirstName}}", firstName)
                .Replace("{{LastName}}", lastName)
                .Replace("{{EventTitle}}", eventTitle)
                .Replace("{{GeneralMessage}}", generalMessage)
                .Replace("{{CustomMessage}}", customMessage);
        }

    }
}
