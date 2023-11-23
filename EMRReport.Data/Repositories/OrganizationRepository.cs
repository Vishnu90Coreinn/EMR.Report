using EMRReport.Data;
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
    public sealed class OrganizationRepository : IOrganizationRepository
    {
        private readonly ScrubberDbContext _dbContext;
        private int UserID = 0;
        public OrganizationRepository(ScrubberDbContext dbContext)
        {
            _dbContext = dbContext;
            UserID = 1;
        }
        public async Task<OrganizationEntity> CreateOrganizationAsync(OrganizationEntity organizationEntity, CancellationToken token)
        {
            if (organizationEntity.IsUnlimited)
                organizationEntity.ClaimCount = 0;
            organizationEntity.CreatedBy = UserID;
            organizationEntity.CreatedDate = DateTime.Now;
            organizationEntity.Status = true;
            organizationEntity.OrganizationGuid = Guid.NewGuid();
            await _dbContext.Organization.AddAsync(organizationEntity, token);
            await _dbContext.SaveChangesAsync(token);
            return organizationEntity;
        }
        public async Task<OrganizationEntity> UpdateOrganizationAsync(OrganizationEntity organizationEntity, CancellationToken token)
        {
            organizationEntity.ModifiedBy = UserID;
            organizationEntity.ModifiedDate = DateTime.Now;
            _dbContext.Organization.Update(organizationEntity);
            await _dbContext.SaveChangesAsync(token);
            return organizationEntity;
        }
        public async Task<OrganizationEntity> GetOrganizationByIDAsync(int OrganizationID, CancellationToken token)
        {
            return await _dbContext.Organization.FirstOrDefaultAsync(x => x.OrganizationID == OrganizationID && x.Status, token);
        }
        public async Task<IEnumerable<OrganizationEntity>> GetOrganizationListAsync(CancellationToken token)
        {
            return await _dbContext.Organization.ToListAsync(token);
        }
        public async Task<IEnumerable<OrganizationEntity>> GetOrganizationListByNameAsync(string organizationName, CancellationToken token)
        {
            organizationName = organizationName.ToLower();
            return await _dbContext.Organization.Where(x => x.OrganizationName.ToLower().Contains(organizationName)).ToListAsync(token);
        }
        public async Task<OrganizationEntity> GetOrganizationByNameAsync(string organizationName, CancellationToken token)
        {
            organizationName = organizationName.ToLower();
            return await _dbContext.Organization.FirstOrDefaultAsync(x => x.OrganizationName.ToLower().Equals(organizationName), token);
        }
    }
}