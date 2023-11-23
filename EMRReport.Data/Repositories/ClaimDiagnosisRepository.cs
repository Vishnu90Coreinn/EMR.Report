using EFCore.BulkExtensions;
using EMRReport.Data;
using EMRReport.DataContracts.Entities;
using EMRReport.DataContracts.IRepositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EMRReport.Data.Repositories
{
    public sealed class ClaimDiagnosisRepository : IClaimDiagnosisRepository
    {
        private readonly ScrubberDbContext _dbContext;
        private int userID = 0;
        public ClaimDiagnosisRepository(ScrubberDbContext dbContext)
        {
            _dbContext = dbContext;
            userID = 1;
        }
        public async Task<ClaimDiagnosisEntity> CreateClaimDiagnosisAsync(ClaimDiagnosisEntity claimDiagnosisEntity, CancellationToken token)
        {
            await _dbContext.ClaimDiagnosis.AddAsync(claimDiagnosisEntity, token);
            await _dbContext.SaveChangesAsync(token);
            return claimDiagnosisEntity;
        }
        public async Task<List<ClaimDiagnosisEntity>> CreateBulkClaimDiagnosisAsync(List<ClaimDiagnosisEntity> claimDiagnosisEntityList, CancellationToken token)
        {
            await _dbContext.BulkInsertAsync(claimDiagnosisEntityList);
            return claimDiagnosisEntityList;
        }
    }
}