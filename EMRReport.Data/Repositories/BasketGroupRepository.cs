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
    public sealed class BasketGroupRepository : IBasketGroupRepository
    {
        private readonly ScrubberDbContext _dbContext;
        private int userID = 0;
        public BasketGroupRepository(ScrubberDbContext dbContext)
        {
            _dbContext = dbContext;
            userID = 1;
        }
        public async Task<BasketGroupEntity> CreateBasketGroupAsync(BasketGroupEntity basketGroupEntity, CancellationToken token)
        {
            await _dbContext.BasketGroup.AddAsync(basketGroupEntity, token);
            await _dbContext.SaveChangesAsync(token);
            return basketGroupEntity;
        }
    }
}