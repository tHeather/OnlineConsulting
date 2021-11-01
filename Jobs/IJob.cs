using System.Threading;
using System.Threading.Tasks;

namespace OnlineConsulting.Jobs
{
    public interface IJob
    {
        public Task Run(CancellationToken token);
    }
}
