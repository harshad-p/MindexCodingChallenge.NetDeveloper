using challenge.Exceptions;
using challenge.Models;
using challenge.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Net;

namespace challenge.Controllers
{
    [Route("api/[controller]")]
    public class ReportingStructureController : Controller
    {
        private readonly ILogger _logger;
        private readonly IReportingStructureService _reportingStructureService;

        public ReportingStructureController(ILogger<ReportingStructureController> logger, IReportingStructureService reportingStructureService)
        {
            _logger = logger;
            _reportingStructureService = reportingStructureService;
        }

        [HttpGet("{id}", Name = "getReportingStructureByEmployeeId")]
        public IActionResult Get(string id)
        {
            _logger.LogDebug($"Received Get Reporting Structure request for Employee [Id: {id}].");

            ReportingStructure reportingStructure;
            
            try
            {
                reportingStructure = _reportingStructureService.GetByEmployeeId(id);
            }
            catch (EmployeeNotFoundException e)
            {
                return StatusCode((int)HttpStatusCode.NotFound, e.Message);
            }
            catch (Exception e)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, e.Message);
            }

            return Ok(reportingStructure);
        }
    }
}