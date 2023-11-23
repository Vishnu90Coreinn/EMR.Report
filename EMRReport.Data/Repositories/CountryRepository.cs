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
    public sealed class CountryRepository : ICountryRepository
    {
        private readonly ScrubberDbContext _dbContext;
        private int UserID = 0;
        public CountryRepository(ScrubberDbContext dbContext)
        {
            _dbContext = dbContext;
            UserID = 1;
        }
        public async Task<CountryEntity> GetCountryByNameAsync(string countryName, CancellationToken token)
        {
            countryName = countryName.ToLower();
            return await _dbContext.Country.FirstOrDefaultAsync(x => x.Country.ToLower().Equals(countryName), token);
        }
    }
}