using challenge.Models;
using System;
using System.Threading.Tasks;

namespace challenge.Repositories
{
    public interface IEmployeeRepository
    {
        bool Exists(string id);
        Employee GetById(String id, bool includeDirectReports = false);
        Employee Add(Employee employee);
        Employee Remove(Employee employee);
        Task SaveAsync();
    }
}