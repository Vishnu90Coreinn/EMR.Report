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
    public sealed class BasketGroupService : IBasketGroupService
    {
        private readonly IMapper _mapper;
        private readonly IBasketGroupRepository _basketGroupRepository;
        public BasketGroupService(IBasketGroupRepository basketGroupRepository, IMapper mapper)
        {
            _basketGroupRepository = basketGroupRepository;
            _mapper = mapper;
        }
        public async Task<BasketGroupServiceObject> CreateBasketGroupAsync(BasketGroupServiceObject basketGroupServiceObject, CancellationToken token)
        {
            var basketGroupEntity = _mapper.Map<BasketGroupEntity>(basketGroupServiceObject);
            basketGroupEntity = await _basketGroupRepository.CreateBasketGroupAsync(basketGroupEntity, token);
            return _mapper.Map<BasketGroupServiceObject>(basketGroupEntity);
        }
    }
}