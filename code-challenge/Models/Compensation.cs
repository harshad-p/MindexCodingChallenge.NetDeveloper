using System;

namespace challenge.Models
{
    public class Compensation
    {
        public string Id { get; set; }
        public string EmployeeId { get; set; }
        public Employee Employee { get; set; }
        public decimal Salary { get; set; }
        public DateTime EffectiveDate { get; set; }

        public override bool Equals(object obj)
        {
            return obj is Compensation compensation
                && Equals(compensation);
        }

        public bool Equals(Compensation compensation)
        {
            return Id == compensation.Id;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        public override string ToString()
        {
            return $"Id: {Id}, " +
                $"Employee Id: {EmployeeId}, " +
                $"Salary: {Salary}, " +
                $"Effective Date: {EffectiveDate}";
        }
    }
}