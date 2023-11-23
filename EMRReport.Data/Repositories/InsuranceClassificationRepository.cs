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
    public sealed class InsuranceClassificationRepository : IInsuranceClassificationRepository
    {
        private readonly ScrubberDbContext _dbContext;
        private int UserID = 0;
        public InsuranceClassificationRepository(ScrubberDbContext dbContext)
        {
            _dbContext = dbContext;
            UserID = 1;
        }
        public async Task<InsuranceClassificationEntity> GetInsuranceClassificationByNameAsync(string insuranceClassification, CancellationToken token)
        {
            insuranceClassification = insuranceClassification.ToLower();
            return await _dbContext.InsuranceClassification.FirstOrDefaultAsync(x => x.InsuranceClassification.ToLower().Equals(insuranceClassification), token);
        }
    }
}