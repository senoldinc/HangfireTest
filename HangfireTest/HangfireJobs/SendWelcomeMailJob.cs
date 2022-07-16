namespace HangfireTest.HangfireJobs;

public class SendWelcomeMailJob
{
    public void SendWelcomeEmail(string text)
    {
        Console.WriteLine(text);
    }
}