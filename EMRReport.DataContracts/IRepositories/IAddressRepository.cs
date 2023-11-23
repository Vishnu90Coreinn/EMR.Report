using EMRReport.DataContracts.Entities;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace EMRReport.DataContracts.IRepositories
{
    public interface IAddressRepository
    {
        Task<AddressEntity> FindAddressByIdAsync(int addressId, CancellationToken token);
    }
}