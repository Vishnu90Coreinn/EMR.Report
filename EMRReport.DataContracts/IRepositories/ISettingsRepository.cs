using System;
using System.Threading;
using System.Threading.Tasks;

namespace EMRReport.DataContracts.IRepositories
{
    public interface ISettingsRepository
    {
        Task<Tuple<DateTime?, DateTime?>> GetDubaiAndAbuDhabiDOS(int DubaiProjectConstatID, int AbuDhabiProjectConstantID, CancellationToken token);
    }
}