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
    public sealed class FacilityTypeRepository : IFacilityTypeRepository
    {
        private readonly ScrubberDbContext _dbContext;
        private int UserID = 0;
        public FacilityTypeRepository(ScrubberDbContext dbContext)
        {
            _dbContext = dbContext;
            UserID = 1;
        }
        public async Task<FacilityTypeEntity> GetFacilityTypeByNameAsync(string facilityTypeName, CancellationToken token)
        {
            facilityTypeName = facilityTypeName.ToLower();
            return await _dbContext.FacilityType.FirstOrDefaultAsync(x => x.FacilityTypeName.ToLower().Equals(facilityTypeName), token);
        }
        public async Task<IEnumerable<FacilityTypeEntity>> GetFacilityTypeDDLAsync(CancellationToken token)
        {
            return await _dbContext.FacilityType.Select(x => new FacilityTypeEntity { FacilityTypeID = x.FacilityTypeID, FacilityTypeName = x.FacilityTypeName }).ToListAsync(token);
        }
    }
}