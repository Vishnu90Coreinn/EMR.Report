using AutoMapper;
using EMRReport.Common.Extensions;
using EMRReport.DataContracts.IRepositories;
using EMRReport.ServiceContracts.IServices;
using EMRReport.ServiceContracts.ServiceObjects;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace EMRReport.Services.Services
{
    public class DashBoardService : IDashBoardService
    {
        IDashBoardRepository _dashBoardRepository;
        IMapper _mapper;
        public DashBoardService(IDashBoardRepository dashBoardRepository, IMapper mapper)
        {
            _dashBoardRepository = dashBoardRepository;
            _mapper = mapper;
        }

        public async Task<List<DashBoardServiceObject>> GetEncounterWiseAsync(DashBoardServiceObject dashBoardServiceObject, CancellationToken token)
        {
            var dateFromTo = dashBoardServiceObject.DateRange.ConvertDateRangeStringToDateTimes();
            DateTime? dateFrom = null;
            DateTime? dateTo = null;
            int type = 1;
            if (dateFromTo != null && dateFromTo.Item1.HasValue && dateFromTo.Item2.HasValue)
            {
                dateFrom = dateFromTo.Item1;
                dateTo = dateFromTo.Item2.Value.AddDays(1);
                type = 1;
            }
            else
            {
                dateTo = DateTime.Now.AddDays(1);
                dateFrom = dateTo.Value.AddYears(-1);
                type = 2;
            }

            var dashBoardReportEntity = await _dashBoardRepository.GetEncounterWiseAsync(dateFrom, dateTo, type, token);
            return _mapper.Map<List<DashBoardServiceObject>>(dashBoardReportEntity);
        }

        public async Task<List<DashBoardServiceObject>> GeErrorCategoryWiseAsync(DashBoardServiceObject dashBoardServiceObject, CancellationToken token)
        {
            var dateFromTo = dashBoardServiceObject.DateRange.ConvertDateRangeStringToDateTimes();
            DateTime? dateFrom = null;
            DateTime? dateTo = null;
            int type = 1;
            if (dateFromTo != null && dateFromTo.Item1.HasValue && dateFromTo.Item2.HasValue)
            {
                dateFrom = dateFromTo.Item1;
                dateTo = dateFromTo.Item2.Value.AddDays(1);
                type = 1;
            }
            else
            {
                dateTo = DateTime.Now.AddDays(1);
                dateFrom = dateTo.Value.AddYears(-1);
                type = 2;
            }

            var dashBoardReportEntity = await _dashBoardRepository.GeErrorCategoryWiseAsync(dateFrom, dateTo, type, token);
            return _mapper.Map<List<DashBoardServiceObject>>(dashBoardReportEntity);
        }

        public async Task<List<DashBoardServiceObject>> GeErrorSummaryWiseAsync(DashBoardServiceObject dashBoardServiceObject, CancellationToken token)
        {
            var dateFromTo = dashBoardServiceObject.DateRange.ConvertDateRangeStringToDateTimes();
            DateTime? dateFrom = null;
            DateTime? dateTo = null;
            int type = 1;
            if (dateFromTo != null && dateFromTo.Item1.HasValue && dateFromTo.Item2.HasValue)
            {
                dateFrom = dateFromTo.Item1;
                dateTo = dateFromTo.Item2.Value.AddDays(1);
                type = 1;
            }
            else
            {
                dateTo = DateTime.Now.AddDays(1);
                dateFrom = dateTo.Value.AddYears(-1);
                type = 2;
            }
            var dashBoardReportEntity = await _dashBoardRepository.GeErrorSummaryWiseAsync(dateFrom, dateTo, type, token);
            return _mapper.Map<List<DashBoardServiceObject>>(dashBoardReportEntity);
        }
        public async Task<List<DashBoardServiceObject>> GeClaimCounterAsync(DashBoardServiceObject dashBoardServiceObject, CancellationToken token)
        {
            var dateFromTo = dashBoardServiceObject.DateRange.ConvertDateRangeStringToDateTimes();
            DateTime? dateFrom = null;
            DateTime? dateTo = null;
            int type = 1;
            if (dateFromTo != null && dateFromTo.Item1.HasValue && dateFromTo.Item2.HasValue)
            {
                dateFrom = dateFromTo.Item1;
                dateTo = dateFromTo.Item2.Value.AddDays(1);
                type = 1;
            }
            else
            {
                dateTo = DateTime.Now.AddDays(1);
                dateFrom = dateTo.Value.AddYears(-1);
                type = 2;
            }
            var dashBoardReportEntity = await _dashBoardRepository.GeClaimCounterAsync(dateFrom, dateTo, type, token);
            return _mapper.Map<List<DashBoardServiceObject>>(dashBoardReportEntity);
        }
    }
}
