using challenge.Exceptions;
using challenge.Models;
using challenge.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net;

namespace challenge.Controllers
{
    [Route("api/[controller]")]
    public class CompensationController : Controller
    {
        private readonly ILogger _logger;
        private readonly ICompensationService _compensationService;

        public CompensationController(ILogger<CompensationController> logger, ICompensationService compensationService)
        {
            _logger = logger;
            _compensationService = compensationService;
        }

        [HttpGet("{employeeId}", Name = "getCompensationByEmployeeId")]
        public IActionResult Get([FromRoute] string employeeId)
        {
            _logger.LogDebug($"Received Get Compensation request for Employee [Id: {employeeId}].");

            // An Employee can have more than 1 Compensations.
            // For each increment in Salary.
            IEnumerable<CompensationResponse> compensations;
            try
            {
                compensations = _compensationService.GetByEmployeeId(employeeId);
            }
            catch (EmployeeNotFoundException e)
            {
                return StatusCode((int)HttpStatusCode.NotFound, e.Message);
            }
            catch (Exception e)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, e.Message);
            }
            
            return Ok(compensations);
        }

        [HttpPost]
        public IActionResult Create([FromBody] CompensationRequest compensationRequest)
        {
            _logger.LogDebug($"Received Create Compensation request.");

            CompensationResponse compensationResponse;
            try
            {
                compensationResponse = _compensationService.Create(compensationRequest);
            }
            catch (EmployeeNotFoundException e)
            {
                return StatusCode((int)HttpStatusCode.NotFound, e.Message);
            }
            catch (SalaryIsLessThanZeroException e)
            {
                return StatusCode((int)HttpStatusCode.BadRequest, e.Message);
            }
            catch (EffectiveDateCouldNotBeParsedException e)
            {
                return StatusCode((int)HttpStatusCode.BadRequest, e.Message);
            }
            catch (Exception e)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, e.Message);
            }

            return CreatedAtRoute(new { employeeId = compensationRequest.EmployeeId }, compensationResponse);
        }

    }
}