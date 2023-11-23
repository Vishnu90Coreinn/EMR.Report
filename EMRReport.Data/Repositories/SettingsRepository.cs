using EMRReport.DataContracts.IRepositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;

namespace EMRReport.Data.Repositories
{
    public sealed class SettingsRepository : ISettingsRepository
    {
        private readonly ScrubberDbContext _dbContext;
        public SettingsRepository(ScrubberDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<Tuple<DateTime?, DateTime?>> GetDubaiAndAbuDhabiDOS(int DubaiProjectConstatID, int AbuDhabiProjectConstantID, CancellationToken token)
        {
            DateTime? DOS = null;
            DateTime? AbuDhabiDOS = null;
            var dubai = await _dbContext.Settings.FirstOrDefaultAsync(x => x.ProjectConstantID == DubaiProjectConstatID);
            if (dubai != null)
            {
                if (!string.IsNullOrEmpty(dubai.ProjectConstantDBID))
                    DOS = DateTime.ParseExact(dubai.ProjectConstantDBID, "d/M/yyyy", CultureInfo.InvariantCulture);
            }
            var abudhabi = await _dbContext.Settings.FirstOrDefaultAsync(x => x.ProjectConstantID == AbuDhabiProjectConstantID);
            if (abudhabi != null)
            {
                if (!string.IsNullOrEmpty(abudhabi.ProjectConstantDBID))
                    AbuDhabiDOS = DateTime.ParseExact(abudhabi.ProjectConstantDBID, "d/M/yyyy", CultureInfo.InvariantCulture);
            }
            return Tuple.Create(DOS, AbuDhabiDOS);
        }
    }
}