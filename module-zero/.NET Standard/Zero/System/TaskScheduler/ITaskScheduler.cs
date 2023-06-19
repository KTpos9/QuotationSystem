namespace Zero.System.TaskScheduler
{
    public interface ITaskScheduler
    {
        string Command { get; }

        void Run();
    }
}