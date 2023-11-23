using System;
using System.Threading;
using System.Threading.Tasks;

namespace EMRReport.ServiceContracts.IServices
{
    public interface ISettingsService
    {
        Task<Tuple<DateTime?, DateTime?>> GetDubaiAndAbuDhabiDOS(CancellationToken token);
    }
}
