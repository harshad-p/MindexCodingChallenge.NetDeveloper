using challenge.Exceptions;
using challenge.Models;
using challenge.Repositories;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

namespace challenge.Services
{
    public class CompensationService : ICompensationService
    {
        private readonly ILogger<ICompensationService> _logger;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly ICompensationRespository _compensationRespository;

        public CompensationService(ILogger<ICompensationService> logger,
                                   IEmployeeRepository employeeRepository,
                                   ICompensationRespository compensationRespository)
        {
            _logger = logger;
            _employeeRepository = employeeRepository;
            _compensationRespository = compensationRespository;
        }

        /// <summary>
        /// Retrieves all the available Compensations for an employee.
        /// </summary>
        /// <param name="employeeId">The Id of the employee.</param>
        /// <returns>Collection of Compensations.</returns>
        /// <exception cref="EmployeeNotFoundException">If employee Id is invalid.</exception>
        /// <exception cref="Exception">In case of some other error.</exception>
        public IEnumerable<CompensationResponse> GetByEmployeeId(string employeeId)
        {
            try
            {
                var employeeExists = _employeeRepository.Exists(employeeId);
                if (!employeeExists)
                {
                    throw new EmployeeNotFoundException($"Employee [Id: '{employeeId}'] not found.");
                }

                var compensations = _compensationRespository.GetAllForEmployee(employeeId);
                var employee = _employeeRepository.GetById(employeeId);

                var compensationsResponse = new HashSet<CompensationResponse>();

                foreach(var compensation in compensations)
                {
                    compensationsResponse.Add(BuildCompensationResponse(compensation, employee));
                }

                return compensationsResponse;
            }
            catch (EmployeeNotFoundException e)
            {
                _logger.LogError(e.Message);
                throw e;
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                throw new Exception("Something went wrong!");
            }
        }

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
        public CompensationResponse Create(CompensationRequest compensationRequest)
        {
            try
            {
                if (compensationRequest == null)
                {
                    throw new Exception("Compensation not provided.");
                }

                var employeeExists = _employeeRepository.Exists(compensationRequest.EmployeeId);
                if (!employeeExists)
                {
                    throw new EmployeeNotFoundException($"Employee [Id: '{compensationRequest.EmployeeId}'] not found.");
                }

                if (compensationRequest.Salary < 0)
                {
                    throw new SalaryIsLessThanZeroException($"Salary [{compensationRequest.Salary}] cannot be Negative.");
                }

                string pattern = "MM-dd-yyyy";
                var isValidDate = DateTime.TryParseExact(compensationRequest.EffectiveDate, pattern, null, System.Globalization.DateTimeStyles.None, out DateTime effectiveDate);
                if (!isValidDate)
                {
                    throw new EffectiveDateCouldNotBeParsedException($"Invalid Effective Date [{compensationRequest.EffectiveDate}] format. Should be of the form: {pattern}.");
                }

                var compensation = new Compensation
                {
                    EffectiveDate = effectiveDate,
                    EmployeeId = compensationRequest.EmployeeId,
                    Salary = compensationRequest.Salary
                };

                _compensationRespository.Add(compensation);
                _compensationRespository.SaveAsync().Wait();

                var compensationResponse = BuildCompensationResponse(compensation);

                return compensationResponse;
            }
            catch (EmployeeNotFoundException e)
            {
                _logger.LogError(e.Message);
                throw e;
            }
            catch (SalaryIsLessThanZeroException e)
            {
                _logger.LogError(e.Message);
                throw e;
            }
            catch (EffectiveDateCouldNotBeParsedException e)
            {
                _logger.LogError(e.Message);
                throw e;
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                throw new Exception($"Something went wrong!. | {e.Message}");
            }
        }

        private CompensationResponse BuildCompensationResponse(Compensation compensation)
        {
            var employee = _employeeRepository.GetById(compensation.EmployeeId);
            return BuildCompensationResponse(compensation, employee);
        }

        private CompensationResponse BuildCompensationResponse(Compensation compensation, Employee employee)
        {
            return new CompensationResponse
            {
                Id = compensation.Id,
                EffectiveDate = compensation.EffectiveDate,
                Employee = employee,
                Salary = compensation.Salary
            };
        }

    }
}
