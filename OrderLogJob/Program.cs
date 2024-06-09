using System;
using System.Threading.Tasks;
using Quartz;
using Quartz.Impl;

class Program
{
    static async Task Main(string[] args)
    {
        StdSchedulerFactory factory = new StdSchedulerFactory();
        IScheduler scheduler = await factory.GetScheduler();
        await scheduler.Start();

        IJobDetail job = JobBuilder.Create<DailyJob>()
            .WithIdentity("dailyJob", "group1")
            .Build();

        ITrigger trigger = TriggerBuilder.Create()
            .WithIdentity("dailyTrigger", "group1")
            .StartNow()
             //.WithSchedule(CronScheduleBuilder.DailyAtHourAndMinute(6, 0))
            .WithSimpleSchedule(x => x
               .WithIntervalInSeconds(15)
               .RepeatForever())
            .Build();

        await scheduler.ScheduleJob(job, trigger);

        Console.ReadLine();

        await scheduler.Shutdown();
    }
}
