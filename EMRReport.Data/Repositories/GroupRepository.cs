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
    public sealed class GroupRepository : IGroupRepository
    {
        private readonly ScrubberDbContext _dbContext;
        private int UserID = 0;
        public GroupRepository(ScrubberDbContext dbContext)
        {
            _dbContext = dbContext;
            UserID = 1;
        }
        public async Task<IEnumerable<GroupEntity>> GetGroupDDLAsync(CancellationToken token)
        {
            return await _dbContext.Group.Select(x => new GroupEntity { GroupId = x.GroupId, GroupName = x.GroupName }).ToListAsync(token);
        }
    }
}