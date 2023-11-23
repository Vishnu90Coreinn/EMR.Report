using EMRReport.Auth.Contracts;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace EMRReport.API.Jobs
{
    public class JwtRefreshTokenCacheJob : IHostedService, IDisposable
    {
        private readonly IJwtAuthManager _jwtAuthManager;
        private Timer _timer;

        public JwtRefreshTokenCacheJob(IJwtAuthManager jwtAuthManager)
        {
            _jwtAuthManager = jwtAuthManager;
        }
        public void Dispose()
        {
            _timer?.Dispose();
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromMinutes(1));
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _timer?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }
        private void DoWork(object state)
        {
            _jwtAuthManager.RemoveExpiredRefreshTokens(DateTime.Now);
        }
    }
}
