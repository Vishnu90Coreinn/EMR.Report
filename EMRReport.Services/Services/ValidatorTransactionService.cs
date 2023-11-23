using AutoMapper;
using EMRReport.DataContracts.Entities;
using EMRReport.DataContracts.IRepositories;
using EMRReport.ServiceContracts.IServices;
using EMRReport.ServiceContracts.ServiceObjects;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace EMRReport.Services.Services
{
    public sealed class ValidatorTransactionService : IValidatorTransactionService
    {
        private readonly IMapper _mapper;
        private readonly IValidatorTransactionRepository _validatorTransactionRepository;
        public ValidatorTransactionService(IValidatorTransactionRepository validatorTransactionRepository, IMapper mapper)
        {
            _validatorTransactionRepository = validatorTransactionRepository;
            _mapper = mapper;
        }
        public async Task<ValidatorTransactionServiceObject> CreateValidatorTransactionAsync(ValidatorTransactionServiceObject validatorTransactionServiceObject, CancellationToken token)
        {
            var validatorTransactionEntity = _mapper.Map<ValidatorTransactionEntity>(validatorTransactionServiceObject);
            validatorTransactionEntity = await _validatorTransactionRepository.CreateValidatorTransactionAsync(validatorTransactionEntity, token);
            return _mapper.Map<ValidatorTransactionServiceObject>(validatorTransactionEntity);
        }
    }
}