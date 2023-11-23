using EMRReport.ServiceContracts.ServiceObjects;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace EMRReport.ServiceContracts.IServices
{
    public interface IControlService
    {
        Task<ICollection<ControlServiceObject>> GetControlDDLAsync(CancellationToken token);
    }
}
