using AutoMapper;
using EMRReport.DataContracts.IRepositories;
using EMRReport.ServiceContracts.IServices;
using EMRReport.ServiceContracts.ServiceObjects;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace EMRReport.Services.Services
{
    public class InsuranceClassificationService : IInsuranceClassificationService
    {
        IInsuranceClassificationRepository _insuranceClassificationRepository;
        IMapper _mapper;
        public InsuranceClassificationService(IInsuranceClassificationRepository insuranceClassificationRepository, IMapper mapper)
        {
            _insuranceClassificationRepository = insuranceClassificationRepository;
            _mapper = mapper;
        }
        public async Task<InsuranceClassificationServiceObject> GetInsuranceClassificationByNameAsync(string insuranceClassification, CancellationToken token)
        {
            var insuranceClassificationEntityList = await _insuranceClassificationRepository.GetInsuranceClassificationByNameAsync(insuranceClassification, token);
            var insuranceClassificationServiceObject = _mapper.Map<InsuranceClassificationServiceObject>(insuranceClassificationEntityList);
            return insuranceClassificationServiceObject;
        }
        public async Task<ICollection<InsuranceClassificationServiceObject>> GetInsuranceClassificationDistinctListFromNameListAsync(ICollection<InsuranceClassificationServiceObject> insuranceclassificationServiceObjectList, CancellationToken token)
        {
            ICollection<InsuranceClassificationServiceObject> insuranceClassificationServiceObjectResultList = new List<InsuranceClassificationServiceObject>();
            var insuranceclassificationNameList = insuranceclassificationServiceObjectList.Select(x => x.InsuranceClassificationName).Distinct().ToArray();
            for (int i = 0; i < insuranceclassificationNameList.Length; i++)
            {
                var data = insuranceclassificationNameList.Skip(i).Take(1).FirstOrDefault();
                if (data != null)
                {
                    InsuranceClassificationServiceObject item = new InsuranceClassificationServiceObject();
                    item = await GetInsuranceClassificationByNameAsync(data, token);
                    if (item != null)
                        insuranceClassificationServiceObjectResultList.Add(item);
                }
            }
            return insuranceClassificationServiceObjectResultList;
        }
    }
}
