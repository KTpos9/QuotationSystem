using Microsoft.Extensions.DependencyInjection;
using System;

namespace Zero.System.TaskScheduler
{
    public class TaskSchedulerRunner
    {
        private readonly IServiceCollection services;

        public TaskSchedulerRunner(Action<IServiceCollection> configureServices)
        {
            services = new ServiceCollection();
            configureServices(services);
        }

        public bool Run(string command)
        {
            var serviceProvider = services.BuildServiceProvider();

            // get tasks and run
            var tasks = serviceProvider.GetServices<ITaskScheduler>();
            foreach (var task in tasks)
            {
                if (task.Command.Trim() == command.Trim())
                {
                    task.Run();
                    return true;
                }
            }

            return false;
        }
    }
}