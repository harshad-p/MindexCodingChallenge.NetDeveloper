using challenge.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace challenge.Repositories
{
    public interface ICompensationRespository
    {
        bool Exists(string id);
        Compensation GetById(string id, bool includeEmployee = false);
        IEnumerable<Compensation> GetAllForEmployee(string employeeId, bool includeEmployees = false);
        Compensation Add(Compensation compensation);
        Compensation Remove(Compensation compensation);
        Task SaveAsync();
    }
}