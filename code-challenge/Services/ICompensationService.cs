using challenge.Models;
using System.Collections.Generic;

namespace challenge.Services
{
    public interface ICompensationService
    {
        /// <summary>
        /// Retrieves all the available Compensations for an employee.
        /// </summary>
        /// <param name="employeeId">The Id of the employee.</param>
        /// <returns>Collection of Compensations.</returns>
        /// <exception cref="EmployeeNotFoundException">If employee Id is invalid.</exception>
        /// <exception cref="Exception">In case of some other error.</exception>
        IEnumerable<CompensationResponse> GetByEmployeeId(string employeeId);

        /// <summary>
        /// Saves a Compensation for an Employee.
        /// </summary>
        /// <param name="compensationRequest">The Compensation to be created.</param>
        /// <returns>null: in case of an error; 
        /// Compensation: otherwise.</returns>
        /// <exception cref="EmployeeNotFoundException">If employee Id is invalid.</exception>
        /// <exception cref="SalaryIsLessThanZeroException">If salary is invalid.</exception>
        /// <exception cref="EffectiveDateCouldNotBeParsedException">If effective date is not in the right format.</exception>
        /// <exception cref="Exception">In case of some other error.</exception>
        CompensationResponse Create(CompensationRequest compensationRequest);
    }
}