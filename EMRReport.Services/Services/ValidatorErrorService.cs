using AutoMapper;
using EMRReport.Common.CodeEncryption;
using EMRReport.Common.Extensions;
using EMRReport.DataContracts.Entities;
using EMRReport.DataContracts.IRepositories;
using EMRReport.ServiceContracts.IServices;
using EMRReport.ServiceContracts.ServiceObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EMRReport.Services.Services
{
    public sealed class ValidatorErrorService : IValidatorErrorService
    {
        private readonly IMapper _mapper;
        private readonly IValidatorErrorRepository _validatorErrorRepository;
        private readonly IUserService _userService;
        private readonly IValidatorCPTService _validatorCPTService;
        private readonly IValidatorICDService _validatorICDService;
        private readonly IValidatorTransactionService _validatorTransactionService;
        public ValidatorErrorService(IValidatorErrorRepository validatorErrorRepository, IUserService userService, IValidatorCPTService validatorCPTService, IValidatorICDService validatorICDService, IValidatorTransactionService validatorTransactionService, IMapper mapper)
        {
            _validatorErrorRepository = validatorErrorRepository;
            _userService = userService;
            _validatorTransactionService = validatorTransactionService;
            _validatorCPTService = validatorCPTService;
            _validatorICDService = validatorICDService;
            _mapper = mapper;
        }

        public async Task<List<ValidatorErrorServiceObject>> GetDetialValidateAsync(List<ValidatorCPTServiceObject> validatorCPTServiceObjectList, List<ValidatorICDServiceObject> validatorICDServiceObjectList, string DateOfBirth, string Gender, bool sequenceCPT, bool sequenceICD, string CPTS, string ICDS, string Classification, CancellationToken token, string userName = "")
        {
            bool IsSequence = sequenceCPT || sequenceICD ? true : false;
            ValidatorTransactionServiceObject validatorTransactionServiceObject = new ValidatorTransactionServiceObject();
            validatorTransactionServiceObject.Status = true;
            if (IsSequence)
                validatorTransactionServiceObject = await _validatorTransactionService.CreateValidatorTransactionAsync(validatorTransactionServiceObject, token);
            int Age = DateOfBirth.ConvertDateStringToDays();
            if (sequenceCPT && validatorCPTServiceObjectList != null && validatorCPTServiceObjectList.Count > 0)
                await _validatorCPTService.BulkCreateValidatorCPTAsync(validatorCPTServiceObjectList, validatorTransactionServiceObject.ValidatorTransactionID, Age, Gender, token);
            if (sequenceICD && validatorICDServiceObjectList != null && validatorICDServiceObjectList.Count > 0)
                await _validatorICDService.BulkCreateValidatorICDAsync(validatorICDServiceObjectList, validatorTransactionServiceObject.ValidatorTransactionID, Age, Gender, token);

            DateTime date = DateTime.Now;
            var getTuple = await _userService.GetUserAuthorityAndRuleVersionAsync(token, userName);
            int userAuthority = getTuple.Item1;
            int ruleVersion = getTuple.Item2;
            List<ValidatorErrorServiceObject> validatorErrorServiceObjectList = new List<ValidatorErrorServiceObject>();
            CPTS = CPTS.ConvertValidatorInputToCommaSepertedString().EncryptCommaSeperatedCodes();
            ICDS = ICDS.ConvertValidatorInputToCommaSepertedString().EncryptCommaSeperatedCodes();
            if (sequenceCPT && validatorCPTServiceObjectList != null && validatorCPTServiceObjectList.Count > 0)
            {
                var validatorErrorEntityCPTList = await _validatorErrorRepository.GetDetailValidateActivityErrorsAsync(validatorTransactionServiceObject.ValidatorTransactionID, ruleVersion, userAuthority, Classification, date, token);
                validatorErrorServiceObjectList = _mapper.Map<List<ValidatorErrorServiceObject>>(validatorErrorEntityCPTList);
            }
            else if (!sequenceCPT && !string.IsNullOrEmpty(CPTS))
            {
                var validatorErrorEntityCPTList = await _validatorErrorRepository.GetAPPValidatorActivityErrorsAsync("", ICDS, CPTS, Classification, date, token, ruleVersion, userAuthority);
                validatorErrorServiceObjectList = _mapper.Map<List<ValidatorErrorServiceObject>>(validatorErrorEntityCPTList);
            }
            if (sequenceICD && validatorICDServiceObjectList != null && validatorICDServiceObjectList.Count > 0)
            {
                var validatorErrorEntityICDList = await _validatorErrorRepository.GetDetailValidateICDErrorsAsync(validatorTransactionServiceObject.ValidatorTransactionID, ruleVersion, userAuthority, Classification, date, token);
                var validatorErrorServiceObjectCPTList = _mapper.Map<List<ValidatorErrorServiceObject>>(validatorErrorEntityICDList);
                await Task.Run(() => validatorErrorServiceObjectList.AddRange(validatorErrorServiceObjectCPTList));
            }
            else if (!sequenceICD && !string.IsNullOrEmpty(ICDS))
            {
                var validatorErrorEntityICDList = await _validatorErrorRepository.GetAPPValidatorICDErrorsAsync("", ICDS, CPTS, Classification, date, token, ruleVersion, userAuthority);
                var validatorErrorServiceObjectCPTList = _mapper.Map<List<ValidatorErrorServiceObject>>(validatorErrorEntityICDList);
                await Task.Run(() => validatorErrorServiceObjectList.AddRange(validatorErrorServiceObjectCPTList));
            }
            if (sequenceCPT && validatorCPTServiceObjectList != null && validatorCPTServiceObjectList.Count > 0)
            {
                var validatorErrorEntityICDCPTList = await _validatorErrorRepository.GetDetailValidateICDCPTErrorsAsync(validatorTransactionServiceObject.ValidatorTransactionID, ruleVersion, userAuthority, Classification, date, token);
                var validatorErrorServiceObjectICDCPTList = _mapper.Map<List<ValidatorErrorServiceObject>>(validatorErrorEntityICDCPTList);
                await Task.Run(() => validatorErrorServiceObjectList.AddRange(validatorErrorServiceObjectICDCPTList));
            }
            else if (!sequenceCPT && !string.IsNullOrEmpty(CPTS))
            {
                var validatorErrorEntityICDCPTList = await _validatorErrorRepository.GetAPPValidatorICDCPTErrorsAsync("", ICDS, CPTS, Classification, date, token, ruleVersion, userAuthority);
                var validatorErrorServiceObjectICDCPTList = _mapper.Map<List<ValidatorErrorServiceObject>>(validatorErrorEntityICDCPTList);
                await Task.Run(() => validatorErrorServiceObjectList.AddRange(validatorErrorServiceObjectICDCPTList));
            }
            validatorErrorServiceObjectList = await GetServiceValidarErrorDescriptAsync(validatorErrorServiceObjectList, token);
            var validatorErrorEntityList = _mapper.Map<List<ValidatorErrorEntity>>(validatorErrorServiceObjectList);
            await _validatorErrorRepository.CreateBulkValidatorErrorAsync(validatorErrorEntityList, token);
            return validatorErrorServiceObjectList;
        }
        public async Task<List<ValidatorErrorServiceObject>> GetValidatorAPPErrorAsync(ValidatorErrorServiceObject validatorErrorServiceObject, string Classification, CancellationToken token)
        {
            string EncryptedCPTS = validatorErrorServiceObject.CPTS.ConvertValidatorInputToCommaSepertedString().EncryptCommaSeperatedCodes();
            string EncryptedICDS = validatorErrorServiceObject.ICDS.ConvertValidatorInputToCommaSepertedString().EncryptCommaSeperatedCodes();
            var getTuple = await _userService.GetUserAuthorityAndRuleVersionAsync(token);
            int userAuthority = getTuple.Item1;
            int ruleVersion = getTuple.Item2;
            DateTime date = DateTime.Now;
            List<ValidatorErrorServiceObject> validatorErrorServiceObjectList = new List<ValidatorErrorServiceObject>();
            if (EncryptedCPTS != null)
            {
                var validatorErrorEntityCPTList = await _validatorErrorRepository.GetAPPValidatorActivityErrorsAsync(validatorErrorServiceObject.CaseNumber, EncryptedICDS, EncryptedCPTS, Classification, date, token, ruleVersion, userAuthority);
                validatorErrorServiceObjectList = _mapper.Map<List<ValidatorErrorServiceObject>>(validatorErrorEntityCPTList);
            }
            if (EncryptedICDS != null)
            {
                var validatorErrorEntityICDList = await _validatorErrorRepository.GetAPPValidatorICDErrorsAsync(validatorErrorServiceObject.CaseNumber, EncryptedICDS, EncryptedCPTS, Classification, date, token, ruleVersion, userAuthority);
                var validatorErrorServiceObjectCPTList = _mapper.Map<List<ValidatorErrorServiceObject>>(validatorErrorEntityICDList);
                await Task.Run(() => validatorErrorServiceObjectList.AddRange(validatorErrorServiceObjectCPTList));
            }
            if (EncryptedCPTS != null || EncryptedCPTS != null && EncryptedICDS != null)
            {
                var validatorErrorEntityICDCPTList = await _validatorErrorRepository.GetAPPValidatorICDCPTErrorsAsync(validatorErrorServiceObject.CaseNumber, EncryptedICDS, EncryptedCPTS, Classification, date, token, ruleVersion, userAuthority);
                var validatorErrorServiceObjectICDCPTList = _mapper.Map<List<ValidatorErrorServiceObject>>(validatorErrorEntityICDCPTList);
                await Task.Run(() => validatorErrorServiceObjectList.AddRange(validatorErrorServiceObjectICDCPTList));
            }
            validatorErrorServiceObjectList = await GetServiceValidarErrorDescriptAsync(validatorErrorServiceObjectList, token);
            var validatorErrorEntityList = _mapper.Map<List<ValidatorErrorEntity>>(validatorErrorServiceObjectList);
            await _validatorErrorRepository.CreateBulkValidatorErrorAsync(validatorErrorEntityList, token);
            return validatorErrorServiceObjectList;
        }
        public async Task<List<ValidatorErrorServiceObject>> GetValidatorErrorAsync(ValidatorErrorServiceObject validatorErrorServiceObject, CancellationToken token)
        {
            string EncryptedCPTS = validatorErrorServiceObject.CPTS.ConvertValidatorInputToCommaSepertedString().EncryptCommaSeperatedCodes();
            string EncryptedICDS = validatorErrorServiceObject.ICDS.ConvertValidatorInputToCommaSepertedString().EncryptCommaSeperatedCodes();
            DateTime date = DateTime.Now;
            List<ValidatorErrorServiceObject> validatorErrorServiceObjectList = new List<ValidatorErrorServiceObject>();
            if (EncryptedCPTS != null)
            {
                var validatorErrorEntityCPTList = await _validatorErrorRepository.GetValidatorActivityErrorsAsync(validatorErrorServiceObject.CaseNumber, EncryptedICDS, EncryptedCPTS, date, token);
                validatorErrorServiceObjectList = _mapper.Map<List<ValidatorErrorServiceObject>>(validatorErrorEntityCPTList);
            }
            if (EncryptedICDS != null)
            {
                var validatorErrorEntityICDList = await _validatorErrorRepository.GetValidatorICDErrorsAsync(validatorErrorServiceObject.CaseNumber, EncryptedICDS, EncryptedCPTS, date, token);
                var validatorErrorServiceObjectCPTList = _mapper.Map<List<ValidatorErrorServiceObject>>(validatorErrorEntityICDList);
                await Task.Run(() => validatorErrorServiceObjectList.AddRange(validatorErrorServiceObjectCPTList));
            }
            if (EncryptedCPTS != null || EncryptedCPTS != null && EncryptedICDS != null)
            {
                var validatorErrorEntityICDCPTList = await _validatorErrorRepository.GetValidatorICDCPTErrorsAsync(validatorErrorServiceObject.CaseNumber, EncryptedICDS, EncryptedCPTS, date, token);
                var validatorErrorServiceObjectICDCPTList = _mapper.Map<List<ValidatorErrorServiceObject>>(validatorErrorEntityICDCPTList);
                await Task.Run(() => validatorErrorServiceObjectList.AddRange(validatorErrorServiceObjectICDCPTList));
            }
            validatorErrorServiceObjectList = await GetServiceValidarErrorDescriptAsync(validatorErrorServiceObjectList, token);
            var validatorErrorEntityList = _mapper.Map<List<ValidatorErrorEntity>>(validatorErrorServiceObjectList);
            await _validatorErrorRepository.CreateBulkValidatorErrorAsync(validatorErrorEntityList, token);
            return validatorErrorServiceObjectList;
        }

        public async Task<List<ValidatorErrorServiceObject>> GetServiceValidarErrorDescriptAsync(List<ValidatorErrorServiceObject> validatorErrorServiceObjectList, CancellationToken token)
        {
            List<ValidatorErrorServiceObject> GetValidatorErrorRequestDtoList = new List<ValidatorErrorServiceObject>();
            if (validatorErrorServiceObjectList != null && validatorErrorServiceObjectList.Count > 0)
            {
                await Task.Run(() =>
                {
                    for (int i = 0; i < validatorErrorServiceObjectList.Count; i++)
                    {
                        var data = validatorErrorServiceObjectList.Skip(i).Take(1).FirstOrDefault();
                        if (data != null)
                        {
                            ValidatorErrorServiceObject item = new ValidatorErrorServiceObject();
                            //try
                            {
                                item.CaseNumber = data.CaseNumber;
                                item.PrefixType = data.PrefixType;
                                item.ValidatedDate = data.ValidatedDate;
                                item.ValidatedBy = data.ValidatedBy;
                                item.ErrorCategory = data.ErrorCategory;
                                item.Status = data.Status;
                                StringBuilder sb = new StringBuilder();
                                if (!string.IsNullOrEmpty(data.ErrorCode1) && data.ErrorCode1.Length > 6)
                                    item.ErrorCode1 = data.ErrorCode1.DecriptCommaSeperatedCodes();
                                if (!string.IsNullOrEmpty(data.ErrorCode2) && data.ErrorCode2.Length > 6)
                                {
                                    item.ErrorCode2 = data.ErrorCode2.DecriptCommaSeperatedCodes();
                                    item.ErrorCode2 = item.ErrorCode2.Length > 99 ? sb.Append(item.ErrorCode2.Substring(1, 99)).Append("..").ToString() : item.ErrorCode2;
                                }
                                if (!string.IsNullOrEmpty(data.Message) && data.Message.IndexOf("COD-") == -1 && data.Message.IndexOf("SUB-") == -1)
                                    item.Message = data.Message.DecriptCommaSeperatedMessage();
                                if (!string.IsNullOrEmpty(data.CodingTips))
                                    item.CodingTips = data.CodingTips.DecriptCommaSeperatedCodes();
                            }
                            //catch (Exception ex)
                            //{

                            //}
                            GetValidatorErrorRequestDtoList.Add(item);
                        }
                    }
                });
            }

            return GetValidatorErrorRequestDtoList;
        }
    }
}
