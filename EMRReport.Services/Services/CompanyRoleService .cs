using AutoMapper;
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
    public sealed class CompanyRoleService : ICompanyRoleService
    {
        private readonly IMapper _mapper;
        private readonly ICompanyRoleRepository _companyRoleRepository;
        public CompanyRoleService(ICompanyRoleRepository companyRoleRepository, IMapper mapper)
        {
            _companyRoleRepository = companyRoleRepository;
            _mapper = mapper;
        }
        public async Task<CompanyRoleServiceObject> GetCompanyRoleByNameAsync(string companyRoleName, CancellationToken token)
        {
            var companyRoleEntity = await _companyRoleRepository.GetRoleByNameAsync(companyRoleName, token);
            return _mapper.Map<CompanyRoleServiceObject>(companyRoleEntity);
        }
        public async Task<ICollection<CompanyRoleServiceObject>> GetCompanyRoleListFromNameListAsync(ICollection<CompanyRoleServiceObject> companyRoleServiceObjectList, CancellationToken token)
        {
            ICollection<CompanyRoleServiceObject> companyRoleServiceObjectResultList = new List<CompanyRoleServiceObject>();
            var companyRoleNameList = companyRoleServiceObjectList.Select(x => x.CompanyRole).Distinct().ToArray();
            for (int i = 0; i < companyRoleNameList.Length; i++)
            {
                var data = companyRoleNameList.Skip(i).Take(1).FirstOrDefault();
                if (data != null)
                {
                    CompanyRoleServiceObject item = new CompanyRoleServiceObject();
                    item = await GetCompanyRoleByNameAsync(data, token);
                    if (item != null)
                        companyRoleServiceObjectResultList.Add(item);
                }
            }
            return companyRoleServiceObjectResultList;
        }
        public async Task<ICollection<CompanyRoleServiceObject>> GetCompanyRoleDDLAsync(CancellationToken token)
        {
            var regulatoryrEntityDDL = await _companyRoleRepository.GetCompanyRoleDDLAsync(token);
            return _mapper.Map<ICollection<CompanyRoleServiceObject>>(regulatoryrEntityDDL);
        }
    }
}
