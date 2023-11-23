using EFCore.BulkExtensions;
using EMRReport.DataContracts.Entities;
using EMRReport.DataContracts.IRepositories;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace EMRReport.Data.Repositories
{
    public sealed class ValidatorICDRepository : IValidatorICDRepository
    {
        private readonly ScrubberDbContext _dbContext;
        private int userID = 0;
        public ValidatorICDRepository(ScrubberDbContext dbContext)
        {
            _dbContext = dbContext;
            userID = 1;
        }
        public async Task<List<ValidatorICDEntity>> BulkCreateValidatorICDAsync(List<ValidatorICDEntity> validatorICDEntityList, CancellationToken token)
        {
            await _dbContext.BulkInsertAsync(validatorICDEntityList);
            return validatorICDEntityList;
        }
    }
}