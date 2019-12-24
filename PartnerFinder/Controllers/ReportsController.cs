using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PartnerFinder.CustomFilters;
using Service;
using Service.Models;
using Service.Services;

namespace PartnerFinder.Controllers
{
    [ServiceFilter(typeof(ObjectExistenceFilter))]
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ReportsController : CommonBaseController
    {
        private readonly IReportService _reportService;
        private readonly IUnitOfWork _unitOfWork;
        public ReportsController(IReportService reportService, IUnitOfWork unitOfWork)
        {
            _reportService = reportService;
            _unitOfWork = unitOfWork;
        }

        [HttpPost]
        public async Task<IActionResult> CreateOne(ReportDto reportDto)
        {
            var report = new Report();
            report.ReceiverId = reportDto.ReceiverId;
            report.Content = reportDto.Content;
            report.PostId = reportDto.PostId;
            report.Type = reportDto.Type;
            var userId = GetUserId();
            report.SenderId = userId;
            await _reportService.AddOne(report);
            await _unitOfWork.Commit();
            return Ok(new { result = true });
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(int index, int size)
        {
            var reports = await _reportService.GetAll(index, size);
            var count = await _reportService.Count();
            return Ok(new { reports, count });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOne(int id)
        {
            await _reportService.DeleteOne(id);
            await _unitOfWork.Commit();
            return Ok(new { result = true });
        }
    }
}