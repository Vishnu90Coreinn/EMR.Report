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
    public sealed class CompanyRoleRepository : ICompanyRoleRepository
    {
        private readonly ScrubberDbContext _dbContext;
        private int UserID = 0;
        public CompanyRoleRepository(ScrubberDbContext dbContext)
        {
            _dbContext = dbContext;
            UserID = 1;
        }
        public async Task<CompanyRoleEntity> GetRoleByNameAsync(string roleName, CancellationToken token)
        {
            roleName = roleName.ToLower();
            return await _dbContext.CompanyRole.FirstOrDefaultAsync(x => x.CompanyRole.ToLower().Equals(roleName), token);
        }
        public async Task<IEnumerable<CompanyRoleEntity>> GetCompanyRoleDDLAsync(CancellationToken token)
        {
            return await _dbContext.CompanyRole.Select(x => new CompanyRoleEntity { CompanyRoleId = x.CompanyRoleId, CompanyRole = x.CompanyRole }).ToListAsync(token);
        }
    }
}