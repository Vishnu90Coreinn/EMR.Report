using EMRReport.DataContracts.Entities;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
namespace EMRReport.DataContracts.IRepositories
{
    public interface IValidatorErrorRepository
    {
        Task<List<ValidatorErrorEntity>> GetValidatorActivityErrorsAsync(string CaseNumber, string EncryptedICDS, string EncryptedCPTS, DateTime date, CancellationToken token);
        Task<List<ValidatorErrorEntity>> GetValidatorICDErrorsAsync(string CaseNumber, string EncryptedICDS, string EncryptedCPTS, DateTime date, CancellationToken token);
        Task<List<ValidatorErrorEntity>> GetValidatorICDCPTErrorsAsync(string CaseNumber, string EncryptedICDS, string EncryptedCPTS, DateTime date, CancellationToken token);
        Task<List<ValidatorErrorEntity>> CreateBulkValidatorErrorAsync(List<ValidatorErrorEntity> validatorErrorEntityList, CancellationToken token);
        Task<List<ValidatorErrorEntity>> GetAPPValidatorActivityErrorsAsync(string CaseNumber, string EncryptedICDS, string EncryptedCPTS, string Classification, DateTime date, CancellationToken token, int RuleVersion, int AuthorityType);
        Task<List<ValidatorErrorEntity>> GetAPPValidatorICDErrorsAsync(string CaseNumber, string EncryptedICDS, string EncryptedCPTS, string Classification, DateTime date, CancellationToken token, int RuleVersion, int AuthorityType);
        Task<List<ValidatorErrorEntity>> GetAPPValidatorICDCPTErrorsAsync(string CaseNumber, string EncryptedICDS, string EncryptedCPTS, string Classification, DateTime date, CancellationToken token, int RuleVersion, int AuthorityType);
        Task<List<ValidatorErrorEntity>> GetDetailValidateActivityErrorsAsync(int ValidatorTransactionID, int RuleVersion, int AuthorityType, string Classification, DateTime date, CancellationToken token);
        Task<List<ValidatorErrorEntity>> GetDetailValidateICDErrorsAsync(int ValidatorTransactionID, int RuleVersion, int AuthorityType, string Classification, DateTime date, CancellationToken token);
        Task<List<ValidatorErrorEntity>> GetDetailValidateICDCPTErrorsAsync(int ValidatorTransactionID, int RuleVersion, int AuthorityType, string Classification, DateTime date, CancellationToken token);
    }
}
