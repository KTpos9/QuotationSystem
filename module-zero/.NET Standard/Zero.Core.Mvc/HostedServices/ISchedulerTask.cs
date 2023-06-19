using System.Threading;
using System.Threading.Tasks;

namespace Zero.Core.Mvc.HostedServices
{
    public interface ISchedulerTask
    {
        SchedulerTaskOption Option { get; }

        Task ExecuteAsync(CancellationToken cancellationToken);
    }
}