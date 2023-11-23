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
    public sealed class ValidatorTransactionRepository : IValidatorTransactionRepository
    {
        private readonly ScrubberDbContext _dbContext;
        private int userID = 0;
        public ValidatorTransactionRepository(ScrubberDbContext dbContext)
        {
            _dbContext = dbContext;
            userID = 1;
        }
        public async Task<ValidatorTransactionEntity> CreateValidatorTransactionAsync(ValidatorTransactionEntity validatorTransactionEntity, CancellationToken token)
        {
            await _dbContext.ValidatorTransaction.AddAsync(validatorTransactionEntity, token);
            await _dbContext.SaveChangesAsync(token);
            return validatorTransactionEntity;
        }
    }
}