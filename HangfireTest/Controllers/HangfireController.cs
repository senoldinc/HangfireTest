using Hangfire;
using HangfireTest.HangfireJobs;
using Microsoft.AspNetCore.Mvc;

namespace HangfireTest.Controllers;

[ApiController]
[Route("api/[controller]")]
public class HangfireController : ControllerBase
{
   
    private readonly ILogger<HangfireController> _logger;
    private readonly IBackgroundJobClient _backgroundJobClient;

    public HangfireController(ILogger<HangfireController> logger, IBackgroundJobClient backgroundJobClient)
    {
        _logger = logger;
        _backgroundJobClient = backgroundJobClient;
    }

    [HttpGet()]
    public IActionResult Get()
    {
        return Ok("Hangfire test app");
    }

    [HttpPost]
    [Route("[action]")]
    public IActionResult Welcome()
    {
        var job = new SendWelcomeMailJob();
        var jobId = BackgroundJob.Enqueue(() => job.SendWelcomeEmail("Welcome to our app"));
        return Ok($"JobId: {jobId}, Welcome email sent to the user!");
    }
    
    [HttpPost]
    [Route("[action]")]
    public IActionResult Discount()
    {
        var job = new SendWelcomeMailJob();
        var jobId = BackgroundJob.Schedule(() => job.SendWelcomeEmail("Hey this is your discount coupon 'WELCOME20'"), TimeSpan.FromSeconds(30));
        return Ok($"JobId: {jobId}, Discount coupon will be sent in 30 seconds!");
    }
    
    [HttpPost]
    [Route("[action]")]
    public IActionResult DatabaseUpdate()
    {
        var manager = new RecurringJobManager();
        manager.AddOrUpdate( "DatabaseUpdated",() => Console.WriteLine("Database updated"), Cron.Minutely);
        return Ok("Database check job initiated!");
    }
    
    [HttpPost]
    [Route("[action]")]
    public IActionResult Confirm()
    {
        var parentJobId = BackgroundJob.Schedule(() => Console.WriteLine("You asked to be unsubscribed!"), TimeSpan.FromSeconds(30));
        BackgroundJob.ContinueJobWith(parentJobId, () => Console.WriteLine("You were unsubscribed!"));
        return Ok($"Confirmation job created!");
    }

    
}