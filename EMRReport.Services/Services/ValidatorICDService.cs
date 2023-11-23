using AutoMapper;
using EMRReport.Common.CodeEncryption;
using EMRReport.DataContracts.Entities;
using EMRReport.DataContracts.IRepositories;
using EMRReport.ServiceContracts.IServices;
using EMRReport.ServiceContracts.ServiceObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace EMRReport.Services.Services
{
    public sealed class ValidatorICDService : IValidatorICDService
    {
        private readonly IMapper _mapper;
        private readonly IValidatorICDRepository _validatorICDRepository;
        public ValidatorICDService(IValidatorICDRepository validatorICDRepository, IMapper mapper)
        {
            _validatorICDRepository = validatorICDRepository;
            _mapper = mapper;
        }
        public async Task<List<ValidatorICDServiceObject>> BulkCreateValidatorICDAsync(List<ValidatorICDServiceObject> basketGroupServiceObjectList, int ValidatorTransactionID, int Age, string Gender, CancellationToken token)
        {
            var validatorICDEntityList = _mapper.Map<List<ValidatorICDEntity>>(basketGroupServiceObjectList);
            await Task.Run(() =>
            {
                for (int i = 0; i < validatorICDEntityList.Count; i++)
                {
                    var data = validatorICDEntityList.Skip(i).Take(1).FirstOrDefault();
                    data.ICD = data.ICD.EncryptCode();
                    data.ValidatorTransactionID = ValidatorTransactionID;
                    data.Age = Age;
                    data.Gender = Gender;
                    data.Status = true;
                }
            });
            validatorICDEntityList = await _validatorICDRepository.BulkCreateValidatorICDAsync(validatorICDEntityList, token);
            return _mapper.Map<List<ValidatorICDServiceObject>>(validatorICDEntityList);
        }
    }
}