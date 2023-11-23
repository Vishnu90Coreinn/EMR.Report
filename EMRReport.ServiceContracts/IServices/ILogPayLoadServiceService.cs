using EMRReport.ServiceContracts.ServiceObjects;
using System.Threading.Tasks;

namespace EMRReport.ServiceContracts.IServices
{
    public interface ILogPayLoadServiceService
    {
        Task SavePayLoad(LogPayLoadServiceObject logPayLoadServiceObject);
    }
}
