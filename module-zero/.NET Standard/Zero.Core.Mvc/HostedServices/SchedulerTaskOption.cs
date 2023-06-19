namespace Zero.Core.Mvc.HostedServices
{
    public class SchedulerTaskOption
    {
        public bool Enabled { get; set; }
        public string CronSchedule { get; set; }
    }
}