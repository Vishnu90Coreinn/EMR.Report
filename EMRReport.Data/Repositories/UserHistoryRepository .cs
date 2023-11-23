using EFCore.BulkExtensions;
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
    public sealed class UserHistoryRepository : IUserHistoryRepository
    {
        private readonly ScrubberDbContext _dbContext;
        private int UserID = 0;
        public UserHistoryRepository(ScrubberDbContext dbContext)
        {
            _dbContext = dbContext;
            UserID = 1;
        }
        public async Task<List<UserHistoryEntity>> GetSessionOutUserHistoryListAsync(CancellationToken token)
        {
            var datenow = DateTime.Now.AddHours(-3);
            return await _dbContext.UserHistory.Where(x => !x.IsLogOut && !x.IsSessionOut && x.LogInTime < datenow).ToListAsync();
        }
        public async Task<UserHistoryEntity> GetUserHistoryByNameAndSessionIDAsync(int UserID, string SessionID, CancellationToken token)
        {
            return await _dbContext.UserHistory.FirstOrDefaultAsync(x => x.LoginUserID == UserID && x.LoginSessionID == SessionID && !x.IsLogOut && !x.IsSessionOut);
        }
        public async Task<UserHistoryEntity> SaveLoginUserHistoryAsync(UserHistoryEntity userHistoryEntity, CancellationToken token)
        {
            await _dbContext.UserHistory.AddAsync(userHistoryEntity, token);
            await _dbContext.SaveChangesAsync(token);
            return userHistoryEntity;
        }
        public async Task<UserHistoryEntity> SaveLogOutUserHistoryAsync(UserHistoryEntity userHistoryEntity, CancellationToken token)
        {
            var userHistoryData = await GetUserHistoryByNameAndSessionIDAsync(userHistoryEntity.LoginUserID, userHistoryEntity.LoginSessionID, token);
            if (userHistoryData != null)
            {
                userHistoryData.IsLogOut = true;
                userHistoryData.LogOutTime = DateTime.Now;
                _dbContext.UserHistory.Update(userHistoryEntity);
                await _dbContext.SaveChangesAsync(token);
            }
            return userHistoryEntity;
        }
        public async Task SaveSessionOutUserHistoryAsync(List<UserHistoryEntity> userHistoryEntityList, CancellationToken token)
        {
            await _dbContext.BulkUpdateAsync(userHistoryEntityList);
        }
    }
}