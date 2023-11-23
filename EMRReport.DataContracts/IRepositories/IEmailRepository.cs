using EMRReport.DataContracts.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EMRReport.DataContracts.IRepositories
{
    public interface IEmailRepository
    {
        Task<EmailEntity> CreateUserAsync(EmailEntity emailEntity, CancellationToken token);
    }
}
