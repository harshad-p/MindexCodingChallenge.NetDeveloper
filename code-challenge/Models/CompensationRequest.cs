namespace challenge.Models
{
    public class CompensationRequest
    {
        public string EmployeeId { get; set; }
        public decimal Salary { get; set; }

        /// <summary>
        /// In the format: MM-dd-yyyy
        /// For example: 03-01-2021
        /// </summary>
        public string EffectiveDate { get; set; }

        public override string ToString()
        {
            return $"EmployeeId: {EmployeeId}, " +
                $"Salary: {Salary}, " +
                $"Effective Date: {EffectiveDate}";
        }
    }
}