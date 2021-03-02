using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using challenge.Models;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using challenge.Data;

namespace challenge.Repositories
{
    public class CompensationRespository : ICompensationRespository
    {
        private readonly ILogger<ICompensationRespository> _logger;
        private readonly EmployeeContext _employeeContext;

        public CompensationRespository(ILogger<ICompensationRespository> logger, EmployeeContext employeeContext)
        {
            _employeeContext = employeeContext;
            _logger = logger;
        }

        public bool Exists(string id)
        {
            return _employeeContext.Compensations.Any(c => c.Id == id);
        }

        public IEnumerable<Compensation> GetAllForEmployee(string employeeId, bool includeEmployees = false)
        {
            var queryableCompensations = _employeeContext.Compensations;
            if (includeEmployees)
            {
                return queryableCompensations.Include(rel => rel.Employee).Where(c => c.EmployeeId == employeeId).ToList();
            }
            return queryableCompensations.Where(c => c.EmployeeId == employeeId).ToList();
        }

        public Compensation GetById(string id, bool includeEmployee = false)
        {
            var queryableCompensations = _employeeContext.Compensations;
            if (includeEmployee)
            {
                return queryableCompensations.Include(rel => rel.Employee).SingleOrDefault(c => c.Id == id);
            }
            return queryableCompensations.SingleOrDefault(c => c.Id == id);
        }

        public Compensation Add(Compensation compensation)
        {
            compensation.Id = Guid.NewGuid().ToString();
            _employeeContext.Compensations.Add(compensation);
            return compensation;
        }

        public Compensation Remove(Compensation compensation)
        {
            return _employeeContext.Remove(compensation).Entity;
        }

        public Task SaveAsync()
        {
            return _employeeContext.SaveChangesAsync();
        }

    }
}
