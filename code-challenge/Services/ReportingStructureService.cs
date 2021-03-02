using challenge.Exceptions;
using challenge.Models;
using challenge.Repositories;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace challenge.Services
{
    public class ReportingStructureService : IReportingStructureService
    {
        private readonly ILogger<IReportingStructureService> _logger;
        private readonly IEmployeeRepository _employeeRepository;

        public ReportingStructureService(ILogger<IReportingStructureService> logger, IEmployeeRepository employeeRepository)
        {
            _logger = logger;
            _employeeRepository = employeeRepository;
        }

        /// <summary>
        /// Finds the number of direct reports for the specified <paramref name="employeeId"/>, 
        /// and it's direct reports recursively.
        /// </summary>
        /// <param name="employeeId">The Id of the Employee.</param>
        /// <returns>ReportingStructure</returns>
        /// <exception cref="EmployeeNotFoundException">If employee Id is invalid.</exception>
        /// <exception cref="Exception">In case of some other error.</exception>
        public ReportingStructure GetByEmployeeId(string employeeId)
        {
            try
            {
                var employeeExists = _employeeRepository.Exists(employeeId);
                if (!employeeExists)
                {
                    throw new EmployeeNotFoundException($"Employee [Id: '{employeeId}'] not found.");
                }

                Employee employee = null;
                // to avoid an extra call to get Employee when building the reporting structure.
                bool isRoot = true; 
                int numberOfReports = 0;
                var queue = new Queue<string>();
                queue.Enqueue(employeeId);

                // BFS.
                while (queue.Any())
                {
                    var id = queue.Dequeue();
                    var temp = _employeeRepository.GetById(id, true);
                    if (isRoot)
                    {
                        employee = temp;
                        isRoot = false;
                    }
                    numberOfReports += temp.DirectReports.Count;

                    foreach (var directReport in temp.DirectReports)
                    {
                        queue.Enqueue(directReport.EmployeeId);
                    }
                }

                var reportingStructure = new ReportingStructure(employee, numberOfReports);

                return reportingStructure;
            }
            catch (EmployeeNotFoundException e)
            {
                _logger.LogError(e.Message);
                throw e;
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return null;
            }
        }

    }
}