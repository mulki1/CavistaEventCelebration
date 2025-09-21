﻿using CavistaEventCelebration.Api.Models;
using CavistaEventCelebration.Api.Models.EmailService;
using CavistaEventCelebration.Api.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CavistaEventCelebration.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class EventController : ControllerBase
{
    private readonly IMailService _mailService;

    public EventController(IMailService mailService)
    {
        _mailService = mailService;
    }


    [HttpGet("GetEvent")]
    public IEnumerable<Event> Get()
    {
        //return Enumerable.Range(1, 5).Select(index =>
        //    new Event
        //    {
        //        Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
        //        Title = $"Event - {Random.Shared.Next(-20, 55)}",
        //    })
        //    .ToArray();

        return null;
    }

    //[Authorize(Roles = "SuperAdmin")]
    [HttpPost("send-Email")]
    public async Task SendMail(MailData mailData)
    {
         mailData = new MailData()
        {
             EmailToId = "baniaz4mulki@yahoo.com",
             EmailSubject = $"Happy Friday ! 🎊",
            EmailBody = "Abdul friday happy",
            EmailToName = $"Ihsan"
        };
        await _mailService.SendEmailAsync(mailData);
    }

    //[Authorize(Roles = "SuperAdmin")]
    [HttpPost("send-Email2")]
    public async Task SendMail2(MailData mailData)
    {
        var message = new Message(new string[] { "matthewobiasogu@gmail.com" }, "Test email", "This is the content from our email.");
        await _mailService.SendEmailSmtp(message);
    }
}

