using EFCore.BulkExtensions;
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
    public sealed class ValidatorCPTRepository : IValidatorCPTRepository
    {
        private readonly ScrubberDbContext _dbContext;
        private int userID = 0;
        public ValidatorCPTRepository(ScrubberDbContext dbContext)
        {
            _dbContext = dbContext;
            userID = 1;
        }
        public async Task<List<ValidatorCPTEntity>> BulkCreateValidatorCPTAsync(List<ValidatorCPTEntity> validatorCPTEntityList, CancellationToken token)
        {
            await _dbContext.BulkInsertAsync(validatorCPTEntityList);
            await _dbContext.SaveChangesAsync(token);
            return validatorCPTEntityList;
        }
    }
}