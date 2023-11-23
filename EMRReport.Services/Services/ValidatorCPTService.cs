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
    public sealed class ValidatorCPTService : IValidatorCPTService
    {
        private readonly IMapper _mapper;
        private readonly IValidatorCPTRepository _validatorCPTRepository;
        public ValidatorCPTService(IValidatorCPTRepository validatorCPTRepository, IMapper mapper)
        {
            _validatorCPTRepository = validatorCPTRepository;
            _mapper = mapper;
        }
        public async Task<List<ValidatorCPTServiceObject>> BulkCreateValidatorCPTAsync(List<ValidatorCPTServiceObject> basketGroupServiceObjectList, int ValidatorTransactionID, int Age, string Gender, CancellationToken token)
        {
            var validatorCPTEntityList = _mapper.Map<List<ValidatorCPTEntity>>(basketGroupServiceObjectList);
            await Task.Run(() =>
            {
                for (int i = 0; i < validatorCPTEntityList.Count; i++)
                {
                    var data = validatorCPTEntityList.Skip(i).Take(1).FirstOrDefault();
                    data.CPT = data.CPT.EncryptCode();
                    data.Net = 1;
                    data.ValidatorTransactionID = ValidatorTransactionID;
                    data.Age = Age;
                    data.Gender = Gender;
                    data.Status = true;
                }
            });
            validatorCPTEntityList = await _validatorCPTRepository.BulkCreateValidatorCPTAsync(validatorCPTEntityList, token);
            return _mapper.Map<List<ValidatorCPTServiceObject>>(validatorCPTEntityList);
        }
    }
}