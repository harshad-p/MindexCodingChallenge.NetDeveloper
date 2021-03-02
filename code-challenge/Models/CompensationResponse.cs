using System;

namespace challenge.Models
{
    public class CompensationResponse
    {
        public string Id { get; set; }
        public Employee Employee { get; set; }
        public decimal Salary { get; set; }
        public DateTime EffectiveDate { get; set; }

        public override string ToString()
        {
            return $"Id: {Id}, " +
                $"Salary: {Salary}, " +
                $"Effective Date: {EffectiveDate}";
        }
    }
}