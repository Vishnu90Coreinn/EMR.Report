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
    public sealed class ActivityRepository : IActivityRepository
    {
        private readonly ScrubberDbContext _dbContext;
        private int userID = 0;
        public ActivityRepository(ScrubberDbContext dbContext)
        {
            _dbContext = dbContext;
            userID = 1;
        }

        public async Task<ActivityEntity> CreateActivityAsync(ActivityEntity activityEntity, CancellationToken token)
        {
            activityEntity.CreatedBy = userID;
            activityEntity.CreatedDate = DateTime.Now;
            await _dbContext.Activity.AddAsync(activityEntity, token);
            await _dbContext.SaveChangesAsync(token);
            return activityEntity;
        }

        public async Task<ActivityEntity> FindActivityAsync(int ActivityID, CancellationToken token)
        {
            return await _dbContext.Activity.FirstOrDefaultAsync(x => x.ActivityID == ActivityID, token);
        }

        public async Task<ICollection<ActivityEntity>> GetActivityListAsync(CancellationToken token)
        {
            return await _dbContext.Activity.ToListAsync(token);
        }

        public async Task<ActivityEntity> UpdateActivityAsync(ActivityEntity activityEntity, CancellationToken token)
        {
            activityEntity.ModifiedBy = userID;
            activityEntity.ModifiedDate = DateTime.Now;
            _dbContext.Activity.Update(activityEntity);
            await _dbContext.SaveChangesAsync(token);
            return activityEntity;
        }
    }
}