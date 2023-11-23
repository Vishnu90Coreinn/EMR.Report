using EMRReport.DataContracts.Entities;
using EMRReport.DataContracts.IRepositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace EMRReport.Data.Repositories
{
    public sealed class AddressRepository : IAddressRepository
    {
        private readonly ScrubberDbContext _dbContext;
        private int UserID = 0;
        public AddressRepository(ScrubberDbContext dbContext)
        {
            _dbContext = dbContext;
            UserID = 1;
        }
        public async Task<AddressEntity> FindAddressByIdAsync(int addressId, CancellationToken token)
        {
            return await _dbContext.Address.FirstOrDefaultAsync(x => x.AddressID == addressId, token);
        }
    }
}