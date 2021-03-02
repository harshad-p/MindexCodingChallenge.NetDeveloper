using challenge.Exceptions;
using challenge.Models;
using System;

namespace challenge.Services
{
    public interface IReportingStructureService
    {
        /// <summary>
        /// Finds the number of direct reports for the specified <paramref name="employeeId"/>, 
        /// and it's direct reports recursively.
        /// </summary>
        /// <param name="employeeId">The Id of the Employee.</param>
        /// <returns>ReportingStructure</returns>
        /// <exception cref="EmployeeNotFoundException">If employee Id is invalid.</exception>
        /// <exception cref="Exception">In case of some other error.</exception>
        ReportingStructure GetByEmployeeId(string employeeId);
    }
}